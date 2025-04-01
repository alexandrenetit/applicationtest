using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Repository implementation for managing Customer entities in the database
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of the CustomerRepository
    /// </summary>
    /// <param name="context">The database context to be used</param>
    /// <exception cref="ArgumentNullException">Thrown when context is null</exception>
    public CustomerRepository(DefaultContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Creates a new customer record in the database
    /// </summary>
    /// <param name="customer">The customer entity to create</param>
    /// <returns>The newly created customer with generated ID</returns>
    public async Task<Customer> CreateAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    /// <summary>
    /// Retrieves a customer by their unique identifier
    /// </summary>
    /// <param name="id">The GUID of the customer to retrieve</param>
    /// <returns>The customer if found, otherwise null</returns>
    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Retrieves all customers from the database
    /// </summary>
    /// <returns>An enumerable collection of all customers ordered by name</returns>
    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves customers by their email address
    /// </summary>
    /// <param name="email">The email address to search for</param>
    /// <returns>Collection of customers with matching email ordered by name</returns>
    public async Task<IEnumerable<Customer>> GetByEmailAsync(string email)
    {
        return await _context.Customers
            .Where(c => c.Email == email)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Updates an existing customer record
    /// </summary>
    /// <param name="customer">The customer entity with updated values</param>
    /// <returns>The updated customer entity</returns>
    public async Task<Customer> UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    /// <summary>
    /// Deletes a customer from the database
    /// </summary>
    /// <param name="id">The GUID of the customer to delete</param>
    /// <returns>True if deletion was successful, false if customer wasn't found</returns>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var customer = await GetByIdAsync(id);
        if (customer == null)
            return false;

        _context.Customers.Remove(customer);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Checks if a customer with the specified email already exists
    /// </summary>
    /// <param name="email">The email address to check</param>
    /// <param name="excludeId">Optional customer ID to exclude from the check</param>
    /// <returns>True if a customer with the email exists, false otherwise</returns>
    public async Task<bool> ExistsByEmailAsync(string email, Guid? excludeId = null)
    {
        var query = _context.Customers.Where(c => c.Email == email);

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}