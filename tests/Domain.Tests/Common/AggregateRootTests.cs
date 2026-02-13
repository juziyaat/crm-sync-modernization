namespace CCA.Sync.Domain.Tests.Common;

using CCA.Sync.Domain.Common;

public class AggregateRootTests
{
    private sealed class TestAggregateRoot : AggregateRoot<int>
    {
        public TestAggregateRoot(int id)
        {
            Id = id;
        }

        public string? Name { get; set; }

        public void ChangeName(string newName)
        {
            Name = newName;
            RaiseDomainEvent(new NameChangedEvent(Id, newName));
        }
    }

    private sealed class NameChangedEvent : DomainEvent
    {
        public NameChangedEvent(int aggregateId, string newName)
        {
            AggregateId = aggregateId;
            NewName = newName;
        }

        public int AggregateId { get; }

        public string NewName { get; }
    }

    [Fact]
    public void Constructor_InitializesDomainEvents()
    {
        var aggregate = new TestAggregateRoot(1);

        Assert.Empty(aggregate.DomainEvents);
    }

    [Fact]
    public void RaiseDomainEvent_AddsDomainEvent()
    {
        var aggregate = new TestAggregateRoot(1);

        aggregate.ChangeName("New Name");

        Assert.Single(aggregate.DomainEvents);
    }

    [Fact]
    public void RaiseDomainEvent_DomainEventContainsData()
    {
        var aggregate = new TestAggregateRoot(1);

        aggregate.ChangeName("New Name");

        var @event = aggregate.DomainEvents.First() as NameChangedEvent;
        Assert.NotNull(@event);
        Assert.Equal("New Name", @event.NewName);
    }

    [Fact]
    public void MultipleDomainEvents_AllAreAdded()
    {
        var aggregate = new TestAggregateRoot(1);

        aggregate.ChangeName("Name1");
        aggregate.ChangeName("Name2");
        aggregate.ChangeName("Name3");

        Assert.Equal(3, aggregate.DomainEvents.Count);
    }

    [Fact]
    public void ClearDomainEvents_RemovesAllEvents()
    {
        var aggregate = new TestAggregateRoot(1);

        aggregate.ChangeName("Name");
        aggregate.ClearDomainEvents();

        Assert.Empty(aggregate.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_CanRaiseNewEventsAfterClearing()
    {
        var aggregate = new TestAggregateRoot(1);

        aggregate.ChangeName("Name1");
        aggregate.ClearDomainEvents();
        aggregate.ChangeName("Name2");

        Assert.Single(aggregate.DomainEvents);
    }

    [Fact]
    public void InheritsEntityEquality()
    {
        var aggregate1 = new TestAggregateRoot(1) { Name = "Test1" };
        var aggregate2 = new TestAggregateRoot(1) { Name = "Test2" };

        Assert.Equal(aggregate1, aggregate2);
    }

    [Fact]
    public void DifferentIds_AreNotEqual()
    {
        var aggregate1 = new TestAggregateRoot(1);
        var aggregate2 = new TestAggregateRoot(2);

        Assert.NotEqual(aggregate1, aggregate2);
    }
}
