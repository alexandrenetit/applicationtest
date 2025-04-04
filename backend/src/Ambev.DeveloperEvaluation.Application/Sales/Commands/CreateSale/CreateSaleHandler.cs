﻿using Ambev.DeveloperEvaluation.Common.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;

/// <summary>
/// Handles the creation of a new sale transaction
/// </summary>
public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleService _saleService;
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IEventNotification _eventNotifier;
    private readonly SaleItemLimitSpecification _saleItemLimitSpecification;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSaleCommandHandler(
        ISaleService saleService,
        ISaleRepository saleRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        IMapper mapper,
        IValidator<CreateSaleCommand> validator,
        IEventNotification eventNotifier,
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
        _saleItemLimitSpecification = saleItemLimitSpecification;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the sale creation process including validation, business rules, and persistence
    /// </summary>
    /// <param name="command">The sale creation command containing customer, branch, and product information</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result containing either the created sale response or validation/domain errors</returns>
    public async Task<CreateSaleResult> Handle(
        CreateSaleCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
        if (customer == null)
            throw new DomainException($"Customer with Id:{command.CustomerId} not found");

        var branch = await _branchRepository.GetByIdAsync(command.BranchId);
        if (branch == null)
            throw new DomainException($"Branch with Id:{command.CustomerId} not found");

        // Create sale via domain service
        var sale = _saleService.CreateSale(customer, branch, command.SaleNumber);

        // Load products
        var productIds = command.Items.Select(i => i.ProductId).ToList();
        var products = await _productRepository.GetByIdsAsync(productIds);

        if (products.Count != productIds.Count)
        {
            var foundProductIds = products.Select(p => p.Id).ToList();
            var missingProductIds = productIds.Except(foundProductIds).ToList();

            throw new DomainException($"Products not found: {string.Join(", ", missingProductIds)}");
        }

        // Prepare items for domain service
        var itemsToAdd = command.Items
            .Select(item => (
                products.First(p => p.Id == item.ProductId),
                item.Quantity
            ));

        if (_saleItemLimitSpecification.IsSatisfiedBy(sale))
        {
            throw new DomainException($"Sale item limit exceeded");
        }

        // Add items via domain service
        _saleService.AddItemsToSale(sale, itemsToAdd);

        // Persist the sale
        await _saleRepository.CreateAsync(sale);

        await _unitOfWork.ApplyChangesAsync(cancellationToken);

        // Notify about the sale creation event
        await _eventNotifier.NotifyAsync(SaleCreatedEvent.CreateFrom(sale));

        // Map to result
        return _mapper.Map<CreateSaleResult>(sale);
    }
}