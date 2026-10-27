using ETMS.Tickets.Domain.Events;
using ETMS.Tickets.Domain.Tickets;

namespace ETMS.UnitTests.Tickets;

public class TicketDomainEventTests
{
    [Fact]
    public void Create_ShouldRegisterTicketCreatedEvent()
    {
        var ticket = Ticket.Create("System unavailable");

        var domainEvent = Assert.Single(ticket.DomainEvents);

        var createdEvent =
            Assert.IsType<TicketCreatedDomainEvent>(domainEvent);

        Assert.Equal(ticket.Id, createdEvent.TicketId);
        Assert.Equal(ticket.Title.Value, createdEvent.Title);
        Assert.NotEqual(Guid.Empty, createdEvent.EventId);
    }

    [Fact]
    public void Resolve_ShouldRegisterTicketResolvedEvent()
    {
        var ticket = Ticket.Create("System unavailable");

        ticket.StartProgress();
        ticket.Resolve();

        Assert.Contains(
            ticket.DomainEvents,
            e => e is TicketResolvedDomainEvent);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        var ticket = Ticket.Create("System unavailable");

        ticket.ClearDomainEvents();

        Assert.Empty(ticket.DomainEvents);
    }
}
