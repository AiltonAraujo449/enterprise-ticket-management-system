using ETMS.Tickets.Domain.Exceptions;
using ETMS.Tickets.Domain.Tickets;
using System.Net.Sockets;

namespace ETMS.UnitTests.Tickets;

public class TicketTests
{
    [Fact]
    public void Create_WithValidTitle_ShouldCreateOpenTicket()
    {
        var ticket = Ticket.Create("System unavailable");

        Assert.NotEqual(Guid.Empty, ticket.Id);
        Assert.Equal("System unavailable", ticket.Title.Value);
        Assert.Equal(TicketStatus.Open, ticket.Status);
    }

    [Fact]
    public void Create_WithEmptyTitle_ShouldThrowDomainException()
    {
        Assert.Throws<DomainException>(
            () => Ticket.Create(""));
    }

    [Fact]
    public void StartProgress_WhenOpen_ShouldChangeStatus()
    {
        var ticket = Ticket.Create("System unavailable");

        ticket.StartProgress();

        Assert.Equal(TicketStatus.InProgress, ticket.Status);
    }

    [Fact]
    public void Resolve_WhenOpen_ShouldThrowDomainException()
    {
        var ticket = Ticket.Create("System unavailable");

        Assert.Throws<DomainException>(
            () => ticket.Resolve());
    }

    [Fact]
    public void CompleteLifecycle_ShouldCloseTicket()
    {
        var ticket = Ticket.Create("System unavailable");

        ticket.StartProgress();
        ticket.Resolve();
        ticket.Close();

        Assert.Equal(TicketStatus.Closed, ticket.Status);
    }
}
