using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for managing Customer entities
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Creates a new customer in the repository
    /// </summary>
    /// <param name="customer">Customer entity to create</param>
    /// <returns>The created customer with generated ID</returns>
    Task<Customer> CreateAsync(Customer customer);

    /// <summary>
    /// Retrieves a customer by its ID
    /// </summary>
    /// <param name="id">ID of the customer to retrieve</param>
    /// <returns>The found customer or null if not found</returns>
    Task<Customer?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all customers
    /// </summary>
    /// <returns>Collection of all customers</returns>
    Task<IEnumerable<Customer>> GetAllAsync();

    /// <summary>
    /// Retrieves customers by email
    /// </summary>
    /// <param name="email">Email to search for</param>
    /// <returns>Collection of customers with matching email</returns>
    Task<IEnumerable<Customer>> GetByEmailAsync(string email);

    /// <summary>
    /// Updates an existing customer
    /// </summary>
    /// <param name="customer">Customer entity to update</param>
    /// <returns>The updated customer</returns>
    Task<Customer> UpdateAsync(Customer customer);

    /// <summary>
    /// Deletes a customer by its ID
    /// </summary>
    /// <param name="id">ID of the customer to delete</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// Checks if a customer with the given email already exists
    /// </summary>
    /// <param name="email">Email to check</param>
    /// <param name="excludeId">Optional ID to exclude from check (for updates)</param>
    /// <returns>True if email exists, false otherwise</returns>
    Task<bool> ExistsByEmailAsync(string email, Guid? excludeId = null);
}