using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.ORM.Seeds;

public static class ProductSeed
{
    public static List<Product> GetSeedProducts() => new()
{
    CreateBeer("Budweiser", "American Lager", 2.99m, "USD"),
    CreateBeer("Stella Artois", "Belgian Pilsner", 3.49m, "EUR"),
    CreateBeer("Brahma", "Brazilian Lager", 7.90m, "BRL"),
    CreateBeer("Corona Extra", "Mexican Lager", 3.29m, "USD"),
    CreateBeer("Hoegaarden", "Belgian Witbier", 4.25m, "EUR"),
    CreateBeer("Michelob Ultra", "Light Lager", 2.79m, "USD"),
    CreateBeer("Skol", "Brazilian Lager", 6.50m, "BRL"),
    CreateBeer("Leffe Blonde", "Abbey Beer", 5.50m, "EUR"),
    CreateBeer("Quilmes", "Argentinian Lager", 2.89m, "USD"),
    CreateBeer("Bud Light", "Light Lager", 2.49m, "USD")
};

    private static Product CreateBeer(string name, string description, decimal price, string currency)
        => new(name, description, new Money(price, currency));
}