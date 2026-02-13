using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when a sync job begins execution.
/// </summary>
public sealed class SyncJobStartedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyncJobStartedEvent"/> class.
    /// </summary>
    /// <param name="syncJobId">The ID of the sync job that started</param>
    /// <param name="startedAt">The timestamp when the job started</param>
    public SyncJobStartedEvent(Guid syncJobId, DateTime startedAt)
    {
        SyncJobId = syncJobId;
        StartedAt = startedAt;
    }

    /// <summary>
    /// Gets the ID of the sync job that started.
    /// </summary>
    public Guid SyncJobId { get; }

    /// <summary>
    /// Gets the timestamp when the job started.
    /// </summary>
    public DateTime StartedAt { get; }
}
