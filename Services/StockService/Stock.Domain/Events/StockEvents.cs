using EventBus.Events;

namespace Stock.Domain.Events;

public class StockReservedEvent : DomainEvent
{
    public Guid ProductId { get; }
    public int Quantity { get; }
    public Guid OrderId { get; }

    public StockReservedEvent(Guid productId, int quantity, Guid orderId)
    {
        ProductId = productId;
        Quantity = quantity;
        OrderId = orderId;
    }
}

public class StockReleasedEvent : DomainEvent
{
    public Guid ProductId { get; }
    public int Quantity { get; }
    public Guid OrderId { get; }

    public StockReleasedEvent(Guid productId, int quantity, Guid orderId)
    {
        ProductId = productId;
        Quantity = quantity;
        OrderId = orderId;
    }
}

public class StockUpdatedEvent : DomainEvent
{
    public Guid ProductId { get; }
    public int NewQuantity { get; }
    public int PreviousQuantity { get; }

    public StockUpdatedEvent(Guid productId, int newQuantity, int previousQuantity)
    {
        ProductId = productId;
        NewQuantity = newQuantity;
        PreviousQuantity = previousQuantity;
    }
}

public class StockOutOfStockEvent : DomainEvent
{
    public Guid ProductId { get; }
    public string ProductName { get; }

    public StockOutOfStockEvent(Guid productId, string productName)
    {
        ProductId = productId;
        ProductName = productName;
    }
}