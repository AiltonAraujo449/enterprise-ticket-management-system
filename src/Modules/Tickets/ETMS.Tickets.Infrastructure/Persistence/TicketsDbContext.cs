using ETMS.Tickets.Domain.Tickets;
using Microsoft.EntityFrameworkCore;

namespace ETMS.Tickets.Infrastructure.Persistence;

public sealed class TicketsDbContext : DbContext
{
    public TicketsDbContext(
        DbContextOptions<TicketsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TicketsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
