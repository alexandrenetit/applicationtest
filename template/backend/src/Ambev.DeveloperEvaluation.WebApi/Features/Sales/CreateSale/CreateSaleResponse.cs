using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents the API response returned after successfully creating a sale, containing all sale details.
/// </summary>
public record CreateSaleResponse
{
    /// <summary>
    /// Gets the unique identifier of the created sale.
    /// </summary>
    public Guid SaleId { get; init; }

    /// <summary>
    /// Gets the business number assigned to the sale.
    /// </summary>
    public string SaleNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets the date and time when the sale was created.
    /// </summary>
    public DateTime SaleDate { get; init; }

    /// <summary>
    /// Gets the current status of the sale (e.g., "Pending", "Completed", "Cancelled").
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Gets the total monetary amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; init; }

    /// <summary>
    /// Gets the currency code in which the total amount is expressed.
    /// </summary>
    public string Currency { get; init; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier of the customer who made the purchase.
    /// </summary>
    public Guid CustomerId { get; init; }

    /// <summary>
    /// Gets the name of the customer who made the purchase.
    /// </summary>
    public string CustomerName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier of the branch where the sale was processed.
    /// </summary>
    public Guid BranchId { get; init; }

    /// <summary>
    /// Gets the name of the branch where the sale was processed.
    /// </summary>
    public string BranchName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the collection of items included in this sale.
    /// </summary>
    public List<CreateSaleItemResult> Items { get; init; } = new();
}
