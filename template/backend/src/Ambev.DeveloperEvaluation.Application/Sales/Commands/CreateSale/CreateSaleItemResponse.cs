namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;

public record CreateSaleItemResponse
{
    public Guid ItemId { get; init; } 
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;    
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal TotalPrice { get; init; }
    public string Currency { get; init; } = string.Empty;
}