using MediatR;
using OneOf.Types;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;

public record CreateSaleCommand : IRequest<CreateSaleResponse>
{
    public Guid CustomerId { get; init; }
    public Guid BranchId { get; init; }
    public string? SaleNumber { get; init; }
    public List<CreateSaleItemRequest> Items { get; init; } = new();
}