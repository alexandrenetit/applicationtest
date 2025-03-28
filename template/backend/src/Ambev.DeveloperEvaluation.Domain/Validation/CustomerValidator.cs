using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.Email).SetValidator(new EmailValidator());

        RuleFor(customer => customer.Name)
            .NotEmpty()
            .WithMessage("The Name cannot be empty.");
    }
}