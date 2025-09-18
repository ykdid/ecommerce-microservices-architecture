using Order.Domain.Common;
using Order.Domain.Events;
using Order.Domain.Enums;
using Order.Domain.ValueObjects;

namespace Order.Domain.Entities;

public sealed class Order : BaseEntity
{
    private readonly List<OrderItem> _orderItems = [];

    public string UserId { get; private set; }
    public OrderStatus Status { get; private set; }
    public Address ShippingAddress { get; private set; }
    public Address BillingAddress { get; private set; }
    public Money TotalAmount { get; private set; }
    public DateTime? ShippedAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public string? CancellationReason { get; private set; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private Order() { }

    public Order(string userId, Address shippingAddress, Address billingAddress)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        ShippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
        BillingAddress = billingAddress ?? throw new ArgumentNullException(nameof(billingAddress));
        Status = OrderStatus.Pending;
        TotalAmount = new Money(0);

        AddDomainEvent(new OrderCreatedDomainEvent(Id, UserId));
    }

    public void AddOrderItem(Guid productId, string productName, Money unitPrice, int quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot add items to a non-pending order");

        var existingItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var newItem = new OrderItem(productId, productName, unitPrice, quantity);
            _orderItems.Add(newItem);
        }

        RecalculateTotalAmount();
        SetUpdatedAt();
    }

    public void RemoveOrderItem(Guid productId)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot remove items from a non-pending order");

        var item = _orderItems.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            _orderItems.Remove(item);
            RecalculateTotalAmount();
            SetUpdatedAt();
        }
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be confirmed");

        if (!_orderItems.Any())
            throw new InvalidOperationException("Cannot confirm an order without items");

        Status = OrderStatus.Confirmed;
        SetUpdatedAt();

        AddDomainEvent(new OrderConfirmedDomainEvent(Id, UserId, TotalAmount.Amount));
    }

    public void Ship()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed orders can be shipped");

        Status = OrderStatus.Shipped;
        ShippedAt = DateTime.UtcNow;
        SetUpdatedAt();

        AddDomainEvent(new OrderShippedDomainEvent(Id, UserId));
    }

    public void Deliver()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException("Only shipped orders can be delivered");

        Status = OrderStatus.Delivered;
        DeliveredAt = DateTime.UtcNow;
        SetUpdatedAt();

        AddDomainEvent(new OrderDeliveredDomainEvent(Id, UserId));
    }

    public void Cancel(string reason)
    {
        if (Status is OrderStatus.Shipped or OrderStatus.Delivered)
            throw new InvalidOperationException("Cannot cancel shipped or delivered orders");

        Status = OrderStatus.Cancelled;
        CancellationReason = reason;
        SetUpdatedAt();

        AddDomainEvent(new OrderCancelledDomainEvent(Id, UserId, reason));
    }

    private void RecalculateTotalAmount()
    {
        var total = _orderItems.Aggregate(
            new Money(0), 
            (sum, item) => sum + (item.UnitPrice * item.Quantity));
        
        TotalAmount = total;
    }
}