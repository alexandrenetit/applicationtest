using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSale
{

    /// <summary>
    /// Validator for <see cref="GetSaleCommand"/>.
    /// </summary>
    public class GetSaleValidator : AbstractValidator<GetSaleCommand>
    {
        /// <summary>
        /// Initializes validation rules for <see cref="GetSaleCommand"/>.
        /// </summary>
        public GetSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required");
        }
    }
}