using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Ambev.DeveloperEvaluation.Domain.Specifications
{
    // <summary>
    /// Spec that check SaleItems amount and SaleItems quantity individually
    /// </summary>
    public class SaleItemLimitSpecification : ISpecification<Sale>
    {
        private readonly int _maximumItemsPerProduct = 20;

        public SaleItemLimitSpecification(IConfiguration configuration)
        {
            _maximumItemsPerProduct = int.TryParse(configuration["BusinessRules:Sales:MaximumItemPerProducts"], out var value) ? value : _maximumItemsPerProduct;
        }

        public int MaximumItemsPerProduct => _maximumItemsPerProduct;

        public bool IsSatisfiedBy(Sale sale)
        {
            return sale.Items
                .Any(i => i.Quantity > _maximumItemsPerProduct);
        }
    }
}