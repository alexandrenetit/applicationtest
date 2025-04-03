namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Represents the request payload for updating an existing sale in the system.
/// </summary>
/// <remarks>
/// This request allows partial updates to a sale - all fields are optional except SaleId.
/// When fields are not provided (null), their current values will be preserved.
/// </remarks>
public record UpdateSaleRequest
{
    /// <summary>
    /// Gets the unique identifier of the sale to be updated.
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid SaleId { get; init; }

    /// <summary>
    /// Gets the optional new sale number to assign to the sale.
    /// </summary>
    /// <remarks>
    /// If not provided, the existing sale number will be maintained.
    /// Must be unique across all sales if provided.
    /// </remarks>
    /// <example>SALE-2023-00042</example>
    public string? SaleNumber { get; init; }

    /// <summary>
    /// Gets the optional new customer identifier for the sale.
    /// </summary>
    /// <remarks>
    /// If provided, must reference an existing customer in the system.
    /// If not provided, the existing customer association will be maintained.
    /// </remarks>
    /// <example>9e725052-4b5a-4a9f-ac20-8c5a8e4f4e4b</example>
    public Guid? CustomerId { get; init; }

    /// <summary>
    /// Gets the optional new branch identifier where the sale occurred.
    /// </summary>
    /// <remarks>
    /// If provided, must reference an existing branch in the system.
    /// If not provided, the existing branch association will be maintained.
    /// </remarks>
    /// <example>7d2e8e1a-5e9b-4a7c-9e8f-3b6a5d4c3b2a</example>
    public Guid? BranchId { get; init; }

    /// <summary>
    /// Gets the collection of items to update in the sale.
    /// </summary>
    /// <remarks>
    /// When provided, this will replace all existing items in the sale.
    /// To keep existing items unchanged, omit this property or provide an empty list.
    /// Each item must specify a valid product and quantity.
    /// </remarks>
    public List<UpdateSaleItemRequest> Items { get; init; } = new();

    /// <summary>
    /// Gets the optional new status to set for the sale.
    /// </summary>
    /// <remarks>
    /// Valid values: "Pending", "Completed", "Cancelled".
    /// If not provided, the existing status will be maintained.
    /// Certain status changes may be restricted based on business rules.
    /// </remarks>
    /// <example>Completed</example>
    public string? Status { get; init; }
}