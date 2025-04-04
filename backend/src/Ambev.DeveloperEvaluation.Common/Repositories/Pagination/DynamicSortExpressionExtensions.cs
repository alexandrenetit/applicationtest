using Ambev.DeveloperEvaluation.Common.Reflection;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Common.Repositories.Pagination;

/// <summary>
/// Extensions to apply dynamic sorting in linq.
/// </summary>
public static class DynamicSortExpressionExtensions
{
    /// <summary>
    /// Rewrite expression appending <see cref="Enumerable.OrderBy"/> and <see cref="Enumerable.ThenBy"/> linq methods dynamically.
    /// </summary>
    /// <typeparam name="TEntity">Principal entity.</typeparam>
    /// <typeparam name="TRelatedEntity">Related entity of navigation.</typeparam>
    /// <param name="expression">Expression</param>
    /// <param name="sorts">List of sorts</param>
    /// <returns>New expression instance.</returns>
    public static Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> RewriteExpressionWithOrderBy<TEntity, TRelatedEntity>(
        this Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> expression,
        IEnumerable<KeyValuePair<string, SortDirection>> sorts)
        where TEntity : class
        where TRelatedEntity : class
    {
        ArgumentNullException.ThrowIfNull(expression, nameof(expression));
        ArgumentNullException.ThrowIfNull(sorts, nameof(sorts));

        var relatedType = typeof(TRelatedEntity);

        var nestedSortProperties = sorts

            .Select(s =>
            {
                var parts = s.Key.Split('.', StringSplitOptions.RemoveEmptyEntries);
                return new
                {
                    Part1 = parts[0],
                    Part2 = parts[1],
                    Direction = s.Value,
                    PropertyType = relatedType.GetProperty(parts[1]).PropertyType,
                };
            })
            .ToArray();

        if (nestedSortProperties.Length > 0)
        {
            ExpressionRewriterBuilder<TEntity, TRelatedEntity> expressionBuilder = new(expression);

            expressionBuilder.AppendOrderBy(
                nestedSortProperties[0].Part2,
                nestedSortProperties[0].PropertyType,
                nestedSortProperties[0].Direction is SortDirection.Ascending);

            for (var i = 1; i < nestedSortProperties.Length; i++)
            {
                expressionBuilder.AppendThenBy(
                    nestedSortProperties[i].Part2,
                    nestedSortProperties[i].PropertyType,
                    nestedSortProperties[i].Direction is SortDirection.Ascending);
            }

            expression = expressionBuilder.Build();
        }

        return expression;
    }
}