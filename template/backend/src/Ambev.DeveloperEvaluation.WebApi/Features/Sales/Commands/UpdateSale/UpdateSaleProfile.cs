using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Enums;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Commands.UpdateSale;

/// <summary>
/// AutoMapper profile that defines mappings between API models and application commands/results
/// for the update sale operation.
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleProfile"/> class.
    /// Configures all necessary mappings for converting between:
    /// - API request models and application commands
    /// - Application results and API response models
    /// </summary>
    public UpdateSaleProfile()
    {
        // Map from API request model to command
        CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
            .ForMember(dest => dest.Status,
                      opt => opt.MapFrom(src => src.Status != null ?
                          Enum.Parse<SaleStatus>(src.Status) : (SaleStatus?)null));

        // Map for the nested items
        CreateMap<UpdateSaleItemRequest, UpdateSaleItemCommand>();

        // Map from internal result to API response model
        CreateMap<UpdateSaleResult, UpdateSaleResponse>();
        CreateMap<CreateSaleItemResult, UpdateSaleItemResponse>();
    }
}