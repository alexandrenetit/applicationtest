using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validators
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(sale => sale.SaleNumber)
                .NotEmpty().WithMessage("Sale number is required.")
                .Length(5, 20).WithMessage("Sale number must be between 5 and 20 characters.")
                .Matches(@"^[A-Za-z0-9-]+$").WithMessage("Sale number can only contain letters, numbers, and hyphens.");

            RuleFor(sale => sale.SaleDate)
                .NotEmpty().WithMessage("Sale date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.")
                .GreaterThanOrEqualTo(new DateTime(2000, 1, 1)).WithMessage("Sale date cannot be before year 2000.");

            RuleFor(sale => sale.Customer)
                .NotNull().WithMessage("Customer is required.")
                .SetValidator(new CustomerValidator()); 

            RuleFor(sale => sale.Branch)
                .NotNull().WithMessage("Branch is required.")
                .SetValidator(new BranchValidator()); 

            RuleFor(sale => sale.Items)
                .NotEmpty().WithMessage("Sale must contain at least one item.")
                .Must(items => items.Count <= 100).WithMessage("Sale cannot contain more than 100 items.");

            RuleForEach(sale => sale.Items)
                .SetValidator(new SaleItemValidator()); 

            RuleFor(sale => sale.TotalAmount)
                .NotNull().WithMessage("Total amount is required.")
                .Must(BeValidMoney).WithMessage("Total amount must be a valid monetary value.")
                .Must((sale, total) => total.Amount > 0).WithMessage("Total amount must be greater than 0.");

            RuleFor(sale => sale.Status)
                .IsInEnum().WithMessage("Invalid sale status.");
        }

        private bool BeValidMoney(Money money)
        {
            if (money == null)
                return false;
            return money.Amount >= 0;
        }        
    }
}