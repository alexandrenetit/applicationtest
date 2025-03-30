using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validators;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sales transaction
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Unique sale number/identifier
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the sale was made
    /// </summary>
    public DateTime SaleDate { get; set; } 

    /// <summary>
    /// Customer who made the purchase
    /// </summary>
    public Customer Customer { get;  set; }

    /// <summary>
    /// Branch where the sale was made
    /// </summary>
    public Branch Branch { get; set; }

    /// <summary>
    /// Collection of items in the sale
    /// </summary>
    private readonly List<SaleItem> _items = new();

    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Status of the sale (Cancelled/Not Cancelled)
    /// </summary>
    public SaleStatus Status { get;  set; }

    /// <summary>
    /// Total amount of the sale
    /// </summary>
    public Money TotalAmount
    {
        get
        {
            if (_items.Count == 0)
                return new Money(0, "USD"); // Default currency when no items

            // Get currency from first item (assuming all items have same currency)
            var currency = _items.First().TotalAmount.Currency;

            // Verify all items have the same currency
            if (_items.Any(item => item.TotalAmount.Currency != currency))
                throw new InvalidOperationException("Cannot sum amounts with different currencies");

            return new Money(_items.Sum(item => item.TotalAmount.Amount), currency);
        }
        set => TotalAmount = value;
    }

    /// <summary>
    /// Adds a product to the sale with specified quantity
    /// </summary>
    public void AddItem(Product product, int quantity)
    {
        _items.Add(new SaleItem(product, quantity));
    }

    /// <summary>
    /// Cancels the sale
    /// </summary>
    public void Cancel()
    {
        Status = SaleStatus.Cancelled;
    }

    /// <summary>
    /// Performs validation of the sale entity using the SaleValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    /// <remarks>
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">Branch</list>
    /// <list type="bullet">Customer</list>
    /// <list type="bullet">Items</list>
    /// <list type="bullet">Sale date</list>
    /// <list type="bullet">Sale number</list>
    /// <list type="bullet">Status</list>
    /// <list type="bullet">Total amount</list>
    /// </remarks>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}