using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Implementation of sale operations
/// </summary>
public class SaleService : ISaleService
{
    private readonly IDiscountCalculator _discountCalculator;

    public SaleService(IDiscountCalculator discountCalculator)
    {
        _discountCalculator = discountCalculator;
    }

    public async Task<Sale> CreateSaleAsync(string saleNumber, Customer customer, Branch branch)
    {
        var sale = new Sale
        {
            SaleNumber = saleNumber,
            SaleDate = DateTime.UtcNow,
            Customer = customer,
            Branch = branch
        };
        var validationErrors = await sale.ValidateAsync();

        if (validationErrors.Any())
        {
            throw new DomainException(
                $"Invalid sale creation: {string.Join(", ", validationErrors.Select(e => e.Detail))}");
        }

        return sale;
    }

    public async Task AddItemToSaleAsync(Sale sale, Product product, int quantity)
    {
        if (sale.Status == SaleStatus.Cancelled)
            throw new DomainException("Cannot add items to a cancelled sale");

        // Apply business rules through discount calculator
        var discount = _discountCalculator.CalculateDiscount(quantity);

        sale.AddItem(product, quantity);

        var validationErrors = await sale.ValidateAsync();
        if (validationErrors.Any())
        {
            throw new DomainException(
                $"Invalid item addition: {string.Join(", ", validationErrors.Select(e => e.Detail))}");
        }
    }

    public Task<Money> CalculateTotalAsync(Sale sale)
    {
        return Task.FromResult(sale.TotalAmount);
    }

    public async Task CancelSaleAsync(Sale sale)
    {
        sale.Cancel();

        var validationErrors = await sale.ValidateAsync();
        if (validationErrors.Any())
        {
            throw new DomainException(
                $"Invalid sale cancellation: {string.Join(", ", validationErrors.Select(e => e.Detail))}");
        }
    }

    public Task<IEnumerable<ValidationErrorDetail>> ValidateSaleAsync(Sale sale)
    {
        return sale.ValidateAsync();
    }
}