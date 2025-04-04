using Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale;
using Ambev.DeveloperEvaluation.Common.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using Moq;
using System.ComponentModel;
using Xunit;
using Entities = Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale;

/// <summary>
/// Tests for the CancelSaleCommandHandler to verify correct behavior of cancelling a sale
/// </summary>
public class CancelSaleHandlerTests
{
    private readonly Mock<ISaleService> _mockSaleService;
    private readonly Mock<ISaleRepository> _mockSaleRepository;
    private readonly Mock<IEventNotification> _mockEventNotifier;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CancelSaleCommandHandler _handler;
    private readonly Guid _validSaleId = Guid.NewGuid();
    private readonly Entities.Sale _existingSale;

    /// <summary>
    /// Sets up the common test fixtures and mock dependencies for all tests
    /// </summary>
    public CancelSaleHandlerTests()
    {
        // Setup mocks
        _mockSaleService = new Mock<ISaleService>();
        _mockSaleRepository = new Mock<ISaleRepository>();
        _mockEventNotifier = new Mock<IEventNotification>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        // Create the handler with mocked dependencies
        _handler = new CancelSaleCommandHandler(
            _mockSaleService.Object,
            _mockSaleRepository.Object,
            _mockEventNotifier.Object,
            _mockUnitOfWork.Object);

        // Create a sample sale
        _existingSale = new Entities.Sale
        {
            Id = _validSaleId,
            SaleNumber = "SALE-20250403-1234",
            Status = SaleStatus.Created
        };

        // Setup repository to return the sample sale for valid ID
        _mockSaleRepository
            .Setup(repo => repo.GetByIdAsync(_validSaleId))
            .ReturnsAsync(_existingSale);
    }

    /// <summary>
    /// Verifies that the handler throws a validation exception when the command has an empty sale ID
    /// </summary>
    [Fact]
    [DisplayName("Should throw ValidationException when command has empty sale ID")]
    public async Task Handle_WithInvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CancelSaleCommand(Guid.Empty);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Verifies that the handler throws a domain exception when the sale doesn't exist
    /// </summary>
    [Fact]
    [DisplayName("Should throw DomainException when sale doesn't exist")]
    public async Task Handle_WithNonExistentSale_ShouldThrowDomainException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var command = new CancelSaleCommand(nonExistentId);

        _mockSaleRepository
            .Setup(repo => repo.GetByIdAsync(nonExistentId))
            .ReturnsAsync((Entities.Sale)null);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Verifies that the handler propagates exceptions thrown by the sale service
    /// </summary>
    [Fact]
    [DisplayName("Should propagate exceptions thrown by sale service")]
    public async Task Handle_WhenSaleServiceThrows_ShouldPropagateException()
    {
        // Arrange
        var command = new CancelSaleCommand(_validSaleId);
        var expectedException = new DomainException("Sale is already cancelled");

        _mockSaleService
            .Setup(service => service.CancelSale(It.IsAny<Entities.Sale>(), null))
            .Throws(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.Equal(expectedException.Message, exception.Message);
    }

    /// <summary>
    /// Verifies that the handler propagates exceptions thrown by the repository
    /// </summary>
    [Fact]
    [DisplayName("Should propagate exceptions thrown by repository")]
    public async Task Handle_WhenRepositoryUpdateFails_ShouldPropagateException()
    {
        // Arrange
        var command = new CancelSaleCommand(_validSaleId);
        var expectedException = new InvalidOperationException("Repository error");

        _mockSaleRepository
            .Setup(repo => repo.UpdateAsync(It.IsAny<Entities.Sale>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.Equal(expectedException.Message, exception.Message);
    }

    /// <summary>
    /// Verifies that the handler propagates exceptions thrown by the unit of work
    /// </summary>
    [Fact]
    [DisplayName("Should propagate exceptions thrown by unit of work")]
    public async Task Handle_WhenUnitOfWorkFails_ShouldPropagateException()
    {
        // Arrange
        var command = new CancelSaleCommand(_validSaleId);
        var expectedException = new InvalidOperationException("Transaction error");

        _mockUnitOfWork
            .Setup(uow => uow.ApplyChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        Assert.Equal(expectedException.Message, exception.Message);
    }
}