using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(item => item.Product)
            .NotNull().WithMessage("Product is required.")
            .SetValidator(new ProductValidator()); // Reuse the Product validator if available

        RuleFor(item => item.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
            .LessThanOrEqualTo(1000).WithMessage("Quantity cannot exceed 1000.");

        RuleFor(item => item.UnitPrice)
            .NotNull().WithMessage("Unit price is required.")
            .Must(BeValidMoney).WithMessage("Unit price must be a valid monetary value.");

        RuleFor(item => item.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.")
            .LessThanOrEqualTo(1).WithMessage("Discount cannot exceed 100% (1.0).");

        // Additional validation for calculated TotalAmount if needed
        RuleFor(item => item.TotalAmount)
            .Must(BeValidMoney).WithMessage("Total amount must be a valid monetary value.")
            .Must((item, total) => total.Amount >= 0).WithMessage("Total amount cannot be negative.");
    }

    private bool BeValidMoney(Money money)
    {
        if (money == null)
            return false;

        // Add any specific Money validation logic here
        return money.Amount >= 0;
    }
}