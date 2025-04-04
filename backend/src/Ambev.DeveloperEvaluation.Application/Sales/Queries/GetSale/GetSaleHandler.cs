using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSale;

/// <summary>
/// 
/// </summary>
public class GetSaleSaleHandler : IRequestHandler<GetSaleCommand, SaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public GetSaleSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the retrieval of a sale by its unique identifier.
    /// </summary>
    /// <param name="command">The command containing the sale ID to retrieve.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>The result containing the sale details.</returns>
    /// <exception cref="ValidationException">Thrown when the command validation fails.</exception>
    /// <exception cref="DomainException">Thrown when the sale or related entities are not found or cannot be updated.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the branch is not found.</exception>
    public async Task<SaleResult> Handle(GetSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new GetSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Retrieve the existing sale
        var sale = await _saleRepository.GetByIdAsync(command.Id);
        if (sale == null)
            throw new DomainException($"Sale with Id:{command.Id} not found");

        // Map to result
        return _mapper.Map<SaleResult>(sale);
    }
}