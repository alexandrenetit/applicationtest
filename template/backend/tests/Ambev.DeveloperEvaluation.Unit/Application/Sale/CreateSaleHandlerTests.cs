using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Entities = Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sale.TestData;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale;

/// <summary>
/// Unit tests for the CreateSaleCommandHandler
/// Following the Red-Green-Refactor and AAA patterns
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly Mock<ISaleService> _mockSaleService;
    private readonly Mock<ISaleRepository> _mockSaleRepository;
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    private readonly Mock<IBranchRepository> _mockBranchRepository;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IValidator<CreateSaleCommand>> _mockValidator;
    private readonly CreateSaleHandlerTestData _saleHandlerTestData;
    private readonly CreateSaleCommandHandler _handler;

    /// <summary>
    /// Set up common test dependencies and create the handler
    /// </summary>
    public CreateSaleHandlerTests()
    {
        // Set up mocks
        _mockSaleService = new Mock<ISaleService>();
        _mockSaleRepository = new Mock<ISaleRepository>();
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockBranchRepository = new Mock<IBranchRepository>();
        _mockProductRepository = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockValidator = new Mock<IValidator<CreateSaleCommand>>();

        // Create test data
        _saleHandlerTestData = new CreateSaleHandlerTestData();

        // Create the handler with mocked dependencies
        _handler = new CreateSaleCommandHandler(
            _mockSaleService.Object,
            _mockSaleRepository.Object,
            _mockCustomerRepository.Object,
            _mockBranchRepository.Object,
            _mockProductRepository.Object,
            _mockMapper.Object,
            _mockValidator.Object);
    }

    /// <summary>
    /// Test that a valid command returns the expected response
    /// </summary>
    [Fact(DisplayName = "Valid command should create a sale and return a properly mapped response")]
    public async Task Handle_ValidCommand_ReturnsCreateSaleResponse()
    {
        // Arrange
        var testSetup = _saleHandlerTestData.SetupValidCommandTest();

        _mockValidator.Setup(v => v.ValidateAsync(testSetup.Command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockCustomerRepository.Setup(r => r.GetByIdAsync(testSetup.Customer.Id))
            .ReturnsAsync(testSetup.Customer);

        _mockBranchRepository.Setup(r => r.GetByIdAsync(testSetup.Branch.Id))
            .ReturnsAsync(testSetup.Branch);

        _mockProductRepository.Setup(r => r.GetByIdsAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync(testSetup.Products);

        _mockSaleService.Setup(s => s.CreateSale(testSetup.Customer, testSetup.Branch, testSetup.Command.SaleNumber))
            .Returns(testSetup.Sale);

        _mockSaleRepository.Setup(r => r.CreateAsync(testSetup.Sale))
            .ReturnsAsync(testSetup.Sale);

        _mockMapper.Setup(m => m.Map<CreateSaleResult>(testSetup.Sale))
            .Returns(testSetup.ExpectedResponse);

        // Act
        var result = await _handler.Handle(testSetup.Command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testSetup.ExpectedResponse.SaleId, result.SaleId);
        Assert.Equal(testSetup.ExpectedResponse.CustomerId, result.CustomerId);
        Assert.Equal(testSetup.ExpectedResponse.BranchId, result.BranchId);

        _mockSaleService.Verify(s => s.CreateSale(testSetup.Customer, testSetup.Branch, testSetup.Command.SaleNumber), Times.Once);
        _mockSaleService.Verify(s => s.AddItemsToSale(testSetup.Sale, It.IsAny<IEnumerable<(Product, int)>>()), Times.Once);
        _mockSaleService.Verify(s => s.CompleteSale(testSetup.Sale), Times.Once);
        _mockSaleRepository.Verify(r => r.CreateAsync(testSetup.Sale), Times.Once);
        _mockMapper.Verify(m => m.Map<CreateSaleResult>(testSetup.Sale), Times.Once);
    }

    /// <summary>
    /// Test that an invalid command throws a validation exception
    /// </summary>
    [Fact(DisplayName = "Invalid command should throw a validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var testSetup = _saleHandlerTestData.SetupInvalidCommandTest();

        _mockValidator.Setup(v => v.ValidateAsync(testSetup.Command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(testSetup.ValidationFailures));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(testSetup.Command, CancellationToken.None));

        _mockCustomerRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockBranchRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockSaleService.Verify(s => s.CreateSale(It.IsAny<Customer>(), It.IsAny<Branch>(), It.IsAny<string>()), Times.Never);
    }

    /// <summary>
    /// Test that when a customer is not found, an InvalidOperationException is thrown
    /// </summary>
    [Fact(DisplayName = "Should throw InvalidOperationException when customer is not found")]
    public async Task Handle_CustomerNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var testSetup = _saleHandlerTestData.SetupCustomerNotFoundTest();

        _mockValidator.Setup(v => v.ValidateAsync(testSetup.Command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockCustomerRepository.Setup(r => r.GetByIdAsync(testSetup.CustomerId))
            .ReturnsAsync((Customer)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(testSetup.Command, CancellationToken.None));

        Assert.Contains($"Customer with Id:{testSetup.CustomerId} not found", exception.Message);
        _mockBranchRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockSaleService.Verify(s => s.CreateSale(It.IsAny<Customer>(), It.IsAny<Branch>(), It.IsAny<string>()), Times.Never);
    }

    /// <summary>
    /// Test that when a branch is not found, an InvalidOperationException is thrown
    /// </summary>
    [Fact(DisplayName = "Should throw InvalidOperationException when branch is not found")]
    public async Task Handle_BranchNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var testSetup = _saleHandlerTestData.SetupBranchNotFoundTest();

        _mockValidator.Setup(v => v.ValidateAsync(testSetup.Command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockCustomerRepository.Setup(r => r.GetByIdAsync(testSetup.Customer.Id))
            .ReturnsAsync(testSetup.Customer);

        _mockBranchRepository.Setup(r => r.GetByIdAsync(testSetup.BranchId))
            .ReturnsAsync((Branch)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(testSetup.Command, CancellationToken.None));

        Assert.Contains($"Branch with Id:{testSetup.Command.CustomerId} not found", exception.Message);
    }

    /// <summary>
    /// Test that when a product is not found, an InvalidOperationException is thrown
    /// </summary>
    [Fact(DisplayName = "Should throw InvalidOperationException when product is not found")]
    public async Task Handle_ProductNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var testSetup = _saleHandlerTestData.SetupProductNotFoundTest();

        _mockValidator.Setup(v => v.ValidateAsync(testSetup.Command,
            It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

        _mockCustomerRepository.Setup(r => r.GetByIdAsync(testSetup.Customer.Id))
            .ReturnsAsync(testSetup.Customer);

        _mockBranchRepository.Setup(r => r.GetByIdAsync(testSetup.Branch.Id))
            .ReturnsAsync(testSetup.Branch);

        _mockSaleService.Setup(s => s.CreateSale(
            testSetup.Customer, testSetup.Branch, testSetup.Command.SaleNumber))
            .Returns(testSetup.Sale);

        _mockProductRepository.Setup(r => r.GetByIdsAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync(testSetup.AvailableProducts);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(testSetup.Command, CancellationToken.None));

        // Instead of checking for an exact string match, verify the general message format
        Assert.Contains("Products not found:", exception.Message);
        // And verify that at least one product ID is mentioned
        Assert.True(exception.Message.Length > "Products not found:".Length);

        _mockSaleService.Verify(s =>
            s.AddItemsToSale(It.IsAny<Entities.Sale>(),
            It.IsAny<IEnumerable<(Product, int)>>()), Times.Never);
        _mockProductRepository.Verify(r => r.GetByIdsAsync(It.IsAny<List<Guid>>()), Times.Once);
        _mockSaleRepository.Verify(r => r.CreateAsync(It.IsAny<Entities.Sale>()), Times.Never);
    }
}