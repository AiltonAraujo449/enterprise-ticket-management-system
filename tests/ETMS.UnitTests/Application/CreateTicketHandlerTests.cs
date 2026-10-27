using ETMS.Tickets.Application.Abstractions.Persistence;
using ETMS.Tickets.Application.Tickets.CreateTicket;
using ETMS.Tickets.Domain.Tickets;

namespace ETMS.UnitTests.Application;

public class CreateTicketHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldCreateTicket()
    {
        var repository = new FakeTicketRepository();
        var handler = new CreateTicketHandler(repository);

        var command = new CreateTicketCommand(
            "System unavailable");

        var ticketId = await handler.HandleAsync(command);

        var ticket = await repository.GetByIdAsync(ticketId);

        Assert.NotNull(ticket);
        Assert.Equal("System unavailable", ticket.Title.Value);
        Assert.Equal(TicketStatus.Open, ticket.Status);
    }

    private sealed class FakeTicketRepository : ITicketRepository
    {
        private readonly Dictionary<Guid, Ticket> _tickets = [];

        public Task AddAsync(
            Ticket ticket,
            CancellationToken cancellationToken = default)
        {
            _tickets.Add(ticket.Id, ticket);

            return Task.CompletedTask;
        }

        public Task<Ticket?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            _tickets.TryGetValue(id, out var ticket);

            return Task.FromResult(ticket);
        }
    }
}
