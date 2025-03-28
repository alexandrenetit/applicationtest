namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Service for calculating discounts based on business rules
/// </summary>
public interface IDiscountCalculator
{
    /// <summary>
    /// Calculates the appropriate discount for a given quantity
    /// </summary>
    decimal CalculateDiscount(int quantity);
}