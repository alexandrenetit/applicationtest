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
        //var rabbitConnectionString = configuration.GetConnectionString("RabbitMQ");

        services.AddRebus((configure, provider) => configure
            .Logging(l => l.Console())
            .Transport(t => t.UseInMemoryTransport(new InMemNetwork(true), "ambev-sales"))
            .Routing(r =>
            {
                // For publishing events
                r.TypeBased()
                    .Map<SaleCreatedEvent>("sale.created");
            })
            , onCreated: async bus =>
            {
                await bus.Subscribe<SaleCreatedEvent>();
            }
        );
        services.AutoRegisterHandlersFromAssemblyOf<Program>();
        services.AddScoped<IEventNotification, RebusMessageProducer>();
        return services;
    }
}