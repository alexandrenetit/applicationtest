using Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Commands.CancelSale
{
    /// <summary>
    /// AutoMapper profile that defines mappings between API models and application commands/results
    /// </summary>
    public class CancelSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelSaleProfile"/> class.
        /// </summary>
        public CancelSaleProfile()
        {
            CreateMap<CancelSaleRequest, CancelSaleCommand>();
        }
    }
}