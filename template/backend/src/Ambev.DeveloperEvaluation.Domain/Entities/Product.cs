using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a product that can be sold
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description of the product
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Standard price of the product
        /// </summary>
        public Money UnitPrice { get; private set; }

        public Product(string name, string description, Money unitPrice)
        {
            Name = name;
            Description = description;
            UnitPrice = unitPrice;
        }

        /// <summary>
        /// Performs validation of the product entity using the ProductValidator rules.
        /// </summary>
        /// <returns>
        /// A <see cref="ValidationResultDetail"/> containing:
        /// - IsValid: Indicates whether all validation rules passed
        /// - Errors: Collection of validation errors if any rules failed
        /// </returns>
        /// <remarks>
        /// <listheader>The validation includes checking:</listheader>
        /// <list type="bullet">Description</list>
        /// <list type="bullet">Name</list>
        /// <list type="bullet">Unit price</list>
        /// </remarks>
        public ValidationResultDetail Validate()
        {
            var validator = new ProductValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}