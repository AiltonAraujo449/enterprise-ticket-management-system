using ETMS.Tickets.Domain.Common;

namespace ETMS.Tickets.Domain.Events;

public sealed record TicketCreatedDomainEvent(
    Guid EventId,
    DateTimeOffset OccurredOn,
    Guid TicketId,
    string Title
) : IDomainEvent;
