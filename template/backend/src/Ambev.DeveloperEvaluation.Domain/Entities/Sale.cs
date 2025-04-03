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
    private Money? _totalAmount;

    /// <summary>
    /// Total amount of the sale
    /// </summary>
    public Money TotalAmount
    {
        get
        {
            // If there's a stored value, return it
            if (_totalAmount != null)
                return _totalAmount;

            // Otherwise calculate from items
            if (_items.Count == 0)
                return new Money(0, "USD"); // Default currency when no items

            var currency = _items.First().TotalAmount.Currency;
            if (_items.Any(item => item.TotalAmount.Currency != currency))
                throw new InvalidOperationException("Cannot sum amounts with different currencies");

            // Calculate the sum and round to two decimal places
            decimal totalAmount = Math.Round(_items.Sum(item => item.TotalAmount.Amount), 2, MidpointRounding.AwayFromZero);

            return new Money(totalAmount, currency);
        }
        set => _totalAmount = value;
    }

    /// <summary>
    /// Foreign key for the customer who made the purchase
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Foreign key for the branch where the sale was made
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets the date and time when the category was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Gets the date and time of the last update to the category's information.
    /// </summary>
    public DateTime? UpdatedAt { get;  set; }

    /// <summary>
    /// Gets the date and time when was cancelled the cart.
    /// </summary>
    public DateTime? CancelledAt { get;  set; }

    /// <summary>
    /// Adds a product to the sale with specified quantity
    /// </summary>
    public void AddItem(Product product, int quantity)
    {
        var item = new SaleItem(product, quantity);
        _items.Add(item);

        // Recalculate total amount after adding an item
        if (_totalAmount == null)
            _totalAmount = item.TotalAmount;
        else
            _totalAmount = new Money(_totalAmount.Amount + item.TotalAmount.Amount, _totalAmount.Currency);
    }

    /// <summary>
    /// Cancels the sale
    /// </summary>
    public void Cancel()
    {
        Status = SaleStatus.Cancelled;
    }

    public void ClearItems()
    {
        // Store the original currency if we have items
        var originalCurrency = _items.Count > 0
            ? _items[0].TotalAmount.Currency
            : "USD"; // default currency

        _items.Clear();
        _totalAmount = new Money(0, originalCurrency);
    }

    public void RemoveItem(SaleItem item)
    {
        if (item == null) return;

        if (_items.Remove(item))
        {
            // Recalculate total if we were tracking it
            if (_totalAmount != null)
            {
                _totalAmount = new Money(
                    _totalAmount.Amount - item.TotalAmount.Amount,
                    _totalAmount.Currency);
            }
        }
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