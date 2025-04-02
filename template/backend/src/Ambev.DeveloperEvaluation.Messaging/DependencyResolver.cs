using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Messaging.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Routing.TypeBased;

namespace Ambev.DeveloperEvaluation.Messaging;

// <summary>
/// Extensions for registering Rebus messaging services
/// </summary>
public static class DependencyResolver
{
    /// <summary>
    /// Adds all Rebus messaging services to the service collection
    /// </summary>
    /// <param name="services">The service collection to configure</param>
    /// <param name="configuration">Application configuration</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddRebusMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitConnectionString = configuration.GetConnectionString("RabbitMQ");

        services.AddRebus((configure, provider) => configure
            .Logging(l => l.Console()) // Or your preferred logger
            .Transport(t => t.UseRabbitMq(rabbitConnectionString, "ambev-sales"))
            .Routing(r => r.TypeBased()
                .Map<SaleCreatedEvent>("ambev-sales"))
            , onCreated: async bus =>
            {
                await bus.Subscribe<SaleCreatedEvent>();
            }
        );

        services.AutoRegisterHandlersFromAssemblyOf<RebusMessageProducer>();
        services.AddScoped<IEventNotification, RebusMessageProducer>();

        return services;
    }
}