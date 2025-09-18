using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;
using Stock.Domain.Enums;

namespace Stock.Infrastructure.Persistence.Configurations;

public class StockItemConfiguration : IEntityTypeConfiguration<StockItem>
{
    public void Configure(EntityTypeBuilder<StockItem> builder)
    {
        builder.ToTable("StockItems");
        
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.ProductId)
            .IsRequired();

        builder.Property(s => s.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.SKU)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Quantity)
            .IsRequired();

        builder.Property(s => s.ReservedQuantity)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(s => s.MinimumStock)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(s => s.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(s => s.LastUpdated)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(s => s.ProductId)
            .IsUnique()
            .HasDatabaseName("IX_StockItems_ProductId");

        builder.HasIndex(s => s.SKU)
            .IsUnique()
            .HasDatabaseName("IX_StockItems_SKU");

        builder.HasIndex(s => s.Status)
            .HasDatabaseName("IX_StockItems_Status");

        // Ignore navigation properties for domain events
        builder.Ignore(s => s.DomainEvents);
    }
}