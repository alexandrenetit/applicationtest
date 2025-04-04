using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;
using FluentValidation.Results;
using Entities = Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale.TestData;

/// <summary>
/// Creating test data for UpdateSaleCommandHandler tests
/// </summary>
public class UpdateSaleHandlerTestData
{
    private readonly Faker _faker;

    public UpdateSaleHandlerTestData()
    {
        _faker = new Faker();
    }

    /// <summary>
    /// Creates a complete test setup for the valid command test case
    /// </summary>
    /// <returns>A complete test setup with command, entities, and expected response</returns>
    public (UpdateSaleCommand Command, Customer Customer, Branch Branch, List<Product> Products,
        Entities.Sale ExistingSale, Entities.Sale UpdatedSale, UpdateSaleResult ExpectedResponse)
        SetupValidCommandTest()
    {
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();
        var saleId = Guid.NewGuid();

        var command = CreateValidCommand(saleId, customerId, branchId, productId1, productId2);
        var customer = CreateCustomer(customerId);
        var branch = CreateBranch(branchId);
        var product1 = CreateProduct(productId1);
        var product2 = CreateProduct(productId2);
        var products = new List<Product> { product1, product2 };

        var existingSale = CreateSale(saleId, Guid.NewGuid(), Guid.NewGuid()); // Different customer/branch IDs
        var updatedSale = CreateUpdatedSale(existingSale, customer, branch);

        // Add items to the updated sale
        foreach (var product in products)
        {
            updatedSale.AddItem(product, _faker.Random.Int(1, 5));
        }

        var expectedResponse = CreateUpdateSaleResponse(saleId, customerId, branchId);

        return (command, customer, branch, products, existingSale, updatedSale, expectedResponse);
    }

    /// <summary>
    /// Creates a test setup for the invalid command test case
    /// </summary>
    /// <returns>Command and validation failures for testing validation errors</returns>
    public (UpdateSaleCommand Command, List<ValidationFailure> ValidationFailures)
        SetupInvalidCommandTest()
    {
        var command = new UpdateSaleCommand();
        var validationFailures = CreateValidationFailures();

        return (command, validationFailures);
    }

    /// <summary>
    /// Creates a test setup for the sale not found test case
    /// </summary>
    /// <returns>Command and sale ID for testing sale lookup failure</returns>
    public (UpdateSaleCommand Command, Guid SaleId) SetupSaleNotFoundTest()
    {
        var saleId = Guid.NewGuid();
        var command = new UpdateSaleCommand
        {
            SaleId = saleId,
            SaleNumber = _faker.Random.AlphaNumeric(10),
            Status = SaleStatus.Created, // Add this
            Items = new List<UpdateSaleItemCommand>
        {
            new UpdateSaleItemCommand
            {
                ProductId = Guid.NewGuid(),
                Quantity = _faker.Random.Int(1, 10)
            }
        }
        };

        return (command, saleId);
    }

    /// <summary>
    /// Creates a test setup for the sale update not allowed test case
    /// </summary>
    /// <returns>Command and existing sale for testing sale update allowed specification</returns>
    public (UpdateSaleCommand Command, Entities.Sale ExistingSale) SetupSaleUpdateNotAllowedTest()
    {
        var saleId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        var command = CreateValidCommand(saleId, customerId, branchId, Guid.NewGuid());
        var existingSale = CreateSale(saleId, customerId, branchId);
        // Set a status that would make the sale not updatable
        existingSale.Status = SaleStatus.Cancelled;

        return (command, existingSale);
    }

    /// <summary>
    /// Creates a test setup for the customer not found test case
    /// </summary>
    /// <returns>Command, existing sale, and customer ID for testing customer lookup failure</returns>
    public (UpdateSaleCommand Command, Entities.Sale ExistingSale, Guid CustomerId)
        SetupCustomerNotFoundTest()
    {
        var saleId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        var command = new UpdateSaleCommand
        {
            SaleId = saleId,
            CustomerId = customerId,
            Items = new List<UpdateSaleItemCommand>
            {
                new UpdateSaleItemCommand
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = _faker.Random.Int(1, 10)
                }
            }
        };

        var existingSale = CreateSale(saleId, Guid.NewGuid(), branchId);

        return (command, existingSale, customerId);
    }

    /// <summary>
    /// Creates a test setup for the branch not found test case
    /// </summary>
    /// <returns>Command, existing sale, customer, and branch ID for testing branch lookup failure</returns>
    public (UpdateSaleCommand Command, Entities.Sale ExistingSale, Customer Customer, Guid BranchId)
    SetupBranchNotFoundTest()
    {
        var saleId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        var command = new UpdateSaleCommand
        {
            SaleId = saleId,
            BranchId = branchId,
            Status = SaleStatus.Created, // Add this line
            Items = new List<UpdateSaleItemCommand>
        {
            new UpdateSaleItemCommand
            {
                ProductId = Guid.NewGuid(),
                Quantity = _faker.Random.Int(1, 10)
            }
        }
        };

        var existingSale = CreateSale(saleId, customerId, Guid.NewGuid());
        var customer = CreateCustomer(customerId);

        return (command, existingSale, customer, branchId);
    }

    /// <summary>
    /// Creates a test setup for the product not found test case
    /// </summary>
    /// <returns>Command, existing sale, customer, branch, and missing product ID for testing product lookup failure</returns>
    public (UpdateSaleCommand Command, Entities.Sale ExistingSale, Customer Customer, Branch Branch,
        List<Product> Products, Guid MissingProductId)
        SetupProductNotFoundTest()
    {
        var saleId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var existingProductId = Guid.NewGuid();
        var missingProductId = Guid.NewGuid();

        var command = new UpdateSaleCommand
        {
            SaleId = saleId,
            Items = new List<UpdateSaleItemCommand>
            {
                new UpdateSaleItemCommand
                {
                    ProductId = existingProductId,
                    Quantity = _faker.Random.Int(1, 10)
                },
                new UpdateSaleItemCommand
                {
                    ProductId = missingProductId,
                    Quantity = _faker.Random.Int(1, 10)
                }
            }
        };

        var existingSale = CreateSale(saleId, customerId, branchId);
        var customer = CreateCustomer(customerId);
        var branch = CreateBranch(branchId);
        var products = new List<Product> { CreateProduct(existingProductId) };

        return (command, existingSale, customer, branch, products, missingProductId);
    }

    /// <summary>
    /// Creates a valid UpdateSaleCommand with the specified sale, customer, branch, and products
    /// </summary>
    private UpdateSaleCommand CreateValidCommand(Guid saleId, Guid customerId,
        Guid branchId, params Guid[] productIds)
    {
        var items = new List<UpdateSaleItemCommand>();

        // Use the actual product IDs passed to the method
        foreach (var productId in productIds)
        {
            items.Add(new UpdateSaleItemCommand
            {
                SaleItemId = Guid.NewGuid(), // Optional sale item ID
                ProductId = productId,
                Quantity = _faker.Random.Int(1, 10)
            });
        }

        return new UpdateSaleCommand
        {
            SaleId = saleId,
            CustomerId = customerId,
            BranchId = branchId,
            SaleNumber = _faker.Random.AlphaNumeric(10),
            Status = SaleStatus.Created,
            Items = items
        };
    }

    /// <summary>
    /// Creates a fake customer entity with random data
    /// </summary>
    private Customer CreateCustomer(Guid id)
    {
        return new Customer
        {
            Id = id,
            Name = _faker.Name.FullName(),
            Email = _faker.Internet.Email()
        };
    }

    /// <summary>
    /// Creates a fake branch entity with random data
    /// </summary>
    private Branch CreateBranch(Guid id)
    {
        return new Branch
        {
            Id = id,
            Name = _faker.Company.CompanyName(),
            PhoneNumber = _faker.Phone.PhoneNumber(),
            Status = BranchStatus.Active,
            Address = new Address(
                _faker.Address.StreetAddress(),
                _faker.Address.City(),
                _faker.Address.State(),
                _faker.Address.ZipCode(),
                _faker.Address.Country())
        };
    }

    /// <summary>
    /// Creates a fake product entity with random data
    /// </summary>
    private Product CreateProduct(Guid id)
    {
        var product = new Product(
            _faker.Commerce.ProductName(),
            _faker.Commerce.ProductDescription(),
            new Money(_faker.Random.Decimal(1, 1000), "USD"));

        // Use reflection to set the ID since it might be protected
        var idProperty = typeof(Product).GetProperty("Id");
        if (idProperty != null && idProperty.CanWrite)
        {
            idProperty.SetValue(product, id);
        }
        else
        {
            // Alternative approach if reflection doesn't work with the actual implementation
            // This assumes there's a method to set the ID or a constructor that takes an ID
            // Adjust according to your actual Product class implementation
        }

        return product;
    }

    /// <summary>
    /// Creates a fake sale entity with random data
    /// </summary>
    private Entities.Sale CreateSale(Guid id, Guid customerId, Guid branchId)
    {
        return new Entities.Sale
        {
            Id = id,
            SaleNumber = _faker.Random.AlphaNumeric(10),
            SaleDate = DateTime.Now,
            CustomerId = customerId,
            BranchId = branchId,
            Status = SaleStatus.Created
        };
    }

    /// <summary>
    /// Creates an updated sale entity based on existing sale and updated entities
    /// </summary>
    private Entities.Sale CreateUpdatedSale(Entities.Sale existingSale, Customer customer, Branch branch)
    {
        var updatedSale = new Entities.Sale
        {
            Id = existingSale.Id,
            SaleNumber = existingSale.SaleNumber,
            SaleDate = existingSale.SaleDate,
            Status = existingSale.Status,
            Customer = customer,
            Branch = branch,
            CustomerId = customer.Id,
            BranchId = branch.Id
        };

        return updatedSale;
    }

    /// <summary>
    /// Creates a validation failure list for testing validation errors
    /// </summary>
    private List<ValidationFailure> CreateValidationFailures()
    {
        return new List<ValidationFailure>
        {
            new ValidationFailure("SaleId", "Sale ID is required"),
            new ValidationFailure("Items", "At least one item is required")
        };
    }

    /// <summary>
    /// Creates a sample sale update response with random data
    /// </summary>
    private UpdateSaleResult CreateUpdateSaleResponse(Guid saleId, Guid customerId, Guid branchId)
    {
        return new UpdateSaleResult
        {
            SaleId = saleId,
            CustomerId = customerId,
            BranchId = branchId,
            SaleNumber = "TEST-123",
            SaleDate = DateTime.Now,
            Status = "Created",
            TotalAmount = 100,
            Currency = "USD",
            CustomerName = "Test Customer",
            BranchName = "Test Branch",
            Items = new List<CreateSaleItemResult>()
        };
    }
}