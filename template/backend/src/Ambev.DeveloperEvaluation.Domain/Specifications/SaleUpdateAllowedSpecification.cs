using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

/// <summary>
/// Specification to check if a sale can be updated
/// </summary>
public class SaleUpdateAllowedSpecification
{
    public bool IsSatisfiedBy(Sale sale)
    {
        // Cannot update cancelled sales
        if (sale.Status == SaleStatus.Cancelled)
            return false;

        return true;
    }
}