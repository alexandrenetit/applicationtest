using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class _202503301116_AddEntitiesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Street = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PriceAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PriceCurrency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SaleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Branches_Id",
                        column: x => x.Id,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sales_Customers_Id",
                        column: x => x.Id,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaleItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    UnitPriceCurrency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(5,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleItems_Products_Id",
                        column: x => x.Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaleItems_Sales_Id",
                        column: x => x.Id,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "Name", "PhoneNumber", "Status", "City", "Country", "PostalCode", "State", "Street" },
                values: new object[,]
                {
                    { new Guid("01c4a1b1-2d77-497a-b15d-3fcb71f66215"), "Southside Center", "555-456-7890", "Active", "Southtown", "USA", "10202", "NY", "321 Southern Road" },
                    { new Guid("3d4dda28-d3e6-4467-94f5-d1b3855f789e"), "Business District Office", "555-890-1234", "Active", "Metropolis", "USA", "10006", "NY", "135 Business Center Road" },
                    { new Guid("49b5898e-2050-438a-b74c-ad4a97e2f9ed"), "Airport Terminal Shop", "555-678-9012", "UnderRenovation", "Metropolis", "USA", "10005", "NY", "987 Airport Terminal C" },
                    { new Guid("4c91b7eb-7b2d-4f1c-b936-e3852e0e3f53"), "North District Branch", "555-345-6789", "Active", "Northville", "USA", "10101", "NY", "789 Northern Boulevard" },
                    { new Guid("7f89f537-8c49-4c90-b21f-ef1a9c5d4cb0"), "Downtown Headquarters", "555-123-4567", "Active", "Metropolis", "USA", "10001", "NY", "123 Main Street" },
                    { new Guid("85728ac7-d654-4b5d-9def-fd8ed92d8538"), "Industrial Zone Branch", "555-901-2345", "TemporarilyClosed", "Industrial Park", "USA", "10505", "NY", "579 Factory Lane" },
                    { new Guid("93e5e068-d2de-4e24-8211-c16e3e6a9a9b"), "Westside Mall Location", "555-234-5678", "Active", "Metropolis", "USA", "10002", "NY", "456 Western Avenue" },
                    { new Guid("bde82fa1-66b9-4ef4-a33b-151fb33f2507"), "East End Location", "555-567-8901", "Active", "Eastville", "USA", "10303", "NY", "654 Eastern Parkway" },
                    { new Guid("c9e06c79-60cc-457f-97c6-e18de43c0e9b"), "University Campus Branch", "555-789-0123", "Active", "College Town", "USA", "10404", "NY", "246 University Drive" },
                    { new Guid("f6a7c5db-e766-4bf8-b1e3-dcbcd60d73d3"), "Suburban Mall Kiosk", "555-012-3456", "ClosedPermanently", "Suburbia", "USA", "10606", "NY", "864 Suburban Mall, Unit 42" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[,]
                {
                    { new Guid("a5b6c7d8-5678-9012-3456-789123456789"), "david.miller@example.com", "David Miller" },
                    { new Guid("b6c7d8e9-6789-0123-4567-891234567890"), "lisa.wilson@example.com", "Lisa Wilson" },
                    { new Guid("c1a2b3d4-1234-5678-9012-345678912345"), "michael.johnson@example.com", "Michael Johnson" },
                    { new Guid("c7d8e9f0-7890-1234-5678-912345678901"), "thomas.moore@example.com", "Thomas Moore" },
                    { new Guid("d2e3f4a5-2345-6789-0123-456789123456"), "sarah.williams@example.com", "Sarah Williams" },
                    { new Guid("d8e9f0a1-8901-2345-6789-123456789012"), "nancy.taylor@example.com", "Nancy Taylor" },
                    { new Guid("e3f4a5b6-3456-7890-1234-567891234567"), "robert.brown@example.com", "Robert Brown" },
                    { new Guid("e9f0a1b2-9012-3456-7890-234567890123"), "james.anderson@example.com", "James Anderson" },
                    { new Guid("f0a1b2c3-0123-4567-8901-345678901234"), "margaret.thomas@example.com", "Margaret Thomas" },
                    { new Guid("f4a5b6c7-4567-8901-2345-678912345678"), "jennifer.davis@example.com", "Jennifer Davis" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "PriceAmount", "PriceCurrency" },
                values: new object[,]
                {
                    { new Guid("2b9a9e2c-4b1d-476b-a6c2-3834a9023bdd"), "Belgian Pilsner", "Stella Artois", 3.49m, "EUR" },
                    { new Guid("39f45a65-0b1e-43a7-8e44-808a7826ac2b"), "Belgian Witbier", "Hoegaarden", 4.25m, "EUR" },
                    { new Guid("64e69709-6dd0-44c3-8651-9e8a233454cb"), "Brazilian Lager", "Skol", 6.50m, "BRL" },
                    { new Guid("8d0c1a84-49cf-4f28-86a0-57a82c00db0d"), "Brazilian Lager", "Brahma", 7.90m, "BRL" },
                    { new Guid("8f09ce39-b03c-4e45-add5-29cc254e4da2"), "Light Lager", "Michelob Ultra", 2.79m, "USD" },
                    { new Guid("9a5bfdcd-c03e-4d72-b9d4-bcad47727049"), "Argentinian Lager", "Quilmes", 2.89m, "USD" },
                    { new Guid("c1d8d459-2149-4147-8e2c-e1d102492481"), "Mexican Lager", "Corona Extra", 3.29m, "USD" },
                    { new Guid("d7c14adf-fc3a-45de-be5f-aa7415483a45"), "Light Lager", "Bud Light", 2.49m, "USD" },
                    { new Guid("e3b46d88-6319-4e64-94a8-90cf612984d1"), "Abbey Beer", "Leffe Blonde", 5.50m, "EUR" },
                    { new Guid("f1a6b5e1-c9d0-4a41-8d29-1399e81b3c8d"), "American Lager", "Budweiser", 2.99m, "USD" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Users");
        }
    }
}
