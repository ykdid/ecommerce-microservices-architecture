using Order.Domain.Common;
using Order.Domain.ValueObjects;

namespace Order.Domain.Entities;

public sealed class OrderItem : BaseEntity
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public Money TotalPrice => UnitPrice * Quantity;

    private OrderItem() { }

    public OrderItem(Guid productId, string productName, Money unitPrice, int quantity)
    {
        ProductId = productId;
        ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
        UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        Quantity = quantity;
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(newQuantity));
        
        Quantity = newQuantity;
        SetUpdatedAt();
    }

    public void UpdatePrice(Money newPrice)
    {
        UnitPrice = newPrice ?? throw new ArgumentNullException(nameof(newPrice));
        SetUpdatedAt();
    }
}