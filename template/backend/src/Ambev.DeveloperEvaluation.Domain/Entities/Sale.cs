using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validators;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
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
        public SaleStatus Status { get; private set; }

        /// <summary>
        /// Total amount of the sale
        /// </summary>
        public Money TotalAmount => new(_items.Sum(item => item.TotalAmount.Amount));        

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
}