namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;

/// <summary>
/// Command for updating a sale item
/// </summary>
public record UpdateSaleItemCommand
{
    /// <summary>
    /// Identifier of the existing sale item (if updating an existing item)
    /// </summary>
    public Guid? SaleItemId { get; set; }

    /// <summary>
    /// Product ID for the item
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; set; }
}