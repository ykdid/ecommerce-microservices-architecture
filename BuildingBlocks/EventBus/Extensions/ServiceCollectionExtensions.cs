using EventBus.Abstractions;
using EventBus.Configuration;
using EventBus.EventBusSubscriptionsManager;
using EventBus.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventBus.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMQSettings>(configuration.GetSection(RabbitMQSettings.SectionName));
        
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        
        services.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMQEventBus>>();
            var subsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
            var rabbitMQSettings = configuration.GetSection(RabbitMQSettings.SectionName).Get<RabbitMQSettings>();
            
            return new RabbitMQEventBus(logger, subsManager, sp, Microsoft.Extensions.Options.Options.Create(rabbitMQSettings!));
        });

        return services;
    }
}