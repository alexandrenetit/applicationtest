using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Repository implementation for managing Branch entities in the database
/// </summary>
public class BranchRepository : IBranchRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of the BranchRepository
    /// </summary>
    /// <param name="context">The database context to be used</param>
    /// <exception cref="ArgumentNullException">Thrown when context is null</exception>
    public BranchRepository(DefaultContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Creates a new branch in the database
    /// </summary>
    /// <param name="branch">The branch entity to create</param>
    /// <returns>The newly created branch with generated ID</returns>
    public async Task<Branch> CreateAsync(Branch branch)
    {
        _context.Branches.Add(branch);
        await _context.SaveChangesAsync();
        return branch;
    }

    /// <summary>
    /// Retrieves a branch by its unique identifier
    /// </summary>
    /// <param name="id">The GUID of the branch to retrieve</param>
    /// <returns>The branch if found (including Address information), otherwise null</returns>
    public async Task<Branch?> GetByIdAsync(Guid id)
    {
        return await _context.Branches
            .Include(b => b.Address) // Include the Address value object
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    /// <summary>
    /// Retrieves all branches from the database
    /// </summary>
    /// <returns>An enumerable collection of all branches ordered by name</returns>
    public async Task<IEnumerable<Branch>> GetAllAsync()
    {
        return await _context.Branches
            .Include(b => b.Address)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves branches filtered by their operational status
    /// </summary>
    /// <param name="status">The status to filter branches by</param>
    /// <returns>Collection of branches with matching status ordered by name</returns>
    public async Task<IEnumerable<Branch>> GetByStatusAsync(BranchStatus status)
    {
        return await _context.Branches
            .Include(b => b.Address)
            .Where(b => b.Status == status)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Updates an existing branch record
    /// </summary>
    /// <param name="branch">The branch entity with updated values</param>
    /// <returns>The updated branch entity</returns>
    public async Task<Branch> UpdateAsync(Branch branch)
    {
        if (branch == null)
            throw new ArgumentNullException(nameof(branch));

        _context.Branches.Update(branch);
        await _context.SaveChangesAsync();
        return branch;
    }

    /// <summary>
    /// Deletes a branch from the database
    /// </summary>
    /// <param name="id">The GUID of the branch to delete</param>
    /// <returns>
    /// True if deletion was successful,
    /// false if branch wasn't found
    /// </returns>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var branch = await GetByIdAsync(id);
        if (branch == null)
            return false;

        // Prevent deletion of active branches
        if (branch.Status == BranchStatus.Active)
            throw new DomainException("Cannot delete an active branch. Change status first.");

        _context.Branches.Remove(branch);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Checks if a branch with the specified name already exists
    /// </summary>
    /// <param name="name">The branch name to check</param>
    /// <param name="excludeId">
    /// Optional branch ID to exclude from the check (useful for update scenarios)
    /// </param>
    /// <returns>True if a branch with the name exists, false otherwise</returns>
    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
    {
        var query = _context.Branches.Where(b => b.Name == name);

        if (excludeId.HasValue)
        {
            query = query.Where(b => b.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}