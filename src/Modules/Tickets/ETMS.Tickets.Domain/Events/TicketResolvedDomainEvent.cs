using ETMS.Tickets.Domain.Common;

namespace ETMS.Tickets.Domain.Events;

public sealed record TicketResolvedDomainEvent(
    Guid EventId,
    DateTimeOffset OccurredOn,
    Guid TicketId
) : IDomainEvent;
