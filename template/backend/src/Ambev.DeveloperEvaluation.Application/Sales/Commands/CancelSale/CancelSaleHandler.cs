using Ambev.DeveloperEvaluation.Common.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale;

/// <summary>
/// Handles the cancellation of an existing sale
/// </summary>
public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, CancelSaleResponse>
{
    private readonly ISaleService _saleService;
    private readonly ISaleRepository _saleRepository;
    private readonly IEventNotification _eventNotifier;
    private readonly IUnitOfWork _unitOfWork;

    public CancelSaleCommandHandler(
        ISaleService saleService,
        ISaleRepository saleRepository,
        IEventNotification eventNotifier,
        IUnitOfWork unitOfWork)
    {
        _saleService = saleService;
        _saleRepository = saleRepository;
        _eventNotifier = eventNotifier;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the sale cancellation process including validation and business rules
    /// </summary>
    /// <param name="command">The cancellation command containing the sale ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result indicating success or failure of the cancellation</returns>
    public async Task<CancelSaleResponse> Handle(
        CancelSaleCommand command,
        CancellationToken cancellationToken)
    {
        // Validate the command
        var validator = new CancelSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Retrieve the sale
        var sale = await _saleRepository.GetByIdAsync(command.Id);
        if (sale == null)
            throw new DomainException($"Sale with Id:{command.Id} not found");

        // Cancel the sale via domain service
        _saleService.CancelSale(sale);

        // Update the sale in repository
        await _saleRepository.UpdateAsync(sale);

        // Commit changes
        await _unitOfWork.ApplyChangesAsync(cancellationToken);

        // Notify about the sale cancellation event
        await _eventNotifier.NotifyAsync(SaleCancelledEvent.CreateFrom(sale));

        // Return success response
        return new CancelSaleResponse { Success = true };
    }
}