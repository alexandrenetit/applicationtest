using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for managing Branch entities
/// </summary>
public interface IBranchRepository
{
    /// <summary>
    /// Creates a new branch in the repository
    /// </summary>
    /// <param name="branch">Branch entity to create</param>
    /// <returns>The created branch with generated ID</returns>
    Task<Branch> CreateAsync(Branch branch);

    /// <summary>
    /// Retrieves a branch by its ID
    /// </summary>
    /// <param name="id">ID of the branch to retrieve</param>
    /// <returns>The found branch or null if not found</returns>
    Task<Branch?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all branches
    /// </summary>
    /// <returns>Collection of all branches</returns>
    Task<IEnumerable<Branch>> GetAllAsync();

    /// <summary>
    /// Retrieves branches by their status
    /// </summary>
    /// <param name="status">Status to filter by</param>
    /// <returns>Collection of branches with matching status</returns>
    Task<IEnumerable<Branch>> GetByStatusAsync(BranchStatus status);

    /// <summary>
    /// Updates an existing branch
    /// </summary>
    /// <param name="branch">Branch entity to update</param>
    /// <returns>The updated branch</returns>
    Task<Branch> UpdateAsync(Branch branch);

    /// <summary>
    /// Deletes a branch by its ID
    /// </summary>
    /// <param name="id">ID of the branch to delete</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// Checks if a branch with the given name already exists
    /// </summary>
    /// <param name="name">Branch name to check</param>
    /// <param name="excludeId">Optional ID to exclude from check (for updates)</param>
    /// <returns>True if name exists, false otherwise</returns>
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
}