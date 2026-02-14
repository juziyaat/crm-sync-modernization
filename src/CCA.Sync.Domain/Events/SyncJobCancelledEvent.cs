using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when a sync job is cancelled.
/// </summary>
public sealed class SyncJobCancelledEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyncJobCancelledEvent"/> class.
    /// </summary>
    /// <param name="syncJobId">The ID of the cancelled sync job</param>
    /// <param name="cancelledAt">The timestamp when the job was cancelled</param>
    /// <param name="reason">The reason for cancellation</param>
    public SyncJobCancelledEvent(Guid syncJobId, DateTime cancelledAt, string reason)
    {
        SyncJobId = syncJobId;
        CancelledAt = cancelledAt;
        Reason = reason;
    }

    /// <summary>
    /// Gets the ID of the cancelled sync job.
    /// </summary>
    public Guid SyncJobId { get; }

    /// <summary>
    /// Gets the timestamp when the job was cancelled.
    /// </summary>
    public DateTime CancelledAt { get; }

    /// <summary>
    /// Gets the reason for cancellation.
    /// </summary>
    public string Reason { get; }
}
