using ETMS.Tickets.Application.Abstractions.Persistence;
using ETMS.Tickets.Application.Tickets.GetTicketById;
using ETMS.Tickets.Domain.Tickets;

namespace ETMS.UnitTests.Application;

public class GetTicketByIdHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenTicketExists_ShouldReturnDto()
    {
        var repository = new FakeTicketRepository();
        var ticket = Ticket.Create("System unavailable");

        await repository.AddAsync(ticket);

        var handler = new GetTicketByIdHandler(repository);

        var result = await handler.HandleAsync(
            new GetTicketByIdQuery(ticket.Id));

        Assert.NotNull(result);
        Assert.Equal(ticket.Id, result.Id);
        Assert.Equal("System unavailable", result.Title);
        Assert.Equal("Open", result.Status);
        Assert.Equal(ticket.CreatedAt, result.CreatedAt);
    }

    [Fact]
    public async Task HandleAsync_WhenTicketDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeTicketRepository();
        var handler = new GetTicketByIdHandler(repository);

        var result = await handler.HandleAsync(
            new GetTicketByIdQuery(Guid.NewGuid()));

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyId_ShouldThrowArgumentException()
    {
        var repository = new FakeTicketRepository();
        var handler = new GetTicketByIdHandler(repository);

        await Assert.ThrowsAsync<ArgumentException>(
            () => handler.HandleAsync(
                new GetTicketByIdQuery(Guid.Empty)));
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