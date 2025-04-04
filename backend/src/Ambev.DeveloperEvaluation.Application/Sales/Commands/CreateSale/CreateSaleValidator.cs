using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;

/// <summary>
/// Validator for the CreateSaleCommand that ensures all required fields are provided
/// and that values meet business rules.
/// </summary>
public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleValidator"/> class.
    /// Configures validation rules for the CreateSaleCommand.
    /// </summary>
    public CreateSaleValidator()
    {
        // Validate Customer ID is provided
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required");

        // Validate Branch ID is provided
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required");

        // Validate that the sale contains at least one item
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Sale must contain at least one item");

        // Validate each item in the sale
        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                // Validate Product ID is provided for each item
                item.RuleFor(i => i.ProductId)
                    .NotEmpty().WithMessage("Product ID is required for all items");

                // Validate Quantity is valid for each item
                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0");
            });
    }
}