namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents a request to add an item to a sale.
/// </summary>
public record CreateSaleItemRequest
{
    /// <summary>
    /// Gets the unique identifier of the product to be added to the sale.
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// Gets the quantity of the product to be purchased.
    /// </summary>
    public int Quantity { get; init; }
}