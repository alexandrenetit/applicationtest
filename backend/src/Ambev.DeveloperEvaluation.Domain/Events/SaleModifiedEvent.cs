using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event representing a sale update
/// </summary>
public class SaleModifiedEvent
{
    public Guid SaleId { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public SaleStatus Status { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public List<SaleItemModifiedEvent> Items { get; set; } = new();

    // Parameterless constructor for serialization
    public SaleModifiedEvent()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    [JsonConstructor]
    public SaleModifiedEvent(
        Guid saleId,
        string saleNumber,
        Guid customerId,
        Guid branchId,
        SaleStatus status,
        decimal totalAmount,
        string currency,
        List<SaleItemModifiedEvent> items)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        CustomerId = customerId;
        BranchId = branchId;
        Status = status;
        TotalAmount = totalAmount;
        Currency = currency;
        Items = items;
        UpdatedAt = DateTime.UtcNow;
    }

    public static SaleModifiedEvent CreateFrom(Sale sale)
    {
        if (sale == null)
            throw new ArgumentNullException(nameof(sale));

        var items = sale.Items.Select(item => new SaleItemModifiedEvent
        {
            ItemId = item.Id,
            ProductId = item.ProductId,
            ProductName = item.Product?.Name ?? string.Empty,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice.Amount,
            Discount = item.Discount,
            TotalAmount = item.TotalAmount.Amount
        }).ToList();

        return new SaleModifiedEvent(
            sale.Id,
            sale.SaleNumber,
            sale.CustomerId,
            sale.BranchId,
            sale.Status,
            sale.TotalAmount.Amount,
            sale.TotalAmount.Currency,
            items);
    }
}

/// <summary>
/// Item details for the sale updated event
/// </summary>
public class SaleItemModifiedEvent
{
    public Guid ItemId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
}