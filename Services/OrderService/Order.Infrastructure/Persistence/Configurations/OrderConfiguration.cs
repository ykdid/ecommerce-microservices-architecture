using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Domain.Entities.Order>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
    {
        builder.ToTable("Orders");
        
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.UserId)
            .IsRequired()
            .HasMaxLength(450);
        
        builder.Property(o => o.Status)
            .HasConversion<string>();
        
        builder.Property(o => o.CancellationReason)
            .HasMaxLength(500);

        builder.OwnsOne(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.Street).HasColumnName("ShippingStreet").HasMaxLength(200);
            address.Property(a => a.City).HasColumnName("ShippingCity").HasMaxLength(100);
            address.Property(a => a.State).HasColumnName("ShippingState").HasMaxLength(100);
            address.Property(a => a.Country).HasColumnName("ShippingCountry").HasMaxLength(100);
            address.Property(a => a.ZipCode).HasColumnName("ShippingZipCode").HasMaxLength(20);
        });

        builder.OwnsOne(o => o.BillingAddress, address =>
        {
            address.Property(a => a.Street).HasColumnName("BillingStreet").HasMaxLength(200);
            address.Property(a => a.City).HasColumnName("BillingCity").HasMaxLength(100);
            address.Property(a => a.State).HasColumnName("BillingState").HasMaxLength(100);
            address.Property(a => a.Country).HasColumnName("BillingCountry").HasMaxLength(100);
            address.Property(a => a.ZipCode).HasColumnName("BillingZipCode").HasMaxLength(20);
        });

        builder.OwnsOne(o => o.TotalAmount, money =>
        {
            money.Property(m => m.Amount).HasColumnName("TotalAmount").HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3);
        });

        // Navigation properties
        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(o => o.UserId);
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.CreatedAt);
    }
}