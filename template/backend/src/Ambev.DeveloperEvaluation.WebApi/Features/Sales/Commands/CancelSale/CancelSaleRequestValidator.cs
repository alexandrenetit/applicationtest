using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Commands.CancelSale
{
    /// <summary>
    /// Validator for <see cref="CancelSaleRequest"/>.
    /// </summary>
    public class CancelSaleRequestValidator : AbstractValidator<CancelSaleRequest>
    {
        /// <summary>
        /// Initializes validation rules for <see cref="CancelSaleRequest"/>.
        /// </summary>
        public CancelSaleRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required");
        }
    }
}