using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.PaginateSales;

/// <summary>
/// Command for paginate a list of sales.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for paging a list of sales.
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request
/// that returns a <see cref="SaleResult"/>.
/// </remarks>
public class PaginateSalesCommand : PaginationQuery, IRequest<PaginateSalesResult>
{
}