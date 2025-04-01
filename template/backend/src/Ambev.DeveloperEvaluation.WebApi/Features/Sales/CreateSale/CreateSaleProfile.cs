using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// AutoMapper profile that defines mappings for sale-related entities in the create sale operation.
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleProfile"/> class.
    /// Configures mappings between API request/response models and internal command/result objects.
    /// </summary>
    public CreateSaleProfile() // Changed from protected to public
    {
        // Map from API request model to command
        CreateMap<CreateSaleRequest, CreateSaleCommand>();

        // Add this mapping for the nested items
        CreateMap<CreateSaleItemRequest, CreateSaleItemCommand>();

        // Map from internal result to API response model
        CreateMap<CreateSaleResult, CreateSaleResponse>();
    }
}