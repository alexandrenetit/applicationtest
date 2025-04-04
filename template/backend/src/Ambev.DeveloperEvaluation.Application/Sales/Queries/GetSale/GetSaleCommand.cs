using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.GetSale;

/// <summary>
/// Represents a command to retrieve a specific sale by its unique identifier.
/// </summary>
public class GetSaleCommand : IRequest<SaleResult>
{
    /// <summary>
    /// The unique identifier of the sale to retrieve.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SaleResult"/>.
    /// </summary>
    /// <param name="id">The ID of the sale to retrieve</param>
    public GetSaleCommand(Guid id)
    {
        Id = id;
    }
}