using Microsoft.EntityFrameworkCore;

namespace Product.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Domain.Entities.Product> Products => Set<Domain.Entities.Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Domain.Entities.Product>(builder =>
        {
            builder.OwnsOne(p => p.Price, price =>
            {
                price.Property(p => p.Amount).HasColumnName("Price");
            });

            builder.OwnsOne(p => p.Stock, stock =>
            {
                stock.Property(s => s.Quantity).HasColumnName("Stock");
            });
        });
    }
}