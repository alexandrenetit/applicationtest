using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;

/// <summary>
/// Validator for the update sale command
/// </summary>
public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty().WithMessage("Sale ID is required");

        RuleFor(x => x.SaleNumber)
            .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters")
            .When(x => x.SaleNumber != null);

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required")
            .When(x => x.CustomerId.HasValue);

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required")
            .When(x => x.BranchId.HasValue);

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty().WithMessage("Product ID is required for all items");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0");
            });
    }
}