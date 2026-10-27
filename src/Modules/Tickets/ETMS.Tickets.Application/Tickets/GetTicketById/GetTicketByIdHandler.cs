using ETMS.Tickets.Application.Abstractions.Persistence;
using ETMS.Tickets.Application.DTOs;

namespace ETMS.Tickets.Application.Tickets.GetTicketById;

public sealed class GetTicketByIdHandler
{
    private readonly ITicketRepository _repository;

    public GetTicketByIdHandler(
        ITicketRepository repository)
    {
        _repository = repository;
    }

    public async Task<TicketDto?> HandleAsync(
        GetTicketByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (query.TicketId == Guid.Empty)
            throw new ArgumentException(
                "Ticket ID cannot be empty.",
                nameof(query));

        var ticket = await _repository.GetByIdAsync(
            query.TicketId,
            cancellationToken);

        if (ticket is null)
            return null;

        return new TicketDto(
            ticket.Id,
            ticket.Title.Value,
            ticket.Status.ToString(),
            ticket.CreatedAt);
    }
}
