using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validators
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(address => address.Street)
                .NotEmpty().WithMessage("Street address is required.")
                .MaximumLength(100).WithMessage("Street address cannot exceed 100 characters.")
                .Matches(@"^[a-zA-Z0-9\s\.,#\-]+$").WithMessage("Street address contains invalid characters.");

            RuleFor(address => address.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(50).WithMessage("City name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s\-]+$").WithMessage("City name contains invalid characters.");

            RuleFor(address => address.State)
                .NotEmpty().WithMessage("State is required.")
                .MaximumLength(50).WithMessage("State name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s\-]+$").WithMessage("State name contains invalid characters.");

            RuleFor(address => address.PostalCode)
                .NotEmpty().WithMessage("Postal code is required.")
                .MaximumLength(20).WithMessage("Postal code cannot exceed 20 characters.")
                .Matches(@"^[a-zA-Z0-9\s\-]+$").WithMessage("Postal code contains invalid characters.");

            RuleFor(address => address.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(50).WithMessage("Country name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s\-]+$").WithMessage("Country name contains invalid characters.");
        }
    }
}