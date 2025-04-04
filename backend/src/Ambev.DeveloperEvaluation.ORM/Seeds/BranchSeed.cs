using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Seeds;

public static class BranchSeed
{
    public static void GetSeedBranches(this ModelBuilder modelBuilder)
    {
        // Configure Branch entity and its owned type Address
        modelBuilder.Entity<Branch>()
            .OwnsOne(b => b.Address);

        // Seed Branch entities
        modelBuilder.Entity<Branch>().HasData(
            new
            {
                Id = Guid.Parse("7f89f537-8c49-4c90-b21f-ef1a9c5d4cb0"),
                Name = "Downtown Headquarters",
                PhoneNumber = "555-123-4567",
                Status = BranchStatus.Active
            },
            new
            {
                Id = Guid.Parse("93e5e068-d2de-4e24-8211-c16e3e6a9a9b"),
                Name = "Westside Mall Location",
                PhoneNumber = "555-234-5678",
                Status = BranchStatus.Active
            },
            new
            {
                Id = Guid.Parse("4c91b7eb-7b2d-4f1c-b936-e3852e0e3f53"),
                Name = "North District Branch",
                PhoneNumber = "555-345-6789",
                Status = BranchStatus.Active
            },
            new
            {
                Id = Guid.Parse("01c4a1b1-2d77-497a-b15d-3fcb71f66215"),
                Name = "Southside Center",
                PhoneNumber = "555-456-7890",
                Status = BranchStatus.Active
            },
            new
            {
                Id = Guid.Parse("bde82fa1-66b9-4ef4-a33b-151fb33f2507"),
                Name = "East End Location",
                PhoneNumber = "555-567-8901",
                Status = BranchStatus.Active
            },
            new
            {
                Id = Guid.Parse("49b5898e-2050-438a-b74c-ad4a97e2f9ed"),
                Name = "Airport Terminal Shop",
                PhoneNumber = "555-678-9012",
                Status = BranchStatus.UnderRenovation
            },
            new
            {
                Id = Guid.Parse("c9e06c79-60cc-457f-97c6-e18de43c0e9b"),
                Name = "University Campus Branch",
                PhoneNumber = "555-789-0123",
                Status = BranchStatus.Active
            },
            new
            {
                Id = Guid.Parse("3d4dda28-d3e6-4467-94f5-d1b3855f789e"),
                Name = "Business District Office",
                PhoneNumber = "555-890-1234",
                Status = BranchStatus.Active
            },
            new
            {
                Id = Guid.Parse("85728ac7-d654-4b5d-9def-fd8ed92d8538"),
                Name = "Industrial Zone Branch",
                PhoneNumber = "555-901-2345",
                Status = BranchStatus.TemporarilyClosed
            },
            new
            {
                Id = Guid.Parse("f6a7c5db-e766-4bf8-b1e3-dcbcd60d73d3"),
                Name = "Suburban Mall Kiosk",
                PhoneNumber = "555-012-3456",
                Status = BranchStatus.ClosedPermanently
            }
        );

        // Seed Address owned entities
        modelBuilder.Entity<Branch>()
            .OwnsOne(b => b.Address)
            .HasData(
                new
                {
                    BranchId = Guid.Parse("7f89f537-8c49-4c90-b21f-ef1a9c5d4cb0"),
                    Street = "123 Main Street",
                    City = "Metropolis",
                    State = "NY",
                    PostalCode = "10001",
                    Country = "USA"
                },
                new
                {
                    BranchId = Guid.Parse("93e5e068-d2de-4e24-8211-c16e3e6a9a9b"),
                    Street = "456 Western Avenue",
                    City = "Metropolis",
                    State = "NY",
                    PostalCode = "10002",
                    Country = "USA"
                },
                new
                {
                    BranchId = Guid.Parse("4c91b7eb-7b2d-4f1c-b936-e3852e0e3f53"),
                    Street = "789 Northern Boulevard",
                    City = "Northville",
                    State = "NY",
                    PostalCode = "10101",
                    Country = "USA"
                },
                new
                {
                    BranchId = Guid.Parse("01c4a1b1-2d77-497a-b15d-3fcb71f66215"),
                    Street = "321 Southern Road",
                    City = "Southtown",
                    State = "NY",
                    PostalCode = "10202",
                    Country = "USA"
                },
                new
                {
                    BranchId = Guid.Parse("bde82fa1-66b9-4ef4-a33b-151fb33f2507"),
                    Street = "654 Eastern Parkway",
                    City = "Eastville",
                    State = "NY",
                    PostalCode = "10303",
                    Country = "USA"
                },
                new
                {
                    BranchId = Guid.Parse("49b5898e-2050-438a-b74c-ad4a97e2f9ed"),
                    Street = "987 Airport Terminal C",
                    City = "Metropolis",
                    State = "NY",
                    PostalCode = "10005",
                    Country = "USA"
                },
                new
                {
                    BranchId = Guid.Parse("c9e06c79-60cc-457f-97c6-e18de43c0e9b"),
                    Street = "246 University Drive",
                    City = "College Town",
                    State = "NY",
                    PostalCode = "10404",
                    Country = "USA"
                },
                new
                {
                    BranchId = Guid.Parse("3d4dda28-d3e6-4467-94f5-d1b3855f789e"),
                    Street = "135 Business Center Road",
                    City = "Metropolis",
                    State = "NY",
                    PostalCode = "10006",
                    Country = "USA"
                },
                new
                {
                    BranchId = Guid.Parse("85728ac7-d654-4b5d-9def-fd8ed92d8538"),
                    Street = "579 Factory Lane",
                    City = "Industrial Park",
                    State = "NY",
                    PostalCode = "10505",
                    Country = "USA"
                },
                new
                {
                    BranchId = Guid.Parse("f6a7c5db-e766-4bf8-b1e3-dcbcd60d73d3"),
                    Street = "864 Suburban Mall, Unit 42",
                    City = "Suburbia",
                    State = "NY",
                    PostalCode = "10606",
                    Country = "USA"
                }
            );
    }
}