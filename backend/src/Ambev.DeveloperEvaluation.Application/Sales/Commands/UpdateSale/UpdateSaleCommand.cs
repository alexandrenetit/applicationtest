using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;

/// <summary>
/// Command to update an existing sale
/// </summary>
public record UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    /// <summary>
    /// Identifier of the sale to update
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// New sale number (optional)
    /// </summary>
    public string? SaleNumber { get; set; }

    /// <summary>
    /// New customer ID (optional)
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// New branch ID (optional)
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Items to update in the sale
    /// </summary>
    public List<UpdateSaleItemCommand> Items { get; set; } = new();

    /// <summary>
    /// New sale status (optional)
    /// </summary>
    public SaleStatus? Status { get; set; }
}