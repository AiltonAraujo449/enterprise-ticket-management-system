
using ETMS.Tickets.Domain.Tickets;
using ETMS.Tickets.Infrastructure.Persistence;
using ETMS.Tickets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ETMS.IntegrationTests.Persistence;

public sealed class TicketRepositoryIntegrationTests
{
    [Fact]
    [Trait("Category", "Database")]
    public async Task AddAndGetById_ShouldPersistAndRestoreTicket()
    {
        var connectionString = Environment.GetEnvironmentVariable(
    "ETMS_TEST_MYSQL_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "ETMS_TEST_MYSQL_CONNECTION_STRING is not configured.");
        }

        var connectionBuilder =
            new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(
                connectionString);

        if (!string.Equals(
            connectionBuilder.Database,
            "etms_test",
            StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Integration tests must use the etms_test database.");
        }

        var options = new DbContextOptionsBuilder<TicketsDbContext>()
            .UseMySQL(connectionString)
            .Options;

        var ticket = Ticket.Create(
            $"Integration test {Guid.NewGuid():N}");

        await using (var writeContext = new TicketsDbContext(options))
        {
            var repository = new TicketRepository(writeContext);
            var unitOfWork = new EfUnitOfWork(writeContext);

            await repository.AddAsync(ticket);
            await unitOfWork.SaveChangesAsync();
        }

        await using (var readContext = new TicketsDbContext(options))
        {
            var repository = new TicketRepository(readContext);

            var restoredTicket = await repository.GetByIdAsync(ticket.Id);

            Assert.NotNull(restoredTicket);
            Assert.Equal(ticket.Id, restoredTicket.Id);
            Assert.Equal(ticket.Title.Value, restoredTicket.Title.Value);
            Assert.Equal(TicketStatus.Open, restoredTicket.Status);
            Assert.Equal(TimeSpan.Zero, restoredTicket.CreatedAt.Offset);

            Assert.Equal(
                ticket.CreatedAt.ToUnixTimeMilliseconds(),
                restoredTicket.CreatedAt.ToUnixTimeMilliseconds());
        }
    }


    [Fact]
    [Trait("Category", "Database")]
    public async Task GetById_ShouldReturnNull_WhenTicketDoesNotExist()
    {
        var connectionString = Environment.GetEnvironmentVariable(
            "ETMS_TEST_MYSQL_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "ETMS_TEST_MYSQL_CONNECTION_STRING is not configured.");
        }

        var connectionBuilder =
            new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(
                connectionString);

        if (!string.Equals(
            connectionBuilder.Database,
            "etms_test",
            StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Integration tests must use the etms_test database.");
        }

        var options = new DbContextOptionsBuilder<TicketsDbContext>()
            .UseMySQL(connectionString)
            .Options;

        await using var context = new TicketsDbContext(options);

        var repository = new TicketRepository(context);

        var result = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }


    [Fact]
    [Trait("Category", "Database")]
    public async Task StatusChange_ShouldPersistAcrossDbContexts()
    {
        var connectionString = Environment.GetEnvironmentVariable(
            "ETMS_TEST_MYSQL_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "ETMS_TEST_MYSQL_CONNECTION_STRING is not configured.");
        }

        var connectionBuilder =
            new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(
                connectionString);

        if (!string.Equals(
            connectionBuilder.Database,
            "etms_test",
            StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Integration tests must use the etms_test database.");
        }

        var options = new DbContextOptionsBuilder<TicketsDbContext>()
            .UseMySQL(connectionString)
            .Options;

        var ticket = Ticket.Create(
            $"Status integration test {Guid.NewGuid():N}");

        await using (var createContext = new TicketsDbContext(options))
        {
            createContext.Tickets.Add(ticket);
            await createContext.SaveChangesAsync();
        }

        await using (var updateContext = new TicketsDbContext(options))
        {
            var storedTicket = await updateContext.Tickets
                .SingleAsync(t => t.Id == ticket.Id);

            storedTicket.StartProgress();

            await updateContext.SaveChangesAsync();
        }

        await using (var readContext = new TicketsDbContext(options))
        {
            var restoredTicket = await readContext.Tickets
                .AsNoTracking()
                .SingleAsync(t => t.Id == ticket.Id);

            Assert.Equal(
                TicketStatus.InProgress,
                restoredTicket.Status);
        }
    }
}
