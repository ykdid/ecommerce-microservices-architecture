using Order.Domain.Common;

namespace Order.Domain.Events;

public sealed record OrderConfirmedDomainEvent(
    Guid OrderId,
    string UserId,
    decimal TotalAmount
) : DomainEvent;