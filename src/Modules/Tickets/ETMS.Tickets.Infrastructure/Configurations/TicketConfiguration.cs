using ETMS.Tickets.Domain.Tickets;
using ETMS.Tickets.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETMS.Tickets.Infrastructure.Persistence.Configurations;

public sealed class TicketConfiguration
    : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");

        builder.HasKey(ticket => ticket.Id);

        builder.Property(ticket => ticket.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(ticket => ticket.Title)
            .HasColumnName("title")
            .HasConversion(
                title => title.Value,
                value => TicketTitle.Create(value))
            .HasMaxLength(TicketTitle.MaxLength)
            .IsRequired();

        builder.Property(ticket => ticket.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(ticket => ticket.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime(6)")
            .HasConversion(
                value => value.UtcDateTime,
                value => new DateTimeOffset(
            DateTime.SpecifyKind(value, DateTimeKind.Utc)))
            .IsRequired();

        builder.Ignore(ticket => ticket.DomainEvents);
    }
}
