using Order.Domain.Common;

namespace Order.Domain.Events;

public sealed record OrderCancelledDomainEvent(
    Guid OrderId,
    string UserId,
    string Reason
) : DomainEvent;