using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(product => product.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

        RuleFor(product => product.Description)
            .MaximumLength(500).WithMessage("Product description cannot exceed 500 characters.");

        RuleFor(product => product.UnitPrice)
            .NotNull().WithMessage("Unit price is required.")
            .Must(BeValidMoney).WithMessage("Unit price must be a valid monetary value.");
    }

    private bool BeValidMoney(Money money)
    {
        if (money == null)
            return false;
        
        return money.Amount >= 0;
    }
}