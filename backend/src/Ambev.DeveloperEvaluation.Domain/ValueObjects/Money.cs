namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

/// <summary>
/// Value object representing monetary value with currency
/// </summary>
public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }
    public string Symbol { get; }

    private static readonly Dictionary<string, string> CurrencySymbols = new()
    {
        ["USD"] = "$",
        ["EUR"] = "€",
        ["BRL"] = "R$",
        ["GBP"] = "£",
        ["JPY"] = "¥"
    };

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
        Symbol = CurrencySymbols[currency];
    }

    public string Formatted => $"{Symbol}{Amount:0.00}";
}