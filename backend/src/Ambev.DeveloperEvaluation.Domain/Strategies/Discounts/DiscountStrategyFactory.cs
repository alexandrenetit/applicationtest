using Ambev.DeveloperEvaluation.Domain.Strategies.Discounts.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Strategies.Discounts
{
    /// <summary>
    /// Factory for creating discount strategies
    /// </summary>
    public static class DiscountStrategyFactory
    {
        /// <summary>
        /// Creates a discount strategy based on the specified type
        /// </summary>
        /// <param name="strategyType">Type of discount strategy to create</param>
        /// <returns>A discount strategy matching the requested type</returns>
        /// <exception cref="ArgumentException">Thrown when an unknown strategy type is provided</exception>
        public static IDiscountStrategy CreateStrategy(DiscountStrategyType strategyType = DiscountStrategyType.Standard)
        {
            return strategyType switch
            {
                DiscountStrategyType.Standard => new StandardDiscountStrategy(),

                // Add cases for future strategy types here                

                _ => throw new DomainException($"Unsupported discount strategy type: {strategyType}", nameof(strategyType))
            };
        }
    }
}