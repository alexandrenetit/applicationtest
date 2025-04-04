﻿using Ambev.DeveloperEvaluation.Domain.Entities;

public interface ISaleService
{
    /// <summary>
    /// Creates a new sale with basic validation
    /// </summary>
    /// <param name="customer">The customer making the purchase</param>
    /// <param name="branch">The branch where the sale occurred</param>
    /// <param name="saleNumber">Optional sale number (will generate if not provided)</param>
    Sale CreateSale(Customer customer, Branch branch, string? saleNumber = null);

    /// <summary>
    /// Adds items to sale with basic quantity validation
    /// </summary>
    /// <param name="sale">The sale to add items to</param>
    /// <param name="items">Collection of products and quantities to add</param>
    void AddItemsToSale(Sale sale, IEnumerable<(Product product, int quantity)> items);    

    /// <summary>
    /// Cancels a sale with basic validation
    /// </summary>
    /// <param name="sale">The sale to cancel</param>
    /// <param name="reason">Optional cancellation reason</param>
    /// <exception cref="InvalidOperationException">Thrown if sale is already cancelled</exception>
    void CancelSale(Sale sale, string? reason = null);

    /// <summary>
    /// Updates an existing sale with new customer, branch information and items
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="customer">The new customer for the sale</param>
    /// <param name="branch">The new branch for the sale</param>
    /// <param name="items">Optional collection of products and quantities to replace existing items</param>
    /// <returns>The updated sale</returns>
    Sale UpdateSale(Sale sale, Customer customer, Branch branch, IEnumerable<(Product product, int quantity)>? items = null);

}