namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Represents the response returned after successfully updating a sale.
/// Contains all updated details of the sale transaction.
/// </summary>
public record UpdateSaleResponse
{
    /// <summary>
    /// Gets the unique identifier of the updated sale.
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid SaleId { get; init; }

    /// <summary>
    /// Gets the business number assigned to the sale.
    /// </summary>
    /// <example>SALE-2023-0042</example>
    public string SaleNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets the date and time when the sale was originally created.
    /// </summary>
    /// <example>2023-06-15T14:32:00Z</example>
    public DateTime SaleDate { get; init; }

    /// <summary>
    /// Gets the current status of the sale.
    /// Possible values: "Pending", "Completed", "Cancelled".
    /// </summary>
    /// <example>Completed</example>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Gets the total monetary amount of the sale after updates.
    /// </summary>
    /// <example>249.99</example>
    public decimal TotalAmount { get; init; }

    /// <summary>
    /// Gets the currency code in which the sale amounts are expressed.
    /// </summary>
    /// <example>USD</example>
    public string Currency { get; init; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier of the customer associated with the sale.
    /// </summary>
    /// <example>9e725052-4b5a-4a9f-ac20-8c5a8e4f4e4b</example>
    public Guid CustomerId { get; init; }

    /// <summary>
    /// Gets the full name of the customer who made the purchase.
    /// </summary>
    /// <example>John Doe</example>
    public string CustomerName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier of the branch where the sale was processed.
    /// </summary>
    /// <example>7d2e8e1a-5e9b-4a7c-9e8f-3b6a5d4c3b2a</example>
    public Guid BranchId { get; init; }

    /// <summary>
    /// Gets the name of the branch where the sale was processed.
    /// </summary>
    /// <example>Downtown Store</example>
    public string BranchName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the collection of items included in the updated sale.
    /// </summary>
    public List<UpdateSaleItemResponse> Items { get; init; } = new();
}

/// <summary>
/// Represents an individual item within an updated sale response.
/// Contains detailed information about each product in the sale.
/// </summary>
public record UpdateSaleItemResponse
{
    /// <summary>
    /// Gets the unique identifier of the sale item.
    /// </summary>
    /// <example>a3d8f7e2-4c1b-4e95-8f3a-6d2c1b5e4f9a</example>
    public Guid ItemId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the product in this sale item.
    /// </summary>
    /// <example>c7f4e8d2-1a5b-4e3c-9d8f-7a6b5c4d3e2f</example>
    public Guid ProductId { get; init; }

    /// <summary>
    /// Gets the name of the product purchased in this item.
    /// </summary>
    /// <example>Premium Beer 350ml</example>
    public string ProductName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the quantity of the product purchased in this item.
    /// </summary>
    /// <example>6</example>
    public int Quantity { get; init; }

    /// <summary>
    /// Gets the price per unit of the product at time of sale.
    /// </summary>
    /// <example>9.99</example>
    public decimal UnitPrice { get; init; }

    /// <summary>
    /// Gets the total price for this item (Quantity × UnitPrice).
    /// </summary>
    /// <example>59.94</example>
    public decimal TotalPrice { get; init; }

    /// <summary>
    /// Gets the currency code in which the item prices are expressed.
    /// </summary>
    /// <example>USD</example>
    public string Currency { get; init; } = string.Empty;
}