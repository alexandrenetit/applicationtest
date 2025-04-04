using Ambev.DeveloperEvaluation.Application.Sales.Queries;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.PaginateSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Queries.PaginateSales;

/// <summary>
/// Profile for mapping between Application and API PaginateSales responses.
/// </summary>
public class PaginateSalesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for PaginateSales feature.
    /// </summary>
    public PaginateSalesProfile()
    {
        CreateMap<PaginateSalesRequest, PaginateSalesCommand>();
        CreateMap<SaleResult, SaleResponse>();
        CreateMap<SaleItemResult, SaleItemResponse>();
    }
}