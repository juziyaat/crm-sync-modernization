namespace CCA.Sync.Domain.Tests.Common;

using CCA.Sync.Domain.Common;

public class DomainEventTests
{
    private sealed class TestDomainEvent : DomainEvent
    {
        public TestDomainEvent(string data)
        {
            Data = data;
        }

        public string Data { get; }
    }

    [Fact]
    public void Constructor_SetsId()
    {
        var @event = new TestDomainEvent("test");

        Assert.NotEqual(Guid.Empty, @event.Id);
    }

    [Fact]
    public void Constructor_SetsOccurredOnToCurrentTime()
    {
        var beforeCreation = DateTime.UtcNow;
        var @event = new TestDomainEvent("test");
        var afterCreation = DateTime.UtcNow;

        Assert.True(@event.OccurredOn >= beforeCreation);
        Assert.True(@event.OccurredOn <= afterCreation);
    }

    [Fact]
    public void MultipleInstances_HaveDifferentIds()
    {
        var event1 = new TestDomainEvent("test");
        var event2 = new TestDomainEvent("test");

        Assert.NotEqual(event1.Id, event2.Id);
    }

    [Fact]
    public void OccurredOn_IsUtcTime()
    {
        var @event = new TestDomainEvent("test");

        Assert.Equal(DateTimeKind.Utc, @event.OccurredOn.Kind);
    }

    [Fact]
    public void CustomData_IsStored()
    {
        var @event = new TestDomainEvent("custom data");

        Assert.Equal("custom data", @event.Data);
    }
}
