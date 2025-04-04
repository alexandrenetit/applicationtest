using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Messaging.Messages.Consumers;

public class SaleCreatedConsumer : IHandleMessages<SaleCreatedEvent>
{
    private readonly ILogger _logger;

    public SaleCreatedConsumer(ILogger<SaleCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCreatedEvent message)
    {
        _logger.LogInformation($"Consuming SaleCreatedEvent: {JsonSerializer.Serialize(message)}");
        return Task.CompletedTask;
    }
}