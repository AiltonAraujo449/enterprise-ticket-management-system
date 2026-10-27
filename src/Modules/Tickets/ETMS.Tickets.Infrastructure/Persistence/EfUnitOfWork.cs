using ETMS.Tickets.Application.Abstractions.Persistence;

namespace ETMS.Tickets.Infrastructure.Persistence;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly TicketsDbContext _context;

    public EfUnitOfWork(TicketsDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
