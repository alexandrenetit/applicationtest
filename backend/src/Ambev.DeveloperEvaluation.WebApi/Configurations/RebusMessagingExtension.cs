using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.WebApi.Messages;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;

namespace Ambev.DeveloperEvaluation.WebApi.Configurations;

public static class RebusMessagingExtension
{
    /// <summary>
    /// Adds all Rebus messaging services to the service collection
    /// </summary>
    /// <param name="services">The service collection to configure</param>
    /// <param name="configuration">Application configuration</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddRebusMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRebus((configure, provider) => configure
            .Logging(l => l.Console())
            .Transport(t => t.UseInMemoryTransport(new InMemNetwork(true), "ambev-sales"))
            .Routing(r => r.TypeBased()
                    .Map<SaleCreatedEvent>("sale.created")
                    .Map<SaleModifiedEvent>("sale.modified"))
            , onCreated: async bus =>
            {
                await bus.Subscribe<SaleCreatedEvent>();
                await bus.Subscribe<SaleModifiedEvent>();
            }
        );
        services.AutoRegisterHandlersFromAssemblyOf<Program>();
        services.AddScoped<IEventNotification, RebusMessageProducer>();
        return services;
    }
}