using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;

/// <summary>
/// Command to create a new sale in the system.
/// </summary>
public record CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// Gets the unique identifier of the customer associated with this sale.
    /// </summary>
    public Guid CustomerId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the branch where the sale is being processed.
    /// </summary>
    public Guid BranchId { get; init; }

    /// <summary>
    /// Gets the optional sale number that can be assigned to this sale.
    /// If not provided, the system will generate one automatically.
    /// </summary>
    public string? SaleNumber { get; init; }

    /// <summary>
    /// Gets the collection of items included in this sale.
    /// </summary>
    public List<CreateSaleItemCommand> Items { get; init; } = new();
}