using Ambev.DeveloperEvaluation.Common.Repositories.Pagination;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.PaginateSales;

/// <summary>
/// Profile for mapping between Sale entity and <see cref="PaginateSalesResult"/>.
/// </summary>
public class PaginateSalesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for Items operation.
    /// </summary>
    public PaginateSalesProfile()
    {
        CreateMap<PaginationQueryResult<Sale>, PaginateSalesResult>();
        CreateMap<Sale, SaleResult>();
        CreateMap<SaleItem, SaleItemResult>();
    }
}