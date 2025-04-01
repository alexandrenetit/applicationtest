namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents the response after adding an item to a sale, containing all relevant item details.
/// </summary>
public record CreateSaleItemRespose
{
    /// <summary>
    /// Gets the unique identifier assigned to this sale item.
    /// </summary>
    public Guid ItemId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the product associated with this sale item.
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// Gets the name of the product associated with this sale item.
    /// </summary>
    public string ProductName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the quantity of the product purchased in this sale item.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// Gets the price per unit of the product in this sale item.
    /// </summary>
    public decimal UnitPrice { get; init; }

    /// <summary>
    /// Gets the total price for this sale item (Quantity × UnitPrice).
    /// </summary>
    public decimal TotalPrice { get; init; }

    /// <summary>
    /// Gets the currency code in which the prices are expressed.
    /// </summary>
    public string Currency { get; init; } = string.Empty;
}