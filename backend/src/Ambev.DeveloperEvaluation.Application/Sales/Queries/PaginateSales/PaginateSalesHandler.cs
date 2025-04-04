using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries.PaginateSales;

// <summary>
/// Handler for processing <see cref="PaginateSalesCommand"/> requests.
/// </summary>
public class PaginateSalesHandler : IRequestHandler<PaginateSalesCommand, PaginateSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="PaginateSalesHandler"/>.
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public PaginateSalesHandler(
        ISaleRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the PaginateSalesCommand request
    /// </summary>
    /// <param name="request">The PaginateSales command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product details if found</returns>
    public async Task<PaginateSalesResult> Handle(PaginateSalesCommand request, CancellationToken cancellationToken)
    {
        var paginationResult = await _saleRepository.PaginateAsync(request, cancellationToken);

        var result = new PaginateSalesResult
        {
            Items = _mapper.Map<ICollection<SaleResult>>(paginationResult.Items),
            TotalItems = paginationResult.TotalItems
        };

        return result;
    }
}