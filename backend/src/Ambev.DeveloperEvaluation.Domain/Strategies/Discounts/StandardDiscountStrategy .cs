namespace Ambev.DeveloperEvaluation.Domain.Strategies.Discounts
{
    /// <summary>
    /// Standard discount strategy that follows business rules:
    /// - Purchases above 4 identical items receive 10% discount
    /// - Purchases between 10 and 20 identical items receive 20% discount
    /// - No discounts for quantities below 4 items or above 20 items
    /// </summary>
    public class StandardDiscountStrategy : IDiscountStrategy
    {
        // <summary>
        /// Calculates discount based on business rules
        /// </summary>
        /// <param name="quantity">Quantity of items</param>
        /// <returns>Discount as a decimal percentage (0-1)</returns>
        public decimal CalculateDiscount(int quantity)
        {

            // Apply discount based on quantity tiers
            return quantity switch
            {
                >= 4 and < 10 => 0.1m,   // 10% discount for 4-9 items
                >= 10 and <= 20 => 0.2m, // 20% discount for 10-20 items                
                _ => 0m                  // No discount for 0-3 items
            };
        }
    }
}