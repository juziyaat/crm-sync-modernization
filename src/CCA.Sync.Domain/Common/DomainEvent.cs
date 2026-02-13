namespace CCA.Sync.Domain.Common;

/// <summary>
/// Base class for all domain events that are raised by aggregates.
/// </summary>
public abstract class DomainEvent
{
    /// <summary>
    /// Gets the unique identifier for this domain event.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets the timestamp when this domain event occurred.
    /// </summary>
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
