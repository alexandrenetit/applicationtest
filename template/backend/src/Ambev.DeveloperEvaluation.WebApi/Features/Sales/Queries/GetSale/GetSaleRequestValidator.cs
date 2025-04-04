using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Queries.GetSale;

/// <summary>
/// Validator for <see cref="GetSaleRequest"/>.
/// </summary>
public class GetSaleRequestValidator : AbstractValidator<GetSaleRequest>
{
    /// <summary>
    /// Initializes validation rules for <see cref="GetSaleRequest"/>.
    /// </summary>
    public GetSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}
