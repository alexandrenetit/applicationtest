using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Repository implementation for Product entities
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of the ProductRepository
    /// </summary>
    /// <param name="context">Database context</param>
    /// <exception cref="ArgumentNullException">Thrown when context is null</exception>
    public ProductRepository(DefaultContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Creates a new product in the repository
    /// </summary>
    /// <param name="product">Product entity to create</param>
    /// <returns>The created product with generated ID</returns>
    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// Retrieves a product by its ID
    /// </summary>
    /// <param name="id">ID of the product to retrieve</param>
    /// <returns>The found product or null if not found</returns>
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Retrieves multiple products by their IDs
    /// </summary>
    /// <param name="ids">Collection of product IDs to retrieve</param>
    /// <returns>Collection of found products</returns>
    public async Task<List<Product>> GetByIdsAsync(List<Guid> ids)
    {
        return await _context.Products
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all products
    /// </summary>
    /// <returns>Collection of all products ordered by name</returns>
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves products by name (contains search)
    /// </summary>
    /// <param name="name">Name to search for</param>
    /// <returns>Collection of matching products ordered by name</returns>
    public async Task<IEnumerable<Product>> GetByNameAsync(string name)
    {
        return await _context.Products
            .Where(p => p.Name.Contains(name))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves products within a price range
    /// </summary>
    /// <param name="minPrice">Minimum price</param>
    /// <param name="maxPrice">Maximum price</param>
    /// <returns>Collection of products in price range ordered by price then name</returns>
    public async Task<IEnumerable<Product>> GetByPriceRangeAsync(Money minPrice, Money maxPrice)
    {
        return await _context.Products
            .Where(p => p.UnitPrice.Amount >= minPrice.Amount &&
                       p.UnitPrice.Amount <= maxPrice.Amount)
            .OrderBy(p => p.UnitPrice.Amount)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="product">Product entity to update</param>
    /// <returns>The updated product</returns>
    public async Task<Product> UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// Deletes a product by its ID
    /// </summary>
    /// <param name="id">ID of the product to delete</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await GetByIdAsync(id);
        if (product == null)
            return false;

        _context.Products.Remove(product);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Checks if a product with the given name already exists
    /// </summary>
    /// <param name="name">Product name to check</param>
    /// <param name="excludeId">Optional ID to exclude from check (for updates)</param>
    /// <returns>True if name exists, false otherwise</returns>
    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
    {
        var query = _context.Products.Where(p => p.Name == name);

        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}