using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Entities = Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale.TestData;

/// <summary>
/// Creating test data for CreateSaleCommandHandler tests
/// </summary>
public class CreateSaleHandlerTestData
{
    private readonly Faker _faker;

    public CreateSaleHandlerTestData()
    {
        _faker = new Faker();
    }

    /// <summary>
    /// Creates a complete test setup for the valid command test case
    /// </summary>
    /// <returns>A complete test setup with command, entities, and expected response</returns>
    public (CreateSaleCommand Command, Customer Customer, Branch Branch, List<Product> Products,
        Entities.Sale Sale, CreateSaleResult ExpectedResponse)
        SetupValidCommandTest()
    {
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();
        var saleId = Guid.NewGuid();

        var command = CreateValidCommand(customerId, branchId, productId1, productId2);
        var customer = CreateCustomer(customerId);
        var branch = CreateBranch(branchId);
        var product1 = CreateProduct(productId1);
        var product2 = CreateProduct(productId2);
        var products = new List<Product> { product1, product2 };

        var sale = CreateSale(saleId, customer, branch);
        var expectedResponse = CreateSaleResponse(saleId, customerId, branchId);

        return (command, customer, branch, products, sale, expectedResponse);
    }

    /// <summary>
    /// Creates a test setup for the invalid command test case
    /// </summary>
    /// <returns>Command and validation failures for testing validation errors</returns>
    public (CreateSaleCommand Command, List<ValidationFailure> ValidationFailures)
        SetupInvalidCommandTest()
    {
        var command = new CreateSaleCommand();
        var validationFailures = CreateValidationFailures();

        return (command, validationFailures);
    }

    /// <summary>
    /// Creates a test setup for the customer not found test case
    /// </summary>
    /// <returns>Command and customer ID for testing customer lookup failure</returns>
    public (CreateSaleCommand Command, Guid CustomerId) SetupCustomerNotFoundTest()
    {
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        // Create a valid command with at least one item to pass validation
        var command = new CreateSaleCommand
        {
            CustomerId = customerId,
            BranchId = branchId,
            SaleNumber = _faker.Random.AlphaNumeric(10),
            Items = new List<CreateSaleItemCommand>
        {
            new CreateSaleItemCommand
            {
                ProductId = Guid.NewGuid(),
                Quantity = _faker.Random.Int(1, 10)
            }
        }
        };

        return (command, customerId);
    }

    /// <summary>
    /// Creates a test setup for the branch not found test case
    /// </summary>
    /// <returns>Command, customer, and branch ID for testing branch lookup failure</returns>
    public (CreateSaleCommand Command, Customer Customer, Guid BranchId) SetupBranchNotFoundTest()
    {
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();

        // Create a command with items to pass validation
        var command = new CreateSaleCommand
        {
            CustomerId = customerId,
            BranchId = branchId,
            SaleNumber = _faker.Random.AlphaNumeric(10),
            Items = new List<CreateSaleItemCommand>
        {
            new CreateSaleItemCommand
            {
                ProductId = Guid.NewGuid(),
                Quantity = _faker.Random.Int(1, 10)
            }
        }
        };

        var customer = CreateCustomer(customerId);

        return (command, customer, branchId);
    }


    /// <summary>
    /// Creates a test setup for the product not found test case
    /// </summary>
    /// <returns>Complete test setup with a missing product ID</returns>
    public (CreateSaleCommand Command, Customer Customer, Branch Branch,
        Entities.Sale Sale, List<Product> AvailableProducts,
        Guid MissingProductId)
        SetupProductNotFoundTest()
    {
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId1 = Guid.NewGuid();
        var missingProductId = Guid.NewGuid();

        var command = CreateValidCommand(customerId, branchId, productId1, missingProductId);
        var customer = CreateCustomer(customerId);
        var branch = CreateBranch(branchId);
        var product1 = CreateProduct(productId1);
        var availableProducts = new List<Product> { product1 };
        var sale = CreateSale(Guid.NewGuid(), customer, branch);

        return (command, customer, branch, sale, availableProducts, missingProductId);
    }

    /// <summary>
    /// Creates a valid CreateSaleCommand with the specified customer, branch, and products
    /// </summary>
    //private CreateSaleCommand CreateValidCommand(Guid customerId,
    //    Guid branchId, params Guid[] productIds)
    //{
    //    var items = new List<CreateSaleItemRequest>
    //    {
    //        new CreateSaleItemRequest
    //        {
    //            ProductId = Guid.NewGuid(),
    //            Quantity = _faker.Random.Int(1, 10)
    //        }
    //    };

    //    return new CreateSaleCommand
    //    {
    //        CustomerId = customerId,
    //        BranchId = branchId,
    //        SaleNumber = _faker.Random.AlphaNumeric(10),
    //        Items = items
    //    };
    //}

    private CreateSaleCommand CreateValidCommand(Guid customerId,
    Guid branchId, params Guid[] productIds)
    {
        var items = new List<CreateSaleItemCommand>();

        // Use the actual product IDs passed to the method
        foreach (var productId in productIds)
        {
            items.Add(new CreateSaleItemCommand
            {
                ProductId = productId,
                Quantity = _faker.Random.Int(1, 10)
            });
        }

        return new CreateSaleCommand
        {
            CustomerId = customerId,
            BranchId = branchId,
            SaleNumber = _faker.Random.AlphaNumeric(10),
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
        return new Product(_faker.Commerce.ProductName(), _faker.Commerce.ProductDescription(),
             new Money(_faker.Random.Decimal(1, 1000), "USD"));
    }

    /// <summary>
    /// Creates a fake sale entity with random data
    /// </summary>
    private DeveloperEvaluation.Domain.Entities.Sale CreateSale(Guid id,
        Customer customer, Branch branch)
    {
        return new DeveloperEvaluation.Domain.Entities.Sale
        {
            Id = id,
            SaleNumber = _faker.Random.AlphaNumeric(10),
            SaleDate = DateTime.Now,
            Customer = customer,
            Branch = branch,
            Status = SaleStatus.Completed,
            CustomerId = customer.Id,
            BranchId = branch.Id
        };
    }

    /// <summary>
    /// Creates a validation failure list for testing validation errors
    /// </summary>
    private List<ValidationFailure> CreateValidationFailures()
    {
        return new List<ValidationFailure>
            {
                new ValidationFailure("CustomerId", "Customer ID is required"),
                new ValidationFailure("BranchId", "Branch ID is required"),
                new ValidationFailure("Items", "At least one item is required")
            };
    }

    /// <summary>
    /// Creates a sample sale response with random data
    /// </summary>
    private CreateSaleResult CreateSaleResponse(Guid saleId, Guid customerId, Guid branchId)
    {
        return new CreateSaleResult
        {
            SaleId = saleId,
            CustomerId = customerId,
            BranchId = branchId,
            SaleNumber = _faker.Random.AlphaNumeric(10),
            SaleDate = DateTime.Now,
            Status = "Completed",
            TotalAmount = _faker.Random.Decimal(100, 10000),
            Currency = "USD",
            CustomerName = _faker.Name.FullName(),
            BranchName = _faker.Company.CompanyName(),
            Items = new List<CreateSaleItemResult>()
        };
    }
}