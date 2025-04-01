namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;

public record CreateSaleResponse
{
    public Guid SaleId { get; init; }
    public string SaleNumber { get; init; } = string.Empty;
    public DateTime SaleDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public Guid CustomerId { get; init; }
    public string CustomerName { get; init; } = string.Empty; 
    public Guid BranchId { get; init; }
    public string BranchName { get; init; } = string.Empty; 
    public List<CreateSaleItemResponse> Items { get; init; } = new();
}