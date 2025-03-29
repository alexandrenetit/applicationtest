using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Services;

public class SaleService : ISaleService
{
    /// <summary>
    /// Creates a new sale with basic validation
    /// </summary>
    public Sale CreateSale(Customer customer, Branch branch, string? saleNumber = null)
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        if (branch == null) throw new ArgumentNullException(nameof(branch));

        return new Sale
        {
            SaleNumber = saleNumber ?? GenerateSaleNumber(),
            SaleDate = DateTime.UtcNow,
            Customer = customer,
            Branch = branch,
            Status = SaleStatus.Pending
        };
    }

    /// <summary>
    /// Adds items to sale with basic quantity validation
    /// </summary>
    public void AddItemsToSale(Sale sale, IEnumerable<(Product product, int quantity)> items)
    {
        if (sale == null) throw new ArgumentNullException(nameof(sale));
        if (items == null) throw new ArgumentNullException(nameof(items));

        if (sale.Status != SaleStatus.Pending)
        {
            throw new InvalidOperationException("Items can only be added to pending sales");
        }

        foreach (var (product, quantity) in items)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException($"Invalid quantity {quantity} for product {product.Name}");
            }

            sale.AddItem(product, quantity);
        }
    }

    // <summary>
    /// Completes the sale after validation
    /// </summary>
    public void CompleteSale(Sale sale)
    {
        if (sale == null) throw new ArgumentNullException(nameof(sale));

        var validation = sale.Validate();
        if (!validation.IsValid)
        {
            throw new InvalidOperationException(
                $"Sale validation failed: {string.Join(", ", validation.Errors.Select(e => e.Detail))}");
        }

        // Additional business rules could be added here
        if (sale.Items.Count == 0)
        {
            throw new InvalidOperationException("Cannot complete sale with no items");
        }
    }

    /// <summary>
    /// Cancels a sale with basic validation
    /// </summary>
    public void CancelSale(Sale sale, string? reason = null)
    {
        if (sale == null) throw new ArgumentNullException(nameof(sale));

        if (sale.Status == SaleStatus.Cancelled)
        {
            throw new InvalidOperationException("Sale is already cancelled");
        }

        sale.Cancel();

        // Additional cancellation logic could be added here
    }

    /// <summary>
    /// Simple sale number generation (can be replaced with more complex logic later)
    /// </summary>
    private string GenerateSaleNumber()
    {
        return $"SALE-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
    }
}