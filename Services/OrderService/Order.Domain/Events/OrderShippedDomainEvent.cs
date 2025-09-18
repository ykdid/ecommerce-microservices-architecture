using Order.Domain.Common;

namespace Order.Domain.Events;

public sealed record OrderShippedDomainEvent(
    Guid OrderId,
    string UserId
) : DomainEvent;