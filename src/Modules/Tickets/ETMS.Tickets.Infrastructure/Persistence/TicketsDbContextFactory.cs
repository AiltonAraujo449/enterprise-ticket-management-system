using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ETMS.Tickets.Infrastructure.Persistence;

public sealed class TicketsDbContextFactory
    : IDesignTimeDbContextFactory<TicketsDbContext>
{
    public TicketsDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable(
                "ETMS_MYSQL_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "ETMS_MYSQL_CONNECTION_STRING is not configured.");
        }

        var options = new DbContextOptionsBuilder<TicketsDbContext>()
            .UseMySQL(connectionString)
            .Options;

        return new TicketsDbContext(options);
    }
}
