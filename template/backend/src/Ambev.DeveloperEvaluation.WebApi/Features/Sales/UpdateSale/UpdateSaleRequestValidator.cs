using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validates the UpdateSaleRequest according to business rules and data integrity requirements.
/// </summary>
/// <remarks>
/// Performs validation on:
/// - Required fields
/// - Field lengths and formats
/// - Business rule compliance
/// - Relationship integrity
/// </remarks>
public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleRequestValidator class.
    /// Configures all validation rules for updating a sale.
    /// </summary>
    public UpdateSaleRequestValidator()
    {
        // Sale ID must always be provided
        RuleFor(x => x.SaleId)
            .NotEmpty().WithMessage("Sale ID is required");

        // Sale number validation (when provided)
        RuleFor(x => x.SaleNumber)
            .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters")
            .When(x => x.SaleNumber != null);

        // Customer ID validation (when provided)
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required")
            .When(x => x.CustomerId.HasValue);

        // Branch ID validation (when provided)
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required")
            .When(x => x.BranchId.HasValue);

        // Items collection validation
        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                // Product ID is required for each item
                item.RuleFor(i => i.ProductId)
                    .NotEmpty().WithMessage("Product ID is required for all items");

                // Quantity must be positive
                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0");
            });
    }
}