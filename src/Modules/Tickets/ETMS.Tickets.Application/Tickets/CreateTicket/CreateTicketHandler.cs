using ETMS.Tickets.Application.Abstractions.Persistence;
using ETMS.Tickets.Domain.Tickets;

namespace ETMS.Tickets.Application.Tickets.CreateTicket;

public sealed class CreateTicketHandler
{
    private readonly ITicketRepository _repository;

    public CreateTicketHandler(ITicketRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> HandleAsync(
        CreateTicketCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var ticket = Ticket.Create(command.Title);

        await _repository.AddAsync(
            ticket,
            cancellationToken);

        return ticket.Id;
    }
}
