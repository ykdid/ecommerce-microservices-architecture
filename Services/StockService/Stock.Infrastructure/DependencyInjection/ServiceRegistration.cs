using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Abstractions.Repositories;
using Stock.Infrastructure.Persistence;
using Stock.Infrastructure.Repositories;

namespace Stock.Infrastructure.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<StockDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Interface registration for DbContext
        services.AddScoped<IStockDbContext>(provider => provider.GetRequiredService<StockDbContext>());

        // Repositories
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }
}