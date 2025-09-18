using Order.Domain.Common;

namespace Order.Domain.Events;

public sealed record OrderCreatedDomainEvent(
    Guid OrderId,
    string UserId
) : DomainEvent;