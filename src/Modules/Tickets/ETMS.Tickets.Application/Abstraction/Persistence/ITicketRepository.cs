using ETMS.Tickets.Domain.Tickets;

namespace ETMS.Tickets.Application.Abstractions.Persistence;

public interface ITicketRepository
{
    Task AddAsync(
        Ticket ticket,
        CancellationToken cancellationToken = default);

    Task<Ticket?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
