using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when a sync job fails.
/// </summary>
public sealed class SyncJobFailedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyncJobFailedEvent"/> class.
    /// </summary>
    /// <param name="syncJobId">The ID of the failed sync job</param>
    /// <param name="failedAt">The timestamp when the job failed</param>
    /// <param name="reason">The reason for the failure</param>
    public SyncJobFailedEvent(Guid syncJobId, DateTime failedAt, string reason)
    {
        SyncJobId = syncJobId;
        FailedAt = failedAt;
        Reason = reason;
    }

    /// <summary>
    /// Gets the ID of the failed sync job.
    /// </summary>
    public Guid SyncJobId { get; }

    /// <summary>
    /// Gets the timestamp when the job failed.
    /// </summary>
    public DateTime FailedAt { get; }

    /// <summary>
    /// Gets the reason for the failure.
    /// </summary>
    public string Reason { get; }
}
