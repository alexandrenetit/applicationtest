namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;

/// <summary>
/// Command representing an individual item to be added to a sale.
/// </summary>
public record CreateSaleItemCommand
{
    /// <summary>
    /// Gets the unique identifier of the product being sold.
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// Gets the quantity of the product being purchased.
    /// </summary>
    public int Quantity { get; init; }
}