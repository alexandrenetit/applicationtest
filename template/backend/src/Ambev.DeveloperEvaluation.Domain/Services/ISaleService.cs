using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Service for managing sales operations
/// </summary>
public interface ISaleService
{
    /// <summary>
    /// Creates a new sale
    /// </summary>
    Task<Sale> CreateSaleAsync(string saleNumber, Customer customer, Branch branch);

    /// <summary>
    /// Adds an item to an existing sale
    /// </summary>
    Task AddItemToSaleAsync(Sale sale, Product product, int quantity);

    /// <summary>
    /// Calculates the total amount for a sale
    /// </summary>
    Task<Money> CalculateTotalAsync(Sale sale);

    /// <summary>
    /// Cancels an existing sale
    /// </summary>
    Task CancelSaleAsync(Sale sale);

    /// <summary>
    /// Validates a sale against business rules
    /// </summary>
    Task<IEnumerable<ValidationErrorDetail>> ValidateSaleAsync(Sale sale);
}