using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Services;

public class SaleService : ISaleService
{
    /// <summary>
    /// Creates a new sale with basic validation
    /// </summary>
    public Sale CreateSale(Customer customer, Branch branch, string? saleNumber = null)
    {
        if (customer == null) throw new DomainException(nameof(customer));
        if (branch == null) throw new DomainException(nameof(branch));

        return new Sale
        {
            SaleNumber = saleNumber ?? GenerateSaleNumber(),
            SaleDate = DateTime.UtcNow,
            Customer = customer,
            Branch = branch,
            Status = SaleStatus.Created,
            CreatedAt = DateTime.UtcNow,
        };
    }

    /// <summary>
    /// Adds items to sale with basic quantity validation
    /// </summary>
    public void AddItemsToSale(Sale sale, IEnumerable<(Product product, int quantity)> items)
    {
        if (sale == null) throw new DomainException(nameof(sale));
        if (items == null) throw new DomainException(nameof(items));

        if (sale.Status == SaleStatus.Cancelled)
        {
            throw new DomainException("Items can only be added to sales canceled");
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

    /// <summary>
    /// Cancels a sale with basic validation
    /// </summary>
    public void CancelSale(Sale sale, string? reason = null)
    {
        if (sale == null) throw new DomainException(nameof(sale));

        if (sale.Status == SaleStatus.Cancelled)
        {
            throw new DomainException("Sale is already cancelled");
        }

        sale.CancelledAt = DateTime.UtcNow;

        sale.Cancel();        
    }

    /// <summary>
    /// Updates an existing sale with new customer, branch, and item information.
    /// Handles item updates by clearing existing items and adding the new items collection.
    /// Validates the updated sale before returning.
    /// </summary>
    public Sale UpdateSale(Sale sale, Customer customer, Branch branch, IEnumerable<(Product product, int quantity)>? items = null)
    {
        if (sale == null) throw new DomainException(nameof(sale));
        if (customer == null) throw new DomainException(nameof(customer));
        if (branch == null) throw new DomainException(nameof(branch));

        if (sale.Status == SaleStatus.Cancelled)
        {
            throw new DomainException("Cancelled sales cannot be updated");
        }

        // Update sale properties
        sale.Customer = customer;
        sale.CustomerId = customer.Id;
        sale.Branch = branch;
        sale.BranchId = branch.Id;
        sale.Status = SaleStatus.Updated;
        sale.UpdatedAt = DateTime.UtcNow;

        // Process item updates if provided
        if (items != null)
        {
            // First group and validate items
            var groupedUpdates = items
                .Where(x => x.product != null)
                .GroupBy(x => x.product.Id)
                .Select(g => (
                    product: g.First().product,
                    quantity: g.Sum(x => x.quantity)
                ))
                .ToList();

            // Clear existing items (preserves currency)
            sale.ClearItems();

            // Add all updated items
            foreach (var (product, quantity) in groupedUpdates)
            {
                if (quantity > 0) // Only add positive quantities
                {
                    sale.AddItem(product, quantity);
                }
            }
        }

        // Validate the updated sale
        var validationResult = sale.Validate();
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.Detail));
            throw new DomainException($"Sale update validation failed: {errors}");
        }

        return sale;
    }

    /// <summary>
    /// Simple sale number generation (can be replaced with more complex logic later)
    /// </summary>
    private string GenerateSaleNumber()
    {
        return $"SALE-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
    }
}