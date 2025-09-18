using EventBus.Events;
using Stock.Domain.Enums;
using Stock.Domain.Events;

namespace Stock.Domain.Entities;

public class StockItem : BaseEntity
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string SKU { get; private set; }
    public int Quantity { get; private set; }
    public int ReservedQuantity { get; private set; }
    public int MinimumStock { get; private set; }
    public StockStatus Status { get; private set; }
    public DateTime LastUpdated { get; private set; }

    private StockItem() { }

    public StockItem(Guid productId, string productName, string sku, int quantity, int minimumStock)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name cannot be empty", nameof(productName));
        
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU cannot be empty", nameof(sku));

        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(quantity));

        if (minimumStock < 0)
            throw new ArgumentException("Minimum stock cannot be negative", nameof(minimumStock));

        Id = Guid.NewGuid();
        ProductId = productId;
        ProductName = productName;
        SKU = sku;
        Quantity = quantity;
        ReservedQuantity = 0;
        MinimumStock = minimumStock;
        Status = quantity > 0 ? StockStatus.Available : StockStatus.OutOfStock;
        LastUpdated = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
    }

    public bool HasSufficientStock(int requestedQuantity)
    {
        return AvailableQuantity >= requestedQuantity;
    }

    public int AvailableQuantity => Quantity - ReservedQuantity;

    public void ReserveStock(int quantity, Guid orderId)
    {
        if (!HasSufficientStock(quantity))
            throw new InvalidOperationException($"Insufficient stock. Available: {AvailableQuantity}, Requested: {quantity}");

        var previousReserved = ReservedQuantity;
        ReservedQuantity += quantity;
        LastUpdated = DateTime.UtcNow;

        UpdateStatus();

        AddDomainEvent(new StockReservedEvent(ProductId, quantity, orderId));
    }

    public void ReleaseReservedStock(int quantity, Guid orderId)
    {
        if (ReservedQuantity < quantity)
            throw new InvalidOperationException($"Cannot release more than reserved. Reserved: {ReservedQuantity}, Requested: {quantity}");

        ReservedQuantity -= quantity;
        LastUpdated = DateTime.UtcNow;

        UpdateStatus();

        AddDomainEvent(new StockReleasedEvent(ProductId, quantity, orderId));
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(newQuantity));

        if (newQuantity < ReservedQuantity)
            throw new InvalidOperationException($"Cannot set quantity below reserved amount. Reserved: {ReservedQuantity}");

        var previousQuantity = Quantity;
        Quantity = newQuantity;
        LastUpdated = DateTime.UtcNow;

        UpdateStatus();

        AddDomainEvent(new StockUpdatedEvent(ProductId, newQuantity, previousQuantity));

        if (AvailableQuantity == 0 && previousQuantity > 0)
        {
            AddDomainEvent(new StockOutOfStockEvent(ProductId, ProductName));
        }
    }

    public void ConsumeReservedStock(int quantity)
    {
        if (ReservedQuantity < quantity)
            throw new InvalidOperationException($"Cannot consume more than reserved. Reserved: {ReservedQuantity}, Requested: {quantity}");

        ReservedQuantity -= quantity;
        Quantity -= quantity;
        LastUpdated = DateTime.UtcNow;

        UpdateStatus();
    }

    public void UpdateProductInfo(string productName, string sku)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name cannot be empty", nameof(productName));
        
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU cannot be empty", nameof(sku));

        ProductName = productName;
        SKU = sku;
        LastUpdated = DateTime.UtcNow;
    }

    public void SetDiscontinued()
    {
        Status = StockStatus.Discontinued;
        LastUpdated = DateTime.UtcNow;
    }

    private void UpdateStatus()
    {
        if (Status == StockStatus.Discontinued) return;

        Status = AvailableQuantity > 0 ? StockStatus.Available : StockStatus.OutOfStock;
    }

    public bool IsLowStock()
    {
        return AvailableQuantity <= MinimumStock && AvailableQuantity > 0;
    }
}