﻿// <auto-generated />
using System;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    [DbContext(typeof(DefaultContext))]
    [Migration("20250330142419_202503301116_AddEntitiesMigration")]
    partial class _202503301116_AddEntitiesMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Branch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Branches", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("7f89f537-8c49-4c90-b21f-ef1a9c5d4cb0"),
                            Name = "Downtown Headquarters",
                            PhoneNumber = "555-123-4567",
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("93e5e068-d2de-4e24-8211-c16e3e6a9a9b"),
                            Name = "Westside Mall Location",
                            PhoneNumber = "555-234-5678",
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("4c91b7eb-7b2d-4f1c-b936-e3852e0e3f53"),
                            Name = "North District Branch",
                            PhoneNumber = "555-345-6789",
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("01c4a1b1-2d77-497a-b15d-3fcb71f66215"),
                            Name = "Southside Center",
                            PhoneNumber = "555-456-7890",
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("bde82fa1-66b9-4ef4-a33b-151fb33f2507"),
                            Name = "East End Location",
                            PhoneNumber = "555-567-8901",
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("49b5898e-2050-438a-b74c-ad4a97e2f9ed"),
                            Name = "Airport Terminal Shop",
                            PhoneNumber = "555-678-9012",
                            Status = "UnderRenovation"
                        },
                        new
                        {
                            Id = new Guid("c9e06c79-60cc-457f-97c6-e18de43c0e9b"),
                            Name = "University Campus Branch",
                            PhoneNumber = "555-789-0123",
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("3d4dda28-d3e6-4467-94f5-d1b3855f789e"),
                            Name = "Business District Office",
                            PhoneNumber = "555-890-1234",
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("85728ac7-d654-4b5d-9def-fd8ed92d8538"),
                            Name = "Industrial Zone Branch",
                            PhoneNumber = "555-901-2345",
                            Status = "TemporarilyClosed"
                        },
                        new
                        {
                            Id = new Guid("f6a7c5db-e766-4bf8-b1e3-dcbcd60d73d3"),
                            Name = "Suburban Mall Kiosk",
                            PhoneNumber = "555-012-3456",
                            Status = "ClosedPermanently"
                        });
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Customers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("c1a2b3d4-1234-5678-9012-345678912345"),
                            Email = "michael.johnson@example.com",
                            Name = "Michael Johnson"
                        },
                        new
                        {
                            Id = new Guid("d2e3f4a5-2345-6789-0123-456789123456"),
                            Email = "sarah.williams@example.com",
                            Name = "Sarah Williams"
                        },
                        new
                        {
                            Id = new Guid("e3f4a5b6-3456-7890-1234-567891234567"),
                            Email = "robert.brown@example.com",
                            Name = "Robert Brown"
                        },
                        new
                        {
                            Id = new Guid("f4a5b6c7-4567-8901-2345-678912345678"),
                            Email = "jennifer.davis@example.com",
                            Name = "Jennifer Davis"
                        },
                        new
                        {
                            Id = new Guid("a5b6c7d8-5678-9012-3456-789123456789"),
                            Email = "david.miller@example.com",
                            Name = "David Miller"
                        },
                        new
                        {
                            Id = new Guid("b6c7d8e9-6789-0123-4567-891234567890"),
                            Email = "lisa.wilson@example.com",
                            Name = "Lisa Wilson"
                        },
                        new
                        {
                            Id = new Guid("c7d8e9f0-7890-1234-5678-912345678901"),
                            Email = "thomas.moore@example.com",
                            Name = "Thomas Moore"
                        },
                        new
                        {
                            Id = new Guid("d8e9f0a1-8901-2345-6789-123456789012"),
                            Email = "nancy.taylor@example.com",
                            Name = "Nancy Taylor"
                        },
                        new
                        {
                            Id = new Guid("e9f0a1b2-9012-3456-7890-234567890123"),
                            Email = "james.anderson@example.com",
                            Name = "James Anderson"
                        },
                        new
                        {
                            Id = new Guid("f0a1b2c3-0123-4567-8901-345678901234"),
                            Email = "margaret.thomas@example.com",
                            Name = "Margaret Thomas"
                        });
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Products", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("f1a6b5e1-c9d0-4a41-8d29-1399e81b3c8d"),
                            Description = "American Lager",
                            Name = "Budweiser"
                        },
                        new
                        {
                            Id = new Guid("2b9a9e2c-4b1d-476b-a6c2-3834a9023bdd"),
                            Description = "Belgian Pilsner",
                            Name = "Stella Artois"
                        },
                        new
                        {
                            Id = new Guid("8d0c1a84-49cf-4f28-86a0-57a82c00db0d"),
                            Description = "Brazilian Lager",
                            Name = "Brahma"
                        },
                        new
                        {
                            Id = new Guid("c1d8d459-2149-4147-8e2c-e1d102492481"),
                            Description = "Mexican Lager",
                            Name = "Corona Extra"
                        },
                        new
                        {
                            Id = new Guid("39f45a65-0b1e-43a7-8e44-808a7826ac2b"),
                            Description = "Belgian Witbier",
                            Name = "Hoegaarden"
                        },
                        new
                        {
                            Id = new Guid("8f09ce39-b03c-4e45-add5-29cc254e4da2"),
                            Description = "Light Lager",
                            Name = "Michelob Ultra"
                        },
                        new
                        {
                            Id = new Guid("64e69709-6dd0-44c3-8651-9e8a233454cb"),
                            Description = "Brazilian Lager",
                            Name = "Skol"
                        },
                        new
                        {
                            Id = new Guid("e3b46d88-6319-4e64-94a8-90cf612984d1"),
                            Description = "Abbey Beer",
                            Name = "Leffe Blonde"
                        },
                        new
                        {
                            Id = new Guid("9a5bfdcd-c03e-4d72-b9d4-bcad47727049"),
                            Description = "Argentinian Lager",
                            Name = "Quilmes"
                        },
                        new
                        {
                            Id = new Guid("d7c14adf-fc3a-45de-be5f-aa7415483a45"),
                            Description = "Light Lager",
                            Name = "Bud Light"
                        });
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Sale", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SaleNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.ToTable("Sales", (string)null);
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.SaleItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(5,4)");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("SaleItems", (string)null);
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Branch", b =>
                {
                    b.OwnsOne("Ambev.DeveloperEvaluation.Domain.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("BranchId")
                                .HasColumnType("uuid");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("City");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("Country");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)")
                                .HasColumnName("PostalCode");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("State");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("Street");

                            b1.HasKey("BranchId");

                            b1.ToTable("Branches");

                            b1.WithOwner()
                                .HasForeignKey("BranchId");

                            b1.HasData(
                                new
                                {
                                    BranchId = new Guid("7f89f537-8c49-4c90-b21f-ef1a9c5d4cb0"),
                                    City = "Metropolis",
                                    Country = "USA",
                                    PostalCode = "10001",
                                    State = "NY",
                                    Street = "123 Main Street"
                                },
                                new
                                {
                                    BranchId = new Guid("93e5e068-d2de-4e24-8211-c16e3e6a9a9b"),
                                    City = "Metropolis",
                                    Country = "USA",
                                    PostalCode = "10002",
                                    State = "NY",
                                    Street = "456 Western Avenue"
                                },
                                new
                                {
                                    BranchId = new Guid("4c91b7eb-7b2d-4f1c-b936-e3852e0e3f53"),
                                    City = "Northville",
                                    Country = "USA",
                                    PostalCode = "10101",
                                    State = "NY",
                                    Street = "789 Northern Boulevard"
                                },
                                new
                                {
                                    BranchId = new Guid("01c4a1b1-2d77-497a-b15d-3fcb71f66215"),
                                    City = "Southtown",
                                    Country = "USA",
                                    PostalCode = "10202",
                                    State = "NY",
                                    Street = "321 Southern Road"
                                },
                                new
                                {
                                    BranchId = new Guid("bde82fa1-66b9-4ef4-a33b-151fb33f2507"),
                                    City = "Eastville",
                                    Country = "USA",
                                    PostalCode = "10303",
                                    State = "NY",
                                    Street = "654 Eastern Parkway"
                                },
                                new
                                {
                                    BranchId = new Guid("49b5898e-2050-438a-b74c-ad4a97e2f9ed"),
                                    City = "Metropolis",
                                    Country = "USA",
                                    PostalCode = "10005",
                                    State = "NY",
                                    Street = "987 Airport Terminal C"
                                },
                                new
                                {
                                    BranchId = new Guid("c9e06c79-60cc-457f-97c6-e18de43c0e9b"),
                                    City = "College Town",
                                    Country = "USA",
                                    PostalCode = "10404",
                                    State = "NY",
                                    Street = "246 University Drive"
                                },
                                new
                                {
                                    BranchId = new Guid("3d4dda28-d3e6-4467-94f5-d1b3855f789e"),
                                    City = "Metropolis",
                                    Country = "USA",
                                    PostalCode = "10006",
                                    State = "NY",
                                    Street = "135 Business Center Road"
                                },
                                new
                                {
                                    BranchId = new Guid("85728ac7-d654-4b5d-9def-fd8ed92d8538"),
                                    City = "Industrial Park",
                                    Country = "USA",
                                    PostalCode = "10505",
                                    State = "NY",
                                    Street = "579 Factory Lane"
                                },
                                new
                                {
                                    BranchId = new Guid("f6a7c5db-e766-4bf8-b1e3-dcbcd60d73d3"),
                                    City = "Suburbia",
                                    Country = "USA",
                                    PostalCode = "10606",
                                    State = "NY",
                                    Street = "864 Suburban Mall, Unit 42"
                                });
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Product", b =>
                {
                    b.OwnsOne("Ambev.DeveloperEvaluation.Domain.ValueObjects.Money", "UnitPrice", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("PriceAmount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasMaxLength(3)
                                .HasColumnType("character varying(3)")
                                .HasColumnName("PriceCurrency");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");

                            b1.HasData(
                                new
                                {
                                    ProductId = new Guid("f1a6b5e1-c9d0-4a41-8d29-1399e81b3c8d"),
                                    Amount = 2.99m,
                                    Currency = "USD"
                                },
                                new
                                {
                                    ProductId = new Guid("2b9a9e2c-4b1d-476b-a6c2-3834a9023bdd"),
                                    Amount = 3.49m,
                                    Currency = "EUR"
                                },
                                new
                                {
                                    ProductId = new Guid("8d0c1a84-49cf-4f28-86a0-57a82c00db0d"),
                                    Amount = 7.90m,
                                    Currency = "BRL"
                                },
                                new
                                {
                                    ProductId = new Guid("c1d8d459-2149-4147-8e2c-e1d102492481"),
                                    Amount = 3.29m,
                                    Currency = "USD"
                                },
                                new
                                {
                                    ProductId = new Guid("39f45a65-0b1e-43a7-8e44-808a7826ac2b"),
                                    Amount = 4.25m,
                                    Currency = "EUR"
                                },
                                new
                                {
                                    ProductId = new Guid("8f09ce39-b03c-4e45-add5-29cc254e4da2"),
                                    Amount = 2.79m,
                                    Currency = "USD"
                                },
                                new
                                {
                                    ProductId = new Guid("64e69709-6dd0-44c3-8651-9e8a233454cb"),
                                    Amount = 6.50m,
                                    Currency = "BRL"
                                },
                                new
                                {
                                    ProductId = new Guid("e3b46d88-6319-4e64-94a8-90cf612984d1"),
                                    Amount = 5.50m,
                                    Currency = "EUR"
                                },
                                new
                                {
                                    ProductId = new Guid("9a5bfdcd-c03e-4d72-b9d4-bcad47727049"),
                                    Amount = 2.89m,
                                    Currency = "USD"
                                },
                                new
                                {
                                    ProductId = new Guid("d7c14adf-fc3a-45de-be5f-aa7415483a45"),
                                    Amount = 2.49m,
                                    Currency = "USD"
                                });
                        });

                    b.Navigation("UnitPrice")
                        .IsRequired();
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Sale", b =>
                {
                    b.HasOne("Ambev.DeveloperEvaluation.Domain.Entities.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Ambev.DeveloperEvaluation.Domain.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("Ambev.DeveloperEvaluation.Domain.ValueObjects.Money", "TotalAmount", b1 =>
                        {
                            b1.Property<Guid>("SaleId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("TotalAmount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasMaxLength(3)
                                .HasColumnType("character varying(3)")
                                .HasColumnName("Currency");

                            b1.HasKey("SaleId");

                            b1.ToTable("Sales");

                            b1.WithOwner()
                                .HasForeignKey("SaleId");
                        });

                    b.Navigation("Branch");

                    b.Navigation("Customer");

                    b.Navigation("TotalAmount")
                        .IsRequired();
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.SaleItem", b =>
                {
                    b.HasOne("Ambev.DeveloperEvaluation.Domain.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Ambev.DeveloperEvaluation.Domain.Entities.Sale", null)
                        .WithMany("Items")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Ambev.DeveloperEvaluation.Domain.ValueObjects.Money", "UnitPrice", b1 =>
                        {
                            b1.Property<Guid>("SaleItemId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("UnitPrice");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasMaxLength(3)
                                .HasColumnType("character varying(3)")
                                .HasColumnName("UnitPriceCurrency");

                            b1.HasKey("SaleItemId");

                            b1.ToTable("SaleItems");

                            b1.WithOwner()
                                .HasForeignKey("SaleItemId");
                        });

                    b.Navigation("Product");

                    b.Navigation("UnitPrice")
                        .IsRequired();
                });

            modelBuilder.Entity("Ambev.DeveloperEvaluation.Domain.Entities.Sale", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
