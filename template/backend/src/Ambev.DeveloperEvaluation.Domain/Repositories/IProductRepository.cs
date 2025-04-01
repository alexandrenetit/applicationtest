using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Defines repository operations for Product entities
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Creates a new product in the repository
    /// </summary>
    /// <param name="product">Product entity to create</param>
    /// <returns>The created product with generated ID</returns>
    Task<Product> CreateAsync(Product product);

    /// <summary>
    /// Retrieves a product by its ID
    /// </summary>
    /// <param name="id">ID of the product to retrieve</param>
    /// <returns>The found product or null if not found</returns>
    Task<Product?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves multiple products by their IDs
    /// </summary>
    /// <param name="ids">Collection of product IDs to retrieve</param>
    /// <returns>Collection of found products</returns>
    Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids);

    /// <summary>
    /// Retrieves all products
    /// </summary>
    /// <returns>Collection of all products</returns>
    Task<IEnumerable<Product>> GetAllAsync();

    /// <summary>
    /// Retrieves products by name (contains search)
    /// </summary>
    /// <param name="name">Name to search for</param>
    /// <returns>Collection of matching products</returns>
    Task<IEnumerable<Product>> GetByNameAsync(string name);

    /// <summary>
    /// Retrieves products within a price range
    /// </summary>
    /// <param name="minPrice">Minimum price</param>
    /// <param name="maxPrice">Maximum price</param>
    /// <returns>Collection of products in price range</returns>
    Task<IEnumerable<Product>> GetByPriceRangeAsync(Money minPrice, Money maxPrice);

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="product">Product entity to update</param>
    /// <returns>The updated product</returns>
    Task<Product> UpdateAsync(Product product);

    /// <summary>
    /// Deletes a product by its ID
    /// </summary>
    /// <param name="id">ID of the product to delete</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// Checks if a product with the given name already exists
    /// </summary>
    /// <param name="name">Product name to check</param>
    /// <param name="excludeId">Optional ID to exclude from check (for updates)</param>
    /// <returns>True if name exists, false otherwise</returns>
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
}