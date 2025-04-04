using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Seeds;

public static class ProductSeed
{
    public static void SeedProducts(this ModelBuilder modelBuilder)
    {
        // Configuração do Money como um owned type
        modelBuilder.Entity<Product>()
            .OwnsOne(p => p.UnitPrice);

        // Seed de entidades Product (sem incluir o UnitPrice)
        modelBuilder.Entity<Product>().HasData(
            new
            {
                Id = Guid.Parse("f1a6b5e1-c9d0-4a41-8d29-1399e81b3c8d"),
                Name = "Budweiser",
                Description = "American Lager"
            },
            new
            {
                Id = Guid.Parse("2b9a9e2c-4b1d-476b-a6c2-3834a9023bdd"),
                Name = "Stella Artois",
                Description = "Belgian Pilsner"
            },
            new
            {
                Id = Guid.Parse("8d0c1a84-49cf-4f28-86a0-57a82c00db0d"),
                Name = "Brahma",
                Description = "Brazilian Lager"
            },
            new
            {
                Id = Guid.Parse("c1d8d459-2149-4147-8e2c-e1d102492481"),
                Name = "Corona Extra",
                Description = "Mexican Lager"
            },
            new
            {
                Id = Guid.Parse("39f45a65-0b1e-43a7-8e44-808a7826ac2b"),
                Name = "Hoegaarden",
                Description = "Belgian Witbier"
            },
            new
            {
                Id = Guid.Parse("8f09ce39-b03c-4e45-add5-29cc254e4da2"),
                Name = "Michelob Ultra",
                Description = "Light Lager"
            },
            new
            {
                Id = Guid.Parse("64e69709-6dd0-44c3-8651-9e8a233454cb"),
                Name = "Skol",
                Description = "Brazilian Lager"
            },
            new
            {
                Id = Guid.Parse("e3b46d88-6319-4e64-94a8-90cf612984d1"),
                Name = "Leffe Blonde",
                Description = "Abbey Beer"
            },
            new
            {
                Id = Guid.Parse("9a5bfdcd-c03e-4d72-b9d4-bcad47727049"),
                Name = "Quilmes",
                Description = "Argentinian Lager"
            },
            new
            {
                Id = Guid.Parse("d7c14adf-fc3a-45de-be5f-aa7415483a45"),
                Name = "Bud Light",
                Description = "Light Lager"
            }
        );

        // Seed dos objetos Money (UnitPrice) para cada Product
        modelBuilder.Entity<Product>()
            .OwnsOne(p => p.UnitPrice).HasData(
                new
                {
                    ProductId = Guid.Parse("f1a6b5e1-c9d0-4a41-8d29-1399e81b3c8d"),
                    Amount = 2.99m,
                    Currency = "USD"
                },
                new
                {
                    ProductId = Guid.Parse("2b9a9e2c-4b1d-476b-a6c2-3834a9023bdd"),
                    Amount = 3.49m,
                    Currency = "EUR"
                },
                new
                {
                    ProductId = Guid.Parse("8d0c1a84-49cf-4f28-86a0-57a82c00db0d"),
                    Amount = 7.90m,
                    Currency = "BRL"
                },
                new
                {
                    ProductId = Guid.Parse("c1d8d459-2149-4147-8e2c-e1d102492481"),
                    Amount = 3.29m,
                    Currency = "USD"
                },
                new
                {
                    ProductId = Guid.Parse("39f45a65-0b1e-43a7-8e44-808a7826ac2b"),
                    Amount = 4.25m,
                    Currency = "EUR"
                },
                new
                {
                    ProductId = Guid.Parse("8f09ce39-b03c-4e45-add5-29cc254e4da2"),
                    Amount = 2.79m,
                    Currency = "USD"
                },
                new
                {
                    ProductId = Guid.Parse("64e69709-6dd0-44c3-8651-9e8a233454cb"),
                    Amount = 6.50m,
                    Currency = "BRL"
                },
                new
                {
                    ProductId = Guid.Parse("e3b46d88-6319-4e64-94a8-90cf612984d1"),
                    Amount = 5.50m,
                    Currency = "EUR"
                },
                new
                {
                    ProductId = Guid.Parse("9a5bfdcd-c03e-4d72-b9d4-bcad47727049"),
                    Amount = 2.89m,
                    Currency = "USD"
                },
                new
                {
                    ProductId = Guid.Parse("d7c14adf-fc3a-45de-be5f-aa7415483a45"),
                    Amount = 2.49m,
                    Currency = "USD"
                }
            );
    }
}