using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Behaviors;
using Stock.Application.IntegrationEvents.EventHandlers;
using System.Reflection;

namespace Stock.Application.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Integration Event Handlers
        services.AddTransient<OrderCreatedIntegrationEventHandler>();
        services.AddTransient<OrderCancelledIntegrationEventHandler>();

        return services;
    }
}