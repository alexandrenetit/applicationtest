using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Repository implementation for managing Sale entities using Entity Framework Core
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of the SaleRepository
    /// </summary>
    /// <param name="context">The database context to use for operations</param>
    /// <exception cref="ArgumentNullException">Thrown when context is null</exception>
    public SaleRepository(DefaultContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // Create
    /// <summary>
    /// Adds a new sale to the repository
    /// </summary>
    /// <param name="sale">Sale entity to add</param>
    /// <returns>The added sale with any generated fields</returns>
    public async Task<Sale> CreateAsync(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
        return sale;
    }

    // Read
    /// <summary>
    /// Gets a sale by its unique identifier
    /// </summary>
    /// <param name="id">Sale ID</param>
    /// <returns>The sale entity or null if not found</returns>
    public async Task<Sale?> GetByIdAsync(Guid id)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <summary>
    /// Gets a sale by its sale number
    /// </summary>
    /// <param name="saleNumber">Unique sale number</param>
    /// <returns>The sale entity or null if not found</returns>
    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber);
    }

    // <summary>
    /// Gets all sales with optional filtering
    /// </summary>
    /// <param name="status">Optional status filter</param>
    /// <param name="fromDate">Optional start date filter</param>
    /// <param name="toDate">Optional end date filter</param>
    /// <returns>Collection of sales matching criteria</returns>
    public async Task<IEnumerable<Sale>> GetAllAsync(
        SaleStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        var query = _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(s => s.Status == status.Value);

        if (fromDate.HasValue)
            query = query.Where(s => s.SaleDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(s => s.SaleDate <= toDate.Value);

        return await query.ToListAsync();
    }

    // Update
    /// <summary>
    /// Updates an existing sale
    /// </summary>
    /// <param name="sale">Sale entity to update</param>
    /// <returns>The updated sale entity</returns>
    public async Task<Sale> UpdateAsync(Sale sale)
    {
        _context.Entry(sale).State = EntityState.Modified;
        return sale;
    }


    // Delete
    /// <summary>
    /// Removes a sale from the repository
    /// </summary>
    /// <param name="id">ID of the sale to remove</param>
    /// <returns>True if deletion was successful</returns>
    public async void Delete(Guid id)
    {
        var sale = await GetByIdAsync(id);
        
        _context.Sales.Remove(sale);        
    }

    /// Gets all sales for a specific customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <returns>Collection of the customer's sales</returns>
    public async Task<IEnumerable<Sale>> GetByCustomerAsync(Guid customerId)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
            .Where(s => s.Customer.Id == customerId)
            .ToListAsync();
    }

    /// <summary>
    /// Gets all sales for a specific branch
    /// </summary>
    /// <param name="branchId">Branch ID</param>
    /// <returns>Collection of the branch's sales</returns>
    public async Task<IEnumerable<Sale>> GetByBranchAsync(Guid branchId)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
            .Where(s => s.Branch.Id == branchId)
            .ToListAsync();
    }
}