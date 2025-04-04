namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

// This allows EF Core to create instances when needed
public record Address
{
    // Parameterless constructor for EF Core
    private Address() { }

    public Address(string street, string city, string state, string postalCode, string country)
    {
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
    }

    public string Street { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string PostalCode { get; init; }
    public string Country { get; init; }

    public string FullAddress => $"{Street}, {City}, {State} {PostalCode}, {Country}";
}