using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale;

/// <summary>
/// Validator for <see cref="CancelSaleCommand"/>.
/// </summary>
public class CancelSaleValidator : AbstractValidator<CancelSaleCommand>
{
    /// <summary>
    /// Initializes validation rules for <see cref="CancelSaleCommand"/>.
    /// </summary>
    public CancelSaleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required.");
    }
}