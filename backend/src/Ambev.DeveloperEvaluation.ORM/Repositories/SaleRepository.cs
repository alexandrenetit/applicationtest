using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Extensions;
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

    /// <inheritdoc/>
    public async Task<Sale> CreateAsync(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
        return sale;
    }

    /// <inheritdoc/>
    public async Task<Sale?> GetByIdAsync(Guid id)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <inheritdoc/>
    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task<Sale> UpdateAsync(Sale sale)
    {
        _context.Entry(sale).State = EntityState.Modified;
        return sale;
    }

    /// <inheritdoc/>
    public async void Delete(Guid id)
    {
        var sale = await GetByIdAsync(id);

        _context.Sales.Remove(sale);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Sale>> GetByCustomerAsync(Guid customerId)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
            .Where(s => s.Customer.Id == customerId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Sale>> GetByBranchAsync(Guid branchId)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
            .Where(s => s.Branch.Id == branchId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<PaginationQueryResult<Sale>> PaginateAsync(
            PaginationQuery paging,
            CancellationToken cancellationToken = default)
    {
        var sortsByRelatedItems = paging.Orders.Where(s => s.Key.StartsWith(nameof(Sale.Items) + '.', StringComparison.OrdinalIgnoreCase));

        return await _context.Sales
            .AsNoTracking()
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
            .ThenInclude(item => item.Product)
            .ToPaginateAsync(paging, cancellationToken);
    }
}