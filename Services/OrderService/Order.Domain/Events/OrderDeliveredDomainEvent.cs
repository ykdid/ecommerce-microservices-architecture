using Order.Domain.Common;

namespace Order.Domain.Events;

public sealed record OrderDeliveredDomainEvent(
    Guid OrderId,
    string UserId
) : DomainEvent;