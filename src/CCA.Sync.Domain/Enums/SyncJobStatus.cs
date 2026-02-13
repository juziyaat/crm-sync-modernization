namespace CCA.Sync.Domain.Enums;

/// <summary>
/// Represents the status of a synchronization job throughout its lifecycle.
/// </summary>
public enum SyncJobStatus
{
    /// <summary>
    /// The job has been created but not yet started.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// The job is currently executing.
    /// </summary>
    Running = 1,

    /// <summary>
    /// The job completed successfully with all records processed.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// The job failed and could not complete processing.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// The job was manually cancelled before completion.
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// The job completed but some records failed while others succeeded.
    /// </summary>
    PartiallyCompleted = 5
}
