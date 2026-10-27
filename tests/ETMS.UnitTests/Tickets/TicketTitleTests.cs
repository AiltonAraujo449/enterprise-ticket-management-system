using ETMS.Tickets.Domain.Exceptions;
using ETMS.Tickets.Domain.ValueObjects;

namespace ETMS.UnitTests.Tickets;

public class TicketTitleTests
{
    [Fact]
    public void Create_WithValidTitle_ShouldSucceed()
    {
        var title = TicketTitle.Create("System unavailable");

        Assert.Equal("System unavailable", title.Value);
    }

    [Fact]
    public void Create_ShouldTrimWhitespace()
    {
        var title = TicketTitle.Create("  System unavailable  ");

        Assert.Equal("System unavailable", title.Value);
    }

    [Fact]
    public void Create_WithEmptyTitle_ShouldThrow()
    {
        Assert.Throws<DomainException>(
            () => TicketTitle.Create(""));
    }

    [Fact]
    public void Create_WithTooLongTitle_ShouldThrow()
    {
        var value = new string('A', 151);

        Assert.Throws<DomainException>(
            () => TicketTitle.Create(value));
    }

    [Fact]
    public void TwoTitlesWithSameValue_ShouldBeEqual()
    {
        var first = TicketTitle.Create("System unavailable");
        var second = TicketTitle.Create("System unavailable");

        Assert.Equal(first, second);
    }
}
