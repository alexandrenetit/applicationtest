using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.ORM.Seeds;

public static class BranchSeed
{
    public static List<Branch> GetSeedBranches()
    {
        return new List<Branch>
        {
            new Branch
            {
                Id = new Guid("a8b3c2d1-1234-5678-9012-abcdef123456"),
                Name = "Manhattan Downtown",
                Address = new Address("200 Broadway", "New York", "NY", "10038", "USA"),
                PhoneNumber = "(212) 555-0101",
                Status = BranchStatus.Active
            },
            new Branch
            {
                Id = new Guid("b9c4d3e2-2345-6789-0123-bcdef1234567"),
                Name = "Brooklyn Heights",
                Address = new Address("55 Water St", "Brooklyn", "NY", "11201", "USA"),
                PhoneNumber = "(718) 555-0202",
                Status = BranchStatus.Active
            },
            new Branch
            {
                Id = new Guid("c5d6e7f8-3456-7890-1234-cdef12345678"),
                Name = "Chicago Loop",
                Address = new Address("233 S Wacker Dr", "Chicago", "IL", "60606", "USA"),
                PhoneNumber = "(312) 555-0303",
                Status = BranchStatus.UnderRenovation
            },
            new Branch
            {
                Id = new Guid("d7e8f9a0-4567-8901-2345-def123456789"),
                Name = "San Francisco Downtown",
                Address = new Address("1 Market St", "San Francisco", "CA", "94105", "USA"),
                PhoneNumber = "(415) 555-0404",
                Status = BranchStatus.TemporarilyClosed
            },
            new Branch
            {
                Id = new Guid("e9f0a1b2-5678-9012-3456-ef1234567890"),
                Name = "Seattle Center",
                Address = new Address("400 Broad St", "Seattle", "WA", "98109", "USA"),
                PhoneNumber = "(206) 555-0505",
                Status = BranchStatus.Active
            },
            new Branch
            {
                Id = new Guid("f1a2b3c4-6789-0123-4567-f12345678901"),
                Name = "Boston Financial District",
                Address = new Address("100 Federal St", "Boston", "MA", "02110", "USA"),
                PhoneNumber = "(617) 555-0606",
                Status = BranchStatus.Active
            },
            new Branch
            {
                Id = new Guid("a2b3c4d5-7890-1234-5678-123456789012"),
                Name = "Austin Downtown",
                Address = new Address("303 Colorado St", "Austin", "TX", "78701", "USA"),
                PhoneNumber = "(512) 555-0707",
                Status = BranchStatus.Active
            },
            new Branch
            {
                Id = new Guid("b3c4d5e6-8901-2345-6789-234567890123"),
                Name = "Miami Beach",
                Address = new Address("1200 Ocean Dr", "Miami Beach", "FL", "33139", "USA"),
                PhoneNumber = "(305) 555-0808",
                Status = BranchStatus.Active
            },
            new Branch
            {
                Id = new Guid("c4d5e6f7-9012-3456-7890-345678901234"),
                Name = "Denver Tech Center",
                Address = new Address("8000 E Belleview Ave", "Greenwood Village", "CO", "80111", "USA"),
                PhoneNumber = "(303) 555-0909",
                Status = BranchStatus.ClosedPermanently
            },
            new Branch
            {
                Id = new Guid("d5e6f7a8-0123-4567-8901-456789012345"),
                Name = "Los Angeles Hollywood",
                Address = new Address("6801 Hollywood Blvd", "Los Angeles", "CA", "90028", "USA"),
                PhoneNumber = "(213) 555-1010",
                Status = BranchStatus.Active
            }
        };
    }
}