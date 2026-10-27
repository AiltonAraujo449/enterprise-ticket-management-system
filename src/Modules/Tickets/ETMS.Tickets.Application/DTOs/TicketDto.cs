namespace ETMS.Tickets.Application.DTOs;

public sealed record TicketDto(
    Guid Id,
    string Title,
    string Status,
    DateTimeOffset CreatedAt
);
