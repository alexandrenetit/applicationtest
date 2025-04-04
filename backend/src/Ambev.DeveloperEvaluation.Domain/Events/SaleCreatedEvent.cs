using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event to notify when sale was created.
/// </summary>
public class SaleCreatedEvent
{
    /// <summary>
    /// Unique identifier of the created sale
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Sale number/identifier
    /// </summary>
    public string SaleNumber { get; set; }

    /// <summary>
    /// Date and time when the sale was created
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Customer identifier
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Branch identifier where the sale was made
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Total amount of the sale
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Currency of the total amount
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Status of the sale
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Items in the sale
    /// </summary>
    public required ICollection<SaleProduct> Products { get; set; }

    public static SaleCreatedEvent CreateFrom(Sale sale)
    {
        if (sale == null)
            throw new ArgumentNullException(nameof(sale));        

        return new SaleCreatedEvent
        {
            SaleId = sale.Id,
            SaleNumber = sale.SaleNumber ?? string.Empty,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            BranchId = sale.BranchId,
            TotalAmount = sale.TotalAmount?.Amount ?? 0m,
            Currency = sale.TotalAmount?.Currency ?? "USD",
            Status = sale.Status.ToString(),
            Products = sale.Items.Select(item => new SaleProduct
            {
                SaleItemId = item.Id,
                ProductId = item.Product?.Id ?? Guid.Empty,
                ProductName = item.Product?.Name ?? "Unknown Product",
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice?.Amount ?? 0m,
                Currency = item.UnitPrice?.Currency ?? "USD",
                TotalAmount = item.TotalAmount?.Amount ?? 0m
            }).ToList()
        };
    }
}

/// <summary>
/// Information about a sale item for messaging
/// </summary>
public record SaleProduct
{
    /// <summary>
    /// Unique identifier of the sale item
    /// </summary>
    public Guid SaleItemId { get; set; }

    /// <summary>
    /// Product identifier
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Currency of the unit price
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Total amount for this item
    /// </summary>
    public decimal TotalAmount { get; set; }
}