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
            Status = SaleStatus.Created
        };
    }

    /// <summary>
    /// Adds items to sale with basic quantity validation
    /// </summary>
    public void AddItemsToSale(Sale sale, IEnumerable<(Product product, int quantity)> items)
    {
        if (sale == null) throw new DomainException(nameof(sale));
        if (items == null) throw new DomainException(nameof(items));

        if (sale.Status != SaleStatus.Pending)
        {
            throw new DomainException("Items can only be added to pending sales");
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
        if (sale == null) throw new DomainException(nameof(sale));

        var validation = sale.Validate();
        if (!validation.IsValid)
        {
            throw new DomainException(
                $"Sale validation failed: {string.Join(", ", validation.Errors.Select(e => e.Detail))}");
        }

        if (sale.Items.Count == 0)
        {
            throw new DomainException("Cannot complete sale with no items");
        }    
        
        sale.Complete();
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

        sale.Cancel();        
    }

    /// <summary>
    /// Updates an existing sale with new customer, branch information and intelligently manages item changes
    /// </summary>
    /// <remarks>
    /// This method implements a smart item update strategy:
    /// - Adds new products not currently in the sale
    /// - Updates quantities for existing products
    /// - Removes products with zero quantity
    /// - Preserves items not mentioned in the update
    /// </remarks>
    public Sale UpdateSale(Sale sale, Customer customer, Branch branch, IEnumerable<(Product product, int quantity)>? items = null)
    {
        if (sale == null) throw new DomainException(nameof(sale));
        if (customer == null) throw new DomainException(nameof(customer));
        if (branch == null) throw new DomainException(nameof(branch));

        if (sale.Status == SaleStatus.Cancelled)
        {
            throw new DomainException("Cancelled sales cannot be updated");
        }

        if (sale.Status == SaleStatus.Completed)
        {
            throw new DomainException("Completed sales cannot be updated");
        }

        // Update sale properties
        sale.Customer = customer;
        sale.CustomerId = customer.Id;
        sale.Branch = branch;
        sale.BranchId = branch.Id;

        // Process item updates if provided
        if (items != null)
        {
            // Create a copy of the current items to work with
            var existingProducts = sale.Items.ToDictionary(item => item.ProductId, item => item);
            var productsToUpdate = items.ToList();

            // We need to recreate the sale with updated items
            // First, get information about current state
            string currency = sale.TotalAmount.Currency;

            // Create a fresh slate by zeroing the total
            sale.TotalAmount = new Money(0, currency);

            // Track products we've processed to determine which ones to preserve
            var processedProductIds = new HashSet<Guid>();

            // Process all updates
            foreach (var (product, quantity) in productsToUpdate)
            {
                if (product == null)
                {
                    throw new DomainException("Product cannot be null");
                }

                processedProductIds.Add(product.Id);

                // Skip removal of items with zero quantity
                if (quantity <= 0)
                {
                    continue;
                }

                // Add or update the item
                sale.AddItem(product, quantity);
            }

            // Preserve existing items that weren't mentioned in the update
            foreach (var item in existingProducts.Values)
            {
                if (!processedProductIds.Contains(item.ProductId))
                {
                    // Recreate this item since it wasn't mentioned in the updates
                    sale.AddItem(item.Product, item.Quantity);
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