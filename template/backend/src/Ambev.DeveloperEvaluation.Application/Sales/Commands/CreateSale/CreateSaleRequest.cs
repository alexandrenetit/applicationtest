using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;

public record CreateSaleRequest
{
    public Guid CustomerId { get; init; }
    public Guid BranchId { get; init; }
    public string? SaleNumber { get; init; }
    public List<CreateSaleItemRequest> Items { get; init; } = new();
}
