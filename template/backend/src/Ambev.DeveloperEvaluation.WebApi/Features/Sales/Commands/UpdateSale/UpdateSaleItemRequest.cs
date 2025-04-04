namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Commands.UpdateSale;

/// <summary>
/// Represents a request to update or add an item to an existing sale.
/// </summary>
public record UpdateSaleItemRequest
{
    /// <summary>
    /// Gets the unique identifier of the existing sale item when updating an item.
    /// Null when adding a new item to the sale.
    /// </summary>
    public Guid? SaleItemId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the product associated with this sale item.
    /// </summary>
    /// <remarks>
    /// This must reference an existing product in the system.
    /// </remarks>
    public Guid ProductId { get; init; }

    /// <summary>
    /// Gets the quantity of the product to be purchased in this item.
    /// </summary>
    /// <remarks>
    /// Must be a positive integer value greater than zero.
    /// </remarks>
    public int Quantity { get; init; }
}