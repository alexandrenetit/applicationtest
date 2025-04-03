using Ambev.DeveloperEvaluation.Domain.Events;
using Rebus.Handlers;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Messages.Consumers;

public class SaleModifiedConsumer : IHandleMessages<SaleModifiedEvent>
{
    private readonly ILogger _logger;

    public SaleModifiedConsumer(ILogger<SaleCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleModifiedEvent message)
    {
        _logger.LogInformation($"Consuming SaleModifiedEvent: {JsonSerializer.Serialize(message)}");
        return Task.CompletedTask;
    }
}