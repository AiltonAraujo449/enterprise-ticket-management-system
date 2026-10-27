
using ETMS.Tickets.Application.Abstractions.Persistence;
using ETMS.Tickets.Infrastructure.Persistence;
using ETMS.Tickets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ETMS.Tickets.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddTicketsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("TicketsDatabase");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'TicketsDatabase' is not configured.");
        }

        services.AddDbContext<TicketsDbContext>(options =>
            options.UseMySQL(connectionString));

        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}

