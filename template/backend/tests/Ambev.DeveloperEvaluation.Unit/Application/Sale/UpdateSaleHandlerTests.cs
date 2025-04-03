using Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Entities = Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sale.TestData;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Common.Repositories;
using NSubstitute;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale;

/// <summary>
/// Unit tests for the UpdateSaleCommandHandler
/// Following the Red-Green-Refactor and AAA patterns
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly Mock<ISaleService> _mockSaleService;
    private readonly Mock<ISaleRepository> _mockSaleRepository;
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    private readonly Mock<IBranchRepository> _mockBranchRepository;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IValidator<UpdateSaleCommand>> _mockValidator;
    private readonly Mock<IEventNotification> _mockEventNotification;
    private readonly SaleItemLimitSpecification _saleItemLimitSpecification;
    private readonly SaleUpdateAllowedSpecification _saleUpdateAllowedSpecification;
    private readonly UpdateSaleHandlerTestData _saleHandlerTestData;
    private readonly UpdateSaleCommandHandler _handler;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Set up common test dependencies and create the handler
    /// </summary>
    public UpdateSaleHandlerTests()
    {
        // Set up mocks
        _mockSaleService = new Mock<ISaleService>();
        _mockSaleRepository = new Mock<ISaleRepository>();
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockBranchRepository = new Mock<IBranchRepository>();
        _mockProductRepository = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockValidator = new Mock<IValidator<UpdateSaleCommand>>();
        _mockEventNotification = new Mock<IEventNotification>();

        // Create actual specification instances
        _saleItemLimitSpecification = new SaleItemLimitSpecification();
        _saleUpdateAllowedSpecification = new SaleUpdateAllowedSpecification();

        _unitOfWork = Substitute.For<IUnitOfWork>();

        // Create test data
        _saleHandlerTestData = new UpdateSaleHandlerTestData();

        // Create the handler with the actual dependencies
        _handler = new UpdateSaleCommandHandler(
            _mockSaleService.Object,
            _mockSaleRepository.Object,
            _mockCustomerRepository.Object,
            _mockBranchRepository.Object,
            _mockProductRepository.Object,
            _mockMapper.Object,
            _mockEventNotification.Object,
            _saleUpdateAllowedSpecification,
            _saleItemLimitSpecification,
            _unitOfWork);
    }

    /// <summary>
    /// Test that a valid command returns the expected response
    /// </summary>
    [Fact(DisplayName = "Valid command should update a sale and return a properly mapped response")]
    public async Task Handle_ValidCommand_ReturnsUpdateSaleResponse()
    {
        // Arrange
        var testSetup = _saleHandlerTestData.SetupValidCommandTest();

        // Add this line to set the Status property
        testSetup.Command.Status = SaleStatus.Created;

        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateSaleCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockSaleRepository.Setup(r => r.GetByIdAsync(testSetup.Command.SaleId))
            .ReturnsAsync(testSetup.ExistingSale);

        // Set the sale status to ensure SaleUpdateAllowedSpecification returns true
        testSetup.ExistingSale.Status = SaleStatus.Created;

        _mockCustomerRepository.Setup(r => r.GetByIdAsync(testSetup.Customer.Id))
            .ReturnsAsync(testSetup.Customer);

        _mockBranchRepository.Setup(r => r.GetByIdAsync(testSetup.Branch.Id))
            .ReturnsAsync(testSetup.Branch);

        _mockProductRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => testSetup.Products.FirstOrDefault(p => p.Id == id));

        // Set up UpdateSale to return testSetup.UpdatedSale with ANY parameters
        _mockSaleService.Setup(s => s.UpdateSale(
            It.IsAny<Entities.Sale>(),
            It.IsAny<Customer>(),
            It.IsAny<Branch>(),
            It.IsAny<IEnumerable<(Product, int)>>()))
            .Returns(testSetup.UpdatedSale);

        // Ensure the updated sale doesn't have too many items
        foreach (var item in testSetup.UpdatedSale.Items)
        {
            // Try using the public property, which might work if it has a setter
            try
            {
                var prop = typeof(SaleItem).GetProperty("Quantity");
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(item, 5);
                }
            }
            catch { }
        }

        _mockSaleRepository.Setup(r => r.UpdateAsync(It.IsAny<Entities.Sale>()))
            .ReturnsAsync(testSetup.UpdatedSale);

        _mockEventNotification.Setup(e => e.NotifyAsync(It.IsAny<SaleModifiedEvent>()))
            .Returns(Task.CompletedTask);

        // This is the critical part - make sure the mapper returns a non-null result
        _mockMapper.Setup(m => m.Map<UpdateSaleResult>(It.IsAny<Entities.Sale>()))
            .Returns(testSetup.ExpectedResponse);

        // Make sure testSetup.ExpectedResponse is not null
        Assert.NotNull(testSetup.ExpectedResponse);

        // Act
        var result = await _handler.Handle(testSetup.Command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testSetup.ExpectedResponse.SaleId, result.SaleId);
        Assert.Equal(testSetup.ExpectedResponse.CustomerId, result.CustomerId);
        Assert.Equal(testSetup.ExpectedResponse.BranchId, result.BranchId);

        _mockSaleService.Verify(s => s.UpdateSale(
            It.IsAny<Entities.Sale>(),
            It.IsAny<Customer>(),
            It.IsAny<Branch>(),
            It.IsAny<IEnumerable<(Product, int)>>()), Times.Once);

        _mockSaleRepository.Verify(r => r.UpdateAsync(It.IsAny<Entities.Sale>()), Times.Once);
        _mockMapper.Verify(m => m.Map<UpdateSaleResult>(It.IsAny<Entities.Sale>()), Times.Once);
        _mockEventNotification.Verify(e => e.NotifyAsync(It.IsAny<SaleModifiedEvent>()), Times.Once);
        _unitOfWork.Received(1).ApplyChangesAsync(CancellationToken.None);
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

        _mockSaleRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockCustomerRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockBranchRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockSaleService.Verify(s => s.UpdateSale(
            It.IsAny<Entities.Sale>(),
            It.IsAny<Customer>(),
            It.IsAny<Branch>(),
            It.IsAny<IEnumerable<(Product, int)>>()), Times.Never);
    }

    /// <summary>
    /// Test that when a sale is not found, a DomainException is thrown
    /// </summary>
    [Fact(DisplayName = "Should throw DomainException when sale is not found")]
    public async Task Handle_SaleNotFound_ThrowsDomainException()
    {
        // Arrange
        var testSetup = _saleHandlerTestData.SetupSaleNotFoundTest();

        // Manually set status on the command
        testSetup.Command.Status = SaleStatus.Created;

        _mockSaleRepository.Setup(r => r.GetByIdAsync(testSetup.SaleId))
            .ReturnsAsync((Entities.Sale)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            _handler.Handle(testSetup.Command, CancellationToken.None));

        Assert.Contains($"Sale with Id:{testSetup.SaleId} not found", exception.Message);

        // Verify that subsequent repository and service calls were not made
        _mockCustomerRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockBranchRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockSaleService.Verify(s => s.UpdateSale(
            It.IsAny<Entities.Sale>(),
            It.IsAny<Customer>(),
            It.IsAny<Branch>(),
            It.IsAny<IEnumerable<(Product, int)>>()), Times.Never);
    }

    /// <summary>
    /// Test that when a sale update is not allowed, a DomainException is thrown
    /// </summary>
    [Fact(DisplayName = "Should throw DomainException when sale update is not allowed")]
    public async Task Handle_SaleUpdateNotAllowed_ThrowsDomainException()
    {
        // Arrange
        var testSetup = _saleHandlerTestData.SetupSaleUpdateNotAllowedTest();

        _mockValidator.Setup(v => v.ValidateAsync(testSetup.Command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockSaleRepository.Setup(r => r.GetByIdAsync(testSetup.ExistingSale.Id))
            .ReturnsAsync(testSetup.ExistingSale);

        // Set the sale status to trigger the specification to return false
        testSetup.ExistingSale.Status = SaleStatus.Cancelled;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            _handler.Handle(testSetup.Command, CancellationToken.None));

        Assert.Contains("This sale cannot be updated due to its current status", exception.Message);

        // Verify that subsequent repository and service calls were not made
        _mockCustomerRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockBranchRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockSaleService.Verify(s => s.UpdateSale(
            It.IsAny<Entities.Sale>(),
            It.IsAny<Customer>(),
            It.IsAny<Branch>(),
            It.IsAny<IEnumerable<(Product, int)>>()), Times.Never);
    }

    /// <summary>
    /// Test that when a customer is not found, a DomainException is thrown
    /// </summary>
    [Fact(DisplayName = "Should throw DomainException when customer is not found")]
    public async Task Handle_CustomerNotFound_ThrowsDomainException()
    {
        // Arrange
        var testSetup = _saleHandlerTestData.SetupCustomerNotFoundTest();

        // Set the Status property on the command to prevent validation error
        testSetup.Command.Status = SaleStatus.Created;

        _mockValidator.Setup(v => v.ValidateAsync(testSetup.Command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockSaleRepository.Setup(r => r.GetByIdAsync(testSetup.ExistingSale.Id))
            .ReturnsAsync(testSetup.ExistingSale);

        // Set the sale status to ensure SaleUpdateAllowedSpecification returns true
        testSetup.ExistingSale.Status = SaleStatus.Created;

        _mockCustomerRepository.Setup(r => r.GetByIdAsync(testSetup.CustomerId))
            .ReturnsAsync((Customer)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            _handler.Handle(testSetup.Command, CancellationToken.None));

        Assert.Contains($"Customer with Id:{testSetup.CustomerId} not found", exception.Message);

        // Verify that subsequent repository and service calls were not made
        _mockBranchRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockSaleService.Verify(s => s.UpdateSale(
            It.IsAny<Entities.Sale>(),
            It.IsAny<Customer>(),
            It.IsAny<Branch>(),
            It.IsAny<IEnumerable<(Product, int)>>()), Times.Never);
    }

    /// <summary>
    /// Test that when a branch is not found, an InvalidOperationException is thrown (note that this is different from the CreateSaleHandler)
    /// </summary>
    [Fact(DisplayName = "Should throw InvalidOperationException when branch is not found")]
    public async Task Handle_BranchNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var testSetup = _saleHandlerTestData.SetupBranchNotFoundTest();

        // Add this line to set the Status property
        testSetup.Command.Status = SaleStatus.Created;

        _mockValidator.Setup(v => v.ValidateAsync(testSetup.Command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockSaleRepository.Setup(r => r.GetByIdAsync(testSetup.ExistingSale.Id))
            .ReturnsAsync(testSetup.ExistingSale);

        // Set the sale status to ensure SaleUpdateAllowedSpecification returns true
        testSetup.ExistingSale.Status = SaleStatus.Created;

        _mockCustomerRepository.Setup(r => r.GetByIdAsync(testSetup.Customer.Id))
            .ReturnsAsync(testSetup.Customer);

        _mockBranchRepository.Setup(r => r.GetByIdAsync(testSetup.BranchId))
            .ReturnsAsync((Branch)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(testSetup.Command, CancellationToken.None));

        // Check that the exception message contains the branch ID
        Assert.Contains($"Branch with Id:{testSetup.BranchId} not found", exception.Message);
    }

    /// <summary>
    /// Test that when a product is not found, a DomainException is thrown
    /// </summary>
    [Fact(DisplayName = "Should throw DomainException when product is not found")]
    public async Task Handle_ProductNotFound_ThrowsDomainException()
    {
        // Arrange
        var testSetup = _saleHandlerTestData.SetupProductNotFoundTest();

        // Add this line to set the required Status property
        testSetup.Command.Status = SaleStatus.Created;

        _mockValidator.Setup(v => v.ValidateAsync(testSetup.Command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockSaleRepository.Setup(r => r.GetByIdAsync(testSetup.ExistingSale.Id))
            .ReturnsAsync(testSetup.ExistingSale);

        // Set the sale status to ensure SaleUpdateAllowedSpecification returns true
        testSetup.ExistingSale.Status = SaleStatus.Created;

        _mockCustomerRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(testSetup.Customer);

        _mockBranchRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(testSetup.Branch);

        // Set up the product repository to return null for the missing product ID
        _mockProductRepository.Setup(r => r.GetByIdAsync(testSetup.MissingProductId))
            .ReturnsAsync((Product)null);

        // Set up to return available products for other IDs
        foreach (var product in testSetup.Products)
        {
            _mockProductRepository.Setup(r => r.GetByIdAsync(product.Id))
                .ReturnsAsync(product);
        }

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            _handler.Handle(testSetup.Command, CancellationToken.None));

        Assert.Contains($"Product with Id:{testSetup.MissingProductId} not found", exception.Message);

        // Verify that subsequent service calls were not made
        _mockSaleRepository.Verify(r => r.UpdateAsync(It.IsAny<Entities.Sale>()), Times.Never);
        _mockEventNotification.Verify(e => e.NotifyAsync(It.IsAny<SaleModifiedEvent>()), Times.Never);
    }   
}