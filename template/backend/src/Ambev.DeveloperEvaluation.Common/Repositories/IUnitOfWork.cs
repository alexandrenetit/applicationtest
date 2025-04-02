namespace Ambev.DeveloperEvaluation.Common.Repositories
{
    /// <summary>
    /// Provides functionality for hashing and verifying passwords.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Apply in-memory changes to the resource destination.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Number of records affected.</returns>
        Task<int> ApplyChangesAsync(CancellationToken cancellationToken = default);
    }
}