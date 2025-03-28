namespace Ambev.DeveloperEvaluation.Domain.Services
{
    /// <summary>
    /// Implementation of discount calculation rules
    /// </summary>
    public class DiscountCalculator : IDiscountCalculator
    {
        public decimal CalculateDiscount(int quantity)
        {
            if (quantity > 20)
                throw new DomainException("Cannot sell more than 20 identical items");

            decimal discount = quantity switch
            {
                >= 10 and <= 20 => 0.2m,
                >= 4 and < 10 => 0.1m,
                _ => 0m
            };

            return discount;
        }
    }
}