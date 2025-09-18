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
        services.Configure<RabbitMQSettings>(options => 
            configuration.GetSection(RabbitMQSettings.SectionName).Bind(options));
        
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        
        services.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMQEventBus>>();
            var subsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
            var rabbitMQSettings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<RabbitMQSettings>>();
            
            return new RabbitMQEventBus(logger, subsManager, sp, rabbitMQSettings);
        });

        return services;
    }
}