using EventBus.Events;

namespace BuildingBlocks.EventBus.IntegrationEvents;

public class OrderCreatedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }
    public string UserId { get; }
    public List<OrderItemData> OrderItems { get; }
    public decimal TotalAmount { get; }
    public string Currency { get; }

    public OrderCreatedIntegrationEvent(Guid orderId, string userId, List<OrderItemData> orderItems, 
        decimal totalAmount, string currency)
    {
        OrderId = orderId;
        UserId = userId;
        OrderItems = orderItems;
        TotalAmount = totalAmount;
        Currency = currency;
    }
}

public class OrderCancelledIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }
    public List<OrderItemData> OrderItems { get; }

    public OrderCancelledIntegrationEvent(Guid orderId, List<OrderItemData> orderItems)
    {
        OrderId = orderId;
        OrderItems = orderItems;
    }
}

public class StockReservedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }
    public Guid ProductId { get; }
    public int ReservedQuantity { get; }
    public bool Success { get; }
    public string? ErrorMessage { get; }

    public StockReservedIntegrationEvent(Guid orderId, Guid productId, int reservedQuantity, 
        bool success, string? errorMessage = null)
    {
        OrderId = orderId;
        ProductId = productId;
        ReservedQuantity = reservedQuantity;
        Success = success;
        ErrorMessage = errorMessage;
    }
}

public class StockReleasedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }
    public Guid ProductId { get; }
    public int ReleasedQuantity { get; }

    public StockReleasedIntegrationEvent(Guid orderId, Guid productId, int releasedQuantity)
    {
        OrderId = orderId;
        ProductId = productId;
        ReleasedQuantity = releasedQuantity;
    }
}

public class StockOutOfStockIntegrationEvent : IntegrationEvent
{
    public Guid ProductId { get; }
    public string ProductName { get; }

    public StockOutOfStockIntegrationEvent(Guid productId, string productName)
    {
        ProductId = productId;
        ProductName = productName;
    }
}

public class OrderItemData
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Currency { get; set; } = string.Empty;
}