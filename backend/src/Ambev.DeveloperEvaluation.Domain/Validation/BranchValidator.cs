using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validators
{
    public class BranchValidator : AbstractValidator<Branch>
    {
        public BranchValidator()
        {
            RuleFor(branch => branch.Name)
                .NotEmpty().WithMessage("Branch name is required.")
                .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters.")
                .Matches(@"^[a-zA-Z0-9\s\-\.]+$").WithMessage("Branch name contains invalid characters.");

            RuleFor(branch => branch.Address)
                .NotNull().WithMessage("Branch location is required.")
                .SetValidator(new AddressValidator()); // Assuming you have an AddressValidator

            RuleFor(branch => branch.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[\d\s\-\(\)]{10,20}$").WithMessage("Invalid phone number format.");

            RuleFor(branch => branch.Status)
                .IsInEnum().WithMessage("Invalid branch status.");

            // Additional business rules for status changes
            RuleFor(branch => branch.Status)
                .Must((branch, status) =>
                    !(branch.Status == BranchStatus.ClosedPermanently && status != BranchStatus.ClosedPermanently))
                .WithMessage("Cannot reopen a permanently closed branch.")
                .When(branch => branch.Status != BranchStatus.ClosedPermanently);
        }
    }
}