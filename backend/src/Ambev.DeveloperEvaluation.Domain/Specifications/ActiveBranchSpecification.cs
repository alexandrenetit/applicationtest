using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class ActiveBranchSpecification : ISpecification<Branch>
{
    public bool IsSatisfiedBy(Branch branch)
    {
        return branch.Status == BranchStatus.Active;
    }
}