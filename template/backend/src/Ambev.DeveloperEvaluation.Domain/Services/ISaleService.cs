using Ambev.DeveloperEvaluation.Domain.Entities;

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
    /// Completes the sale after validation
    /// </summary>
    /// <param name="sale">The sale to complete</param>
    /// <exception cref="InvalidOperationException">Thrown if validation fails</exception>
    void CompleteSale(Sale sale);

    /// <summary>
    /// Cancels a sale with basic validation
    /// </summary>
    /// <param name="sale">The sale to cancel</param>
    /// <param name="reason">Optional cancellation reason</param>
    /// <exception cref="InvalidOperationException">Thrown if sale is already cancelled</exception>
    void CancelSale(Sale sale, string? reason = null);
}