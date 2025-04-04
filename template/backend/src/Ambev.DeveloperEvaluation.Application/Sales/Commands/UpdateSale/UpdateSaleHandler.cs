using Ambev.DeveloperEvaluation.Common.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;

/// <summary>
/// Handles the updating of an existing sale transaction
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleService _saleService;
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IEventNotification _eventNotifier;
    private readonly SaleUpdateAllowedSpecification _saleUpdateAllowedSpecification;
    private readonly SaleItemLimitSpecification _saleItemLimitSpecification;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSaleHandler(
        ISaleService saleService,
        ISaleRepository saleRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        IMapper mapper,
        IEventNotification eventNotifier,
        SaleUpdateAllowedSpecification saleUpdateAllowedSpecification,
        SaleItemLimitSpecification saleItemLimitSpecification,
        IUnitOfWork unitOfWork)
    {
        _saleService = saleService;
        _saleRepository = saleRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _eventNotifier = eventNotifier;
        _saleUpdateAllowedSpecification = saleUpdateAllowedSpecification;
        _saleItemLimitSpecification = saleItemLimitSpecification;
        _unitOfWork = unitOfWork;
    }

    // Update the Handle method to use the conversion method
    public async Task<UpdateSaleResult> Handle(
        UpdateSaleCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Retrieve the existing sale
        var sale = await _saleRepository.GetByIdAsync(command.SaleId);
        if (sale == null)
            throw new DomainException($"Sale with Id:{command.SaleId} not found");

        // Check if sale can be updated
        if (!_saleUpdateAllowedSpecification.IsSatisfiedBy(sale))
            throw new DomainException("This sale cannot be updated due to its current status which is cancelled");

        // Update sale properties if provided
        if (command.SaleNumber != null)
            sale.SaleNumber = command.SaleNumber;

        // Update customer if provided
        Customer? customer = null;
        if (command.CustomerId.HasValue)
        {
            customer = await _customerRepository.GetByIdAsync(command.CustomerId.Value);
            if (customer == null)
                throw new DomainException($"Customer with Id:{command.CustomerId} not found");
        }

        // Update branch if provided
        Branch? branch = null;
        if (command.BranchId.HasValue)
        {
            branch = await _branchRepository.GetByIdAsync(command.BranchId.Value);
            if (branch == null)
                throw new InvalidOperationException($"Branch with Id:{command.BranchId} not found");
        }

        // Update sale items and check for inconsistencies
        var saleItems = await ConvertToSaleItemsAsync(command.Items);
        _saleService.UpdateSale(sale, customer, branch, saleItems);

        // Check if sale satisfies item limit after changes
        if (_saleItemLimitSpecification.IsSatisfiedBy(sale))
            throw new DomainException("Sale item limit exceeded");

        // Persist the updated sale
        await _saleRepository.UpdateAsync(sale);

        await _unitOfWork.ApplyChangesAsync(cancellationToken);

        // Notify about the update
        await _eventNotifier.NotifyAsync(SaleModifiedEvent.CreateFrom(sale));

        // Map to result
        return _mapper.Map<UpdateSaleResult>(sale);
    }

    // Add a method to convert UpdateSaleItemCommand to (Product product, int quantity)
    private async Task<IEnumerable<(Product product, int quantity)>> ConvertToSaleItemsAsync(IEnumerable<UpdateSaleItemCommand> items)
    {
        var saleItems = new List<(Product product, int quantity)>();
        foreach (var item in items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new DomainException($"Product with Id:{item.ProductId} not found");

            saleItems.Add((product, item.Quantity));
        }
        return saleItems;
    }
}