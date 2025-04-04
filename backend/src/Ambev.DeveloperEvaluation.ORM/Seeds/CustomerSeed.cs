using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.ORM.Seeds;

public static class CustomerSeed
{
    public static List<Customer> GetSeedCustomers()
    {
        return new List<Customer>
    {
        new Customer
        {
            Id = new Guid("c1a2b3d4-1234-5678-9012-345678912345"),
            Name = "Michael Johnson",
            Email = "michael.johnson@example.com"
        },
        new Customer
        {
            Id = new Guid("d2e3f4a5-2345-6789-0123-456789123456"),
            Name = "Sarah Williams",
            Email = "sarah.williams@example.com"
        },
        new Customer
        {
            Id = new Guid("e3f4a5b6-3456-7890-1234-567891234567"),
            Name = "Robert Brown",
            Email = "robert.brown@example.com"
        },
        new Customer
        {
            Id = new Guid("f4a5b6c7-4567-8901-2345-678912345678"),
            Name = "Jennifer Davis",
            Email = "jennifer.davis@example.com"
        },
        new Customer
        {
            Id = new Guid("a5b6c7d8-5678-9012-3456-789123456789"),
            Name = "David Miller",
            Email = "david.miller@example.com"
        },
        new Customer
        {
            Id = new Guid("b6c7d8e9-6789-0123-4567-891234567890"),
            Name = "Lisa Wilson",
            Email = "lisa.wilson@example.com"
        },
        new Customer
        {
            Id = new Guid("c7d8e9f0-7890-1234-5678-912345678901"),
            Name = "Thomas Moore",
            Email = "thomas.moore@example.com"
        },
        new Customer
        {
            Id = new Guid("d8e9f0a1-8901-2345-6789-123456789012"),
            Name = "Nancy Taylor",
            Email = "nancy.taylor@example.com"
        },
        new Customer
        {
            Id = new Guid("e9f0a1b2-9012-3456-7890-234567890123"),
            Name = "James Anderson",
            Email = "james.anderson@example.com"
        },
        new Customer
        {
            Id = new Guid("f0a1b2c3-0123-4567-8901-345678901234"),
            Name = "Margaret Thomas",
            Email = "margaret.thomas@example.com"
        }
    };
    }
}