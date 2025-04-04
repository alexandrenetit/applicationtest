using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Common.Reflection;

/// <summary>
/// Buider to rewrite expression dynamically.
/// </summary>
/// <typeparam name="TSource">Principal accessor type</typeparam>
/// <typeparam name="TRelatedSource">Navegation accessor type</typeparam>
public class ExpressionRewriterBuilder<TSource, TRelatedSource>
    where TSource : class
    where TRelatedSource : class
{
    private readonly Expression _originalBody;
    private readonly ParameterExpression _originalParameter;
    private Expression<Func<TSource, IEnumerable<TRelatedSource>>>? _currentExpression;

    /// <summary>
    /// New instance of <see cref="ExpressionRewriterBuilder{TSource, TRelatedSource}"/>.
    /// </summary>
    /// <param name="expression">Expression to be appended.</param>
    public ExpressionRewriterBuilder(Expression<Func<TSource, IEnumerable<TRelatedSource>>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression, nameof(expression));

        _originalBody = expression.Body;
        _originalParameter = expression.Parameters[0];
    }

    /// <summary>
    /// Append <see cref="Enumerable.OrderBy"/> call inside current expression.
    /// </summary>
    /// <param name="propertyName">Property name</param>
    /// <param name="propertyType">Property type</param>
    /// <param name="ascending">Direction ascending sort</param>
    /// <returns>Current instance of <see cref="ExpressionRewriterBuilder{TSource, TRelatedSource}"/>.</returns>
    public ExpressionRewriterBuilder<TSource, TRelatedSource> AppendOrderBy(
        string propertyName,
        Type propertyType,
        bool ascending)
    {
        ArgumentException.ThrowIfNullOrEmpty(propertyName, nameof(propertyName));
        ArgumentNullException.ThrowIfNull(propertyType, nameof(propertyType));

        // note: parâmetro para o OrderBy (i => i.PurchaseStatus)
        var itemParameter = Expression.Parameter(typeof(TRelatedSource), "i");
        var purchaseStatusProperty = Expression.Property(itemParameter, propertyName);

        // note: lambda para o OrderBy
        var orderByLambda = Expression.Lambda(purchaseStatusProperty, itemParameter);

        var methodName = ascending ? nameof(Enumerable.OrderBy) : nameof(Enumerable.OrderByDescending);

        // note: construção da chamada ao método OrderBy
        var orderByCall = Expression.Call(
            typeof(Enumerable),
            methodName,
            [typeof(TRelatedSource), propertyType],
            _currentExpression?.Body ?? _originalBody,
            orderByLambda
        );

        // note: nova expressão lambda combinando o Where e o OrderBy
        _currentExpression = Expression.Lambda<Func<TSource, IEnumerable<TRelatedSource>>>(orderByCall, _originalParameter);

        return this;
    }

    /// <summary>
    /// Append <see cref="Enumerable.ThenBy"/> call inside current expression.
    /// </summary>
    /// <param name="propertyName">Property name</param>
    /// <param name="propertyType">Property type</param>
    /// <param name="ascending">Direction ascending sort</param>
    /// <returns>Current instance of <see cref="ExpressionRewriterBuilder{TSource, TRelatedSource}"/>.</returns>
    public ExpressionRewriterBuilder<TSource, TRelatedSource> AppendThenBy(
        string propertyName,
        Type propertyType,
        bool ascending)
    {
        ArgumentException.ThrowIfNullOrEmpty(propertyName, nameof(propertyName));
        ArgumentNullException.ThrowIfNull(propertyType, nameof(propertyType));

        // note: parâmetro para o ThenBy (i => i.PurchaseStatus)
        var itemParameter = Expression.Parameter(typeof(TRelatedSource), "i");
        var purchaseStatusProperty = Expression.Property(itemParameter, propertyName);

        // note: lambda para o ThenBy
        var orderByLambda = Expression.Lambda(purchaseStatusProperty, itemParameter);

        var methodName = ascending ? nameof(Enumerable.ThenBy) : nameof(Enumerable.ThenByDescending);

        // note: construção da chamada ao método ThenBy
        var thenByCall = Expression.Call(
            typeof(Enumerable),
            methodName,
            [typeof(TRelatedSource), propertyType],
            _currentExpression?.Body ?? _originalBody,
            orderByLambda
        );

        // note: nova expressão lambda combinando o Where e o ThenBy
        _currentExpression = Expression.Lambda<Func<TSource, IEnumerable<TRelatedSource>>>(thenByCall, _originalParameter);

        return this;
    }

    public Expression<Func<TSource, IEnumerable<TRelatedSource>>> Build()
    {
        return _currentExpression ?? throw new InvalidOperationException("Cannot append expression.");
    }
}