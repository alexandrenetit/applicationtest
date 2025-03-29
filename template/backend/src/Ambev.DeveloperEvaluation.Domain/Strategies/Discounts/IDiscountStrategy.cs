namespace Ambev.DeveloperEvaluation.Domain.Strategies.Discounts
{
    /// <summary>
    /// Interface for discount calculation strategies
    /// </summary>
    public interface IDiscountStrategy
    {
        /// <summary>
        /// Calculates the discount based on provided parameters
        /// </summary>
        /// <param name="quantity">Quantity of items</param>
        /// <returns>Discount as a decimal percentage (0-1)</returns>
        decimal CalculateDiscount(int quantity);
    }
}