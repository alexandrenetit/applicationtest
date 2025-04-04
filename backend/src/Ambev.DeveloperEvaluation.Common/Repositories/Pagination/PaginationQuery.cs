namespace Ambev.DeveloperEvaluation.Common.Repositories.Pagination;

/// <summary>
/// Represents a pagination query.
/// </summary>
public class PaginationQuery
{
    /// <summary>
    /// Page number of pagination.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Page size of pagination.
    /// </summary>
    public int Size { get; init; }

    /// <summary>
    /// Ordering of pagination.
    /// </summary>
    public IEnumerable<KeyValuePair<string, SortDirection>> Orders { get; init; } = [];
}