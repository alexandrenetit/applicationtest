namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;

public record CreateSaleItemRequest
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}