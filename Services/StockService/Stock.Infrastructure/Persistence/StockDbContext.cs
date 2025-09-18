using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Persistence;

public interface IStockDbContext
{
    DbSet<StockItem> StockItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class StockDbContext : DbContext, IStockDbContext
{
    public StockDbContext(DbContextOptions<StockDbContext> options) : base(options) { }

    public DbSet<StockItem> StockItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StockDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Domain events could be dispatched here if needed
        return await base.SaveChangesAsync(cancellationToken);
    }
}