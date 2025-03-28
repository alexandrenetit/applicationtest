using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an individual line item in a sale
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Product being sold
    /// </summary>
    public Product Product { get; set; }

    /// <summary>
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Unit price at time of sale
    /// </summary>
    public Money UnitPrice { get; private set; }

    /// <summary>
    /// Discount applied to this item
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Calculated total amount for this item
    /// </summary>
    public Money TotalAmount => new(UnitPrice.Amount * Quantity * (1 - Discount));

    public SaleItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
        UnitPrice = product.UnitPrice;
        ApplyDiscountRules();
    }

    private void ApplyDiscountRules()
    {
        Discount = Quantity switch
        {
            >= 10 and <= 20 => 0.2m,
            >= 4 and < 10 => 0.1m,
            _ => 0m
        };
    }

    /// <summary>
    /// Performs validation of the sale item entity using the SaleItemValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    /// <remarks>
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">Discount</list>
    /// <list type="bullet">Product</list>
    /// <list type="bullet">Quantity</list>
    /// <list type="bullet">Total amount</list>
    /// <list type="bullet">Unit price</list>
    /// </remarks>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}