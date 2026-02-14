using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when a sync job completes successfully.
/// </summary>
public sealed class SyncJobCompletedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyncJobCompletedEvent"/> class.
    /// </summary>
    /// <param name="syncJobId">The ID of the completed sync job</param>
    /// <param name="completedAt">The timestamp when the job completed</param>
    /// <param name="totalRecords">The total number of records processed</param>
    /// <param name="successfulRecords">The number of records successfully synced</param>
    public SyncJobCompletedEvent(Guid syncJobId, DateTime completedAt, int totalRecords, int successfulRecords)
    {
        SyncJobId = syncJobId;
        CompletedAt = completedAt;
        TotalRecords = totalRecords;
        SuccessfulRecords = successfulRecords;
    }

    /// <summary>
    /// Gets the ID of the completed sync job.
    /// </summary>
    public Guid SyncJobId { get; }

    /// <summary>
    /// Gets the timestamp when the job completed.
    /// </summary>
    public DateTime CompletedAt { get; }

    /// <summary>
    /// Gets the total number of records processed.
    /// </summary>
    public int TotalRecords { get; }

    /// <summary>
    /// Gets the number of records successfully synced.
    /// </summary>
    public int SuccessfulRecords { get; }
}
