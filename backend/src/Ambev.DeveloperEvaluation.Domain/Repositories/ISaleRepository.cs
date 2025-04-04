using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for CRUD operations on Sale entities
/// </summary>
public interface ISaleRepository
{
    // Create
    /// <summary>
    /// Adds a new sale to the repository
    /// </summary>
    /// <param name="sale">Sale entity to add</param>
    /// <returns>The added sale with any generated fields</returns>
    Task<Sale> CreateAsync(Sale sale);

    // Read
    /// <summary>
    /// Gets a sale by its unique identifier
    /// </summary>
    /// <param name="id">Sale ID</param>
    /// <returns>The sale entity or null if not found</returns>
    Task<Sale?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets a sale by its sale number
    /// </summary>
    /// <param name="saleNumber">Unique sale number</param>
    /// <returns>The sale entity or null if not found</returns>
    Task<Sale?> GetBySaleNumberAsync(string saleNumber);

    /// <summary>
    /// Gets all sales with optional filtering
    /// </summary>
    /// <param name="status">Optional status filter</param>
    /// <param name="fromDate">Optional start date filter</param>
    /// <param name="toDate">Optional end date filter</param>
    /// <returns>Collection of sales matching criteria</returns>
    Task<IEnumerable<Sale>> GetAllAsync(
        SaleStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null);

    // Update
    /// <summary>
    /// Updates an existing sale
    /// </summary>
    /// <param name="sale">Sale entity to update</param>
    /// <returns>The updated sale entity</returns>
    Task<Sale> UpdateAsync(Sale sale);

    // Delete
    /// <summary>
    /// Removes a sale from the repository
    /// </summary>
    /// <param name="id">ID of the sale to remove</param>
    /// <returns>True if deletion was successful</returns>
    void Delete(Guid id);

    /// <summary>
    /// Gets all sales for a specific customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <returns>Collection of the customer's sales</returns>
    Task<IEnumerable<Sale>> GetByCustomerAsync(Guid customerId);

    /// <summary>
    /// Gets all sales for a specific branch
    /// </summary>
    /// <param name="branchId">Branch ID</param>
    /// <returns>Collection of the branch's sales</returns>
    Task<IEnumerable<Sale>> GetByBranchAsync(Guid branchId);

    /// <summary>
    /// Retrieves all paginated sales.
    /// </summary>
    /// <param name="paging">Info to paginate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The list of paginated sales</returns>
    Task<PaginationQueryResult<Sale>> PaginateAsync(
        PaginationQuery paging,
        CancellationToken cancellationToken = default);
}