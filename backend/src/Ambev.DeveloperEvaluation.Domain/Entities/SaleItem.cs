using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Strategies.Discounts;
using Ambev.DeveloperEvaluation.Domain.Strategies.Discounts.Enums;
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
    private Money? _totalAmount;

    /// <summary>
    /// Calculated total amount for this item
    /// </summary>
    public Money TotalAmount
    {
        get
        {
            if (_totalAmount != null)
                return _totalAmount;

            // Calculate the raw amount
            decimal rawAmount = UnitPrice.Amount * Quantity * (1 - Discount);

            // Round to exactly two decimal places
            decimal roundedAmount = Math.Round(rawAmount, 2, MidpointRounding.AwayFromZero);

            return new Money(
                amount: roundedAmount,
                currency: UnitPrice.Currency);
        }
        private set => _totalAmount = value;
    }

    /// <summary>
    /// The discount strategy used for this sale item
    /// </summary>
    private readonly IDiscountStrategy _discountStrategy;

    /// <summary>
    /// Foreign key for the sale this item belongs to
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Foreign key for the product being sold
    /// </summary>
    public Guid ProductId { get; set; }

    // Add a parameterless constructor for EF Core
    protected SaleItem() { }

    /// <summary>
    /// Creates a new sale item with the specified discount strategy type
    /// </summary>
    /// <param name="product">Product being sold</param>
    /// <param name="quantity">Quantity of the product</param>
    /// <param name="strategyType">Type of discount strategy to apply (defaults to Standard)</param>
    public SaleItem(Product product, int quantity, DiscountStrategyType strategyType = DiscountStrategyType.Standard)
        : this(product, quantity, DiscountStrategyFactory.CreateStrategy(strategyType))
    {
    }

    /// <summary>
    /// Creates a new sale item with a custom discount strategy instance
    /// </summary>
    /// <param name="product">Product being sold</param>
    /// <param name="quantity">Quantity of the product</param>
    /// <param name="discountStrategy">Custom discount strategy to apply</param>
    public SaleItem(Product product, int quantity, IDiscountStrategy discountStrategy)
    {
        Product = product;
        Quantity = quantity;
        UnitPrice = product.UnitPrice;
        _discountStrategy = discountStrategy;
        ApplyDiscountRules();

        // Initialize TotalAmount with the calculated value
        _totalAmount = CalculateTotalAmount();
    }

    /// <summary>
    /// Updates the quantity of the item and recalculates the total amount
    /// </summary>
    /// <returns></returns>
    private Money CalculateTotalAmount()
    {
        // Calculate the raw amount
        decimal rawAmount = UnitPrice.Amount * Quantity * (1 - Discount);

        // Round to exactly two decimal places
        decimal roundedAmount = Math.Round(rawAmount, 2, MidpointRounding.AwayFromZero);

        return new Money(
            amount: roundedAmount,
            currency: UnitPrice.Currency);
    }

    /// <summary>
    /// Applies the discount rules based on the current discount strategy
    /// </summary>
    private void ApplyDiscountRules()
    {
        Discount = _discountStrategy.CalculateDiscount(Quantity);
        // Update TotalAmount after discount changes
        _totalAmount = CalculateTotalAmount();
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