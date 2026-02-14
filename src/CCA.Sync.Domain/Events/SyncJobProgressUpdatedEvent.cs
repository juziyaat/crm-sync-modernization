using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when a sync job's progress is updated.
/// </summary>
public sealed class SyncJobProgressUpdatedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyncJobProgressUpdatedEvent"/> class.
    /// </summary>
    /// <param name="syncJobId">The ID of the sync job</param>
    /// <param name="processedRecords">The number of records processed so far</param>
    /// <param name="totalRecords">The total number of records</param>
    /// <param name="progressPercentage">The progress percentage</param>
    public SyncJobProgressUpdatedEvent(Guid syncJobId, int processedRecords, int totalRecords, double progressPercentage)
    {
        SyncJobId = syncJobId;
        ProcessedRecords = processedRecords;
        TotalRecords = totalRecords;
        ProgressPercentage = progressPercentage;
    }

    /// <summary>
    /// Gets the ID of the sync job.
    /// </summary>
    public Guid SyncJobId { get; }

    /// <summary>
    /// Gets the number of records processed so far.
    /// </summary>
    public int ProcessedRecords { get; }

    /// <summary>
    /// Gets the total number of records.
    /// </summary>
    public int TotalRecords { get; }

    /// <summary>
    /// Gets the progress percentage.
    /// </summary>
    public double ProgressPercentage { get; }
}
