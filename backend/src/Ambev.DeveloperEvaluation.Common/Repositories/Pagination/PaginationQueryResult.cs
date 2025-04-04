namespace Ambev.DeveloperEvaluation.Common.Repositories.Pagination;

/// <summary>
/// Represents the result from pagination query.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginationQueryResult<T>
{
    /// <summary>
    /// Items of pagination.
    /// </summary>
    public ICollection<T> Items { get; init; } = [];

    /// <summary>
    /// Total items of pagination.
    /// </summary>
    public int TotalItems { get; init; }
}