using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;

namespace Ambev.DeveloperEvaluation.ORM.Extensions;

internal static class QueryableExtensions
{
    public static async Task<PaginationQueryResult<T>> ToPaginateAsync<T>(
        this IQueryable<T> queryable,
        PaginationQuery query,
        CancellationToken cancellationToken = default)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(queryable, nameof(queryable));
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        var total = await queryable.CountAsync(cancellationToken);

        var items = await ApplyOrderBy(queryable, query.Orders)
            .Skip(Math.Max(query.Page - 1, 0) * query.Size)
            .Take(Math.Max(query.Size, 1))
            .ToArrayAsync(cancellationToken);

        return new()
        {
            Items = items,
            TotalItems = total,
        };
    }

    private static IQueryable<T> ApplyOrderBy<T>(
        IQueryable<T> queryable,
        IEnumerable<KeyValuePair<string, SortDirection>> orders)
        where T : class
    {
        IOrderedQueryable<T>? orderedQuery = null;

        var sortsWithoutNavigation = orders.Where(o => !o.Key.Contains('.'));

        foreach (var order in sortsWithoutNavigation)
        {
            try
            {
                var expression = $"{order.Key} {(order.Value is SortDirection.Ascending ? "asc" : "desc")}";
                if (orderedQuery is not null)
                {
                    orderedQuery = orderedQuery.ThenBy(expression);
                }
                else
                {
                    orderedQuery = queryable.OrderBy(expression);
                }
            }
            catch (ParseException ex)
            {
                throw new ValidationException(
                [
                    new(string.Empty, ex.Message),
                ]);
            }
        }

        return orderedQuery is null ? queryable : orderedQuery;
    }
}