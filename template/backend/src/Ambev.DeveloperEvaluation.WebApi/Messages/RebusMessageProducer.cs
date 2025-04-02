using Ambev.DeveloperEvaluation.Domain.Events;
using Rebus.Bus;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Messages;

// <summary>
/// Responsable to send event message.
/// </summary>
public class RebusMessageProducer : IEventNotification
{
    private readonly IBus _bus;
    private readonly ILogger _logger;

    public RebusMessageProducer(
        IBus bus,
        ILogger<RebusMessageProducer> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task NotifyAsync<T>(T messageEvent)
    {
        _logger.LogInformation($"Publish event of type {typeof(T).Name} " +
            $"=> {JsonSerializer.Serialize(messageEvent)}");

        // Check if we've registered a destination for this message type
        await _bus.Publish(messageEvent);

        // For debugging, add this:
        _logger.LogInformation($"Event published successfully");
    }
}
