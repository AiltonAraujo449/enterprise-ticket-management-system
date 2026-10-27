
using ETMS.Tickets.Application.Abstractions.Persistence;
using ETMS.Tickets.Domain.Tickets;
using Microsoft.EntityFrameworkCore;

namespace ETMS.Tickets.Infrastructure.Persistence.Repositories;

public sealed class TicketRepository : ITicketRepository
{
    private readonly TicketsDbContext _context;

    public TicketRepository(TicketsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Ticket ticket,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ticket);

        await _context.Tickets.AddAsync(
            ticket,
            cancellationToken);
    }

    public async Task<Ticket?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tickets
            .AsNoTracking()
            .FirstOrDefaultAsync(
                ticket => ticket.Id == id,
                cancellationToken);
    }
}

