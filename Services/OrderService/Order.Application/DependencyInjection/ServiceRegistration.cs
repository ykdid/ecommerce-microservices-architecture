using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.IntegrationEvents.EventHandlers;

namespace Order.Application.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Integration Event Handlers
        services.AddTransient<StockReservedIntegrationEventHandler>();
        services.AddTransient<StockOutOfStockIntegrationEventHandler>();
        
        return services;
    }
}