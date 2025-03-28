namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

/// <summary>
/// Value object representing monetary value with currency
/// </summary>
public record Money
{
    public decimal Amount { get; }
    public string Currency { get; } = "USD"; // Default currency

    public Money(decimal amount, string? currency = null)
    {
        if (amount < 0)
            throw new ArgumentException("Money amount cannot be negative");

        Amount = amount;
        Currency = currency ?? Currency;
    }

    // Operator overloads for money operations
    public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount, a.Currency);
    public static Money operator -(Money a, Money b) => new(a.Amount - b.Amount, a.Currency);
}