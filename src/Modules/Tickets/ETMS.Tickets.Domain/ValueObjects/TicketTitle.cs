using ETMS.Tickets.Domain.Exceptions;

namespace ETMS.Tickets.Domain.ValueObjects;

public sealed record TicketTitle
{
    public const int MaxLength = 150;

    public string Value { get; }

    private TicketTitle(string value)
    {
        Value = value;
    }

    public static TicketTitle Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(
                "Ticket title cannot be empty.");

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > MaxLength)
            throw new DomainException(
                $"Ticket title cannot exceed {MaxLength} characters.");

        return new TicketTitle(normalizedValue);
    }

    public override string ToString() => Value;
}
