using ETMS.Tickets.Domain.Common;
using ETMS.Tickets.Domain.Events;
using ETMS.Tickets.Domain.Exceptions;
using ETMS.Tickets.Domain.ValueObjects;

namespace ETMS.Tickets.Domain.Tickets;

public sealed class Ticket : AggregateRoot
{
    public TicketTitle Title { get; private set; }

    public TicketStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    private Ticket(
        Guid id,
        TicketTitle title,
        DateTimeOffset createdAt) : base(id)
    {
        Title = title;
        Status = TicketStatus.Open;
        CreatedAt = createdAt;
    }

    public static Ticket Create(string title)
    {
        var ticketTitle = TicketTitle.Create(title);
        var createdAt = DateTimeOffset.UtcNow;

        var ticket = new Ticket(
            Guid.NewGuid(),
            ticketTitle,
            createdAt);

        ticket.AddDomainEvent(
            new TicketCreatedDomainEvent(
                Guid.NewGuid(),
                createdAt,
                ticket.Id,
                ticket.Title.Value));

        return ticket;
    }

    public void StartProgress()
    {
        if (Status != TicketStatus.Open)
            throw new DomainException(
                "Only open tickets can start progress.");

        Status = TicketStatus.InProgress;
    }

    public void Resolve()
    {
        if (Status != TicketStatus.InProgress)
            throw new DomainException(
                "Only tickets in progress can be resolved.");

        Status = TicketStatus.Resolved;

        AddDomainEvent(
            new TicketResolvedDomainEvent(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                Id));
    }

    public void Close()
    {
        if (Status != TicketStatus.Resolved)
            throw new DomainException(
                "Only resolved tickets can be closed.");

        Status = TicketStatus.Closed;
    }
}
