namespace Order.Application.DTOs;

public sealed record OrderDto(
    Guid Id,
    string UserId,
    string Status,
    AddressDto ShippingAddress,
    AddressDto BillingAddress,
    decimal TotalAmount,
    string Currency,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? ShippedAt,
    DateTime? DeliveredAt,
    string? CancellationReason,
    IEnumerable<OrderItemDto> OrderItems
);