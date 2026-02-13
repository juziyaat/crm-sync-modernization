using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.Enums;
using CCA.Sync.Domain.Events;
using CCA.Sync.Domain.ValueObjects;

namespace CCA.Sync.Domain.Aggregates.SyncJob;

/// <summary>
/// Represents a data synchronization job between systems.
/// This is the aggregate root for sync job tracking.
/// </summary>
/// <remarks>
/// State machine transitions:
///   Pending → Running (Start)
///   Pending → Cancelled (Cancel)
///   Running → Completed (Complete)
///   Running → Failed (Fail)
///   Running → Cancelled (Cancel)
///   Running → PartiallyCompleted (CompletePartially)
/// </remarks>
public sealed class SyncJob : AggregateRoot<Guid>
{
    private readonly List<SyncJobError> _errors = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="SyncJob"/> class.
    /// </summary>
    private SyncJob()
    {
    }

    /// <summary>
    /// Gets the tenant identifier.
    /// </summary>
    public TenantId TenantId { get; private set; } = null!;

    /// <summary>
    /// Gets the type of sync job.
    /// </summary>
    public SyncJobType JobType { get; private set; }

    /// <summary>
    /// Gets the current status of the sync job.
    /// </summary>
    public SyncJobStatus Status { get; private set; } = SyncJobStatus.Pending;

    /// <summary>
    /// Gets the optional LDC account ID associated with this sync job.
    /// </summary>
    public Guid? LdcAccountId { get; private set; }

    /// <summary>
    /// Gets the optional correlation ID for linking related jobs.
    /// </summary>
    public Guid? CorrelationId { get; private set; }

    /// <summary>
    /// Gets the sync job statistics.
    /// </summary>
    public SyncJobStatistics Statistics { get; private set; } = null!;

    /// <summary>
    /// Gets the errors that occurred during the sync job.
    /// </summary>
    public IReadOnlyCollection<SyncJobError> Errors => _errors.AsReadOnly();

    /// <summary>
    /// Gets the timestamp when the job was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the timestamp when the job started executing.
    /// </summary>
    public DateTime? StartedAt { get; private set; }

    /// <summary>
    /// Gets the timestamp when the job completed (successfully, partially, or with failure).
    /// </summary>
    public DateTime? CompletedAt { get; private set; }

    /// <summary>
    /// Creates a new sync job.
    /// </summary>
    /// <param name="tenantId">The tenant identifier</param>
    /// <param name="jobType">The type of sync job</param>
    /// <param name="ldcAccountId">The optional LDC account ID associated with the job</param>
    /// <param name="correlationId">The optional correlation ID for linking related jobs</param>
    /// <returns>A result containing the sync job or an error</returns>
    public static Result<SyncJob> Create(
        TenantId tenantId,
        SyncJobType jobType,
        Guid? ldcAccountId = null,
        Guid? correlationId = null)
    {
        ArgumentNullException.ThrowIfNull(tenantId);

        if (ldcAccountId.HasValue && ldcAccountId.Value == Guid.Empty)
        {
            return Result<SyncJob>.Failure(
                new Error("SyncJob.InvalidLdcAccountId", "LDC account ID cannot be an empty GUID."));
        }

        if (correlationId.HasValue && correlationId.Value == Guid.Empty)
        {
            return Result<SyncJob>.Failure(
                new Error("SyncJob.InvalidCorrelationId", "Correlation ID cannot be an empty GUID."));
        }

        var job = new SyncJob
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            JobType = jobType,
            Status = SyncJobStatus.Pending,
            LdcAccountId = ldcAccountId,
            CorrelationId = correlationId,
            Statistics = SyncJobStatistics.CreateEmpty(),
            CreatedAt = DateTime.UtcNow
        };

        job.RaiseDomainEvent(new SyncJobCreatedEvent(
            job.Id,
            tenantId.Value,
            jobType,
            ldcAccountId));

        return Result<SyncJob>.Success(job);
    }

    /// <summary>
    /// Starts the sync job execution.
    /// </summary>
    /// <param name="totalRecords">The total number of records to be processed</param>
    /// <returns>A result indicating success or failure</returns>
    public Result Start(int totalRecords)
    {
        if (Status != SyncJobStatus.Pending)
        {
            return Result.Failure(
                new Error("SyncJob.InvalidStateTransition", $"Cannot start a job that is in '{Status}' status. Job must be in 'Pending' status."));
        }

        if (totalRecords < 0)
        {
            return Result.Failure(
                new Error("SyncJob.InvalidTotalRecords", "Total records cannot be negative."));
        }

        var statsResult = SyncJobStatistics.CreateInitial(totalRecords);
        if (statsResult.IsFailure)
        {
            return Result.Failure(statsResult.Error);
        }

        Status = SyncJobStatus.Running;
        StartedAt = DateTime.UtcNow;
        Statistics = statsResult.Value;

        RaiseDomainEvent(new SyncJobStartedEvent(Id, StartedAt.Value));

        return Result.Success();
    }

    /// <summary>
    /// Updates the sync job progress with new statistics.
    /// </summary>
    /// <param name="statistics">The updated statistics</param>
    /// <returns>A result indicating success or failure</returns>
    public Result UpdateProgress(SyncJobStatistics statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);

        if (Status != SyncJobStatus.Running)
        {
            return Result.Failure(
                new Error("SyncJob.InvalidStateTransition", $"Cannot update progress of a job that is in '{Status}' status. Job must be in 'Running' status."));
        }

        Statistics = statistics;

        RaiseDomainEvent(new SyncJobProgressUpdatedEvent(
            Id,
            statistics.ProcessedRecords,
            statistics.TotalRecords,
            statistics.ProgressPercentage));

        return Result.Success();
    }

    /// <summary>
    /// Records an error that occurred during sync job execution.
    /// </summary>
    /// <param name="errorCode">The error code categorizing the type of error</param>
    /// <param name="errorMessage">The detailed error message</param>
    /// <param name="recordIdentifier">The identifier of the record that caused the error (optional)</param>
    /// <returns>A result indicating success or failure</returns>
    public Result RecordError(string errorCode, string errorMessage, string? recordIdentifier = null)
    {
        if (Status != SyncJobStatus.Running)
        {
            return Result.Failure(
                new Error("SyncJob.InvalidStateTransition", $"Cannot record errors for a job that is in '{Status}' status. Job must be in 'Running' status."));
        }

        var errorResult = SyncJobError.Create(errorCode, errorMessage, recordIdentifier);
        if (errorResult.IsFailure)
        {
            return Result.Failure(errorResult.Error);
        }

        _errors.Add(errorResult.Value);

        return Result.Success();
    }

    /// <summary>
    /// Completes the sync job successfully.
    /// </summary>
    /// <param name="finalStatistics">The final statistics for the completed job</param>
    /// <returns>A result indicating success or failure</returns>
    public Result Complete(SyncJobStatistics finalStatistics)
    {
        ArgumentNullException.ThrowIfNull(finalStatistics);

        if (Status != SyncJobStatus.Running)
        {
            return Result.Failure(
                new Error("SyncJob.InvalidStateTransition", $"Cannot complete a job that is in '{Status}' status. Job must be in 'Running' status."));
        }

        Status = SyncJobStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        Statistics = finalStatistics;

        RaiseDomainEvent(new SyncJobCompletedEvent(
            Id,
            CompletedAt.Value,
            finalStatistics.TotalRecords,
            finalStatistics.SuccessfulRecords));

        return Result.Success();
    }

    /// <summary>
    /// Marks the sync job as failed.
    /// </summary>
    /// <param name="reason">The reason for the failure</param>
    /// <returns>A result indicating success or failure</returns>
    public Result Fail(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return Result.Failure(
                new Error("SyncJob.InvalidFailureReason", "Failure reason cannot be empty."));
        }

        if (Status != SyncJobStatus.Running)
        {
            return Result.Failure(
                new Error("SyncJob.InvalidStateTransition", $"Cannot fail a job that is in '{Status}' status. Job must be in 'Running' status."));
        }

        Status = SyncJobStatus.Failed;
        CompletedAt = DateTime.UtcNow;

        RaiseDomainEvent(new SyncJobFailedEvent(Id, CompletedAt.Value, reason));

        return Result.Success();
    }

    /// <summary>
    /// Cancels the sync job.
    /// </summary>
    /// <param name="reason">The reason for cancellation</param>
    /// <returns>A result indicating success or failure</returns>
    public Result Cancel(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return Result.Failure(
                new Error("SyncJob.InvalidCancellationReason", "Cancellation reason cannot be empty."));
        }

        if (Status != SyncJobStatus.Pending && Status != SyncJobStatus.Running)
        {
            return Result.Failure(
                new Error("SyncJob.InvalidStateTransition", $"Cannot cancel a job that is in '{Status}' status. Job must be in 'Pending' or 'Running' status."));
        }

        Status = SyncJobStatus.Cancelled;
        CompletedAt = DateTime.UtcNow;

        RaiseDomainEvent(new SyncJobCancelledEvent(Id, CompletedAt.Value, reason));

        return Result.Success();
    }

    /// <summary>
    /// Completes the sync job with partial success (some records failed).
    /// </summary>
    /// <param name="finalStatistics">The final statistics for the partially completed job</param>
    /// <returns>A result indicating success or failure</returns>
    public Result CompletePartially(SyncJobStatistics finalStatistics)
    {
        ArgumentNullException.ThrowIfNull(finalStatistics);

        if (Status != SyncJobStatus.Running)
        {
            return Result.Failure(
                new Error("SyncJob.InvalidStateTransition", $"Cannot partially complete a job that is in '{Status}' status. Job must be in 'Running' status."));
        }

        if (finalStatistics.FailedRecords == 0)
        {
            return Result.Failure(
                new Error("SyncJob.NoFailedRecords", "A partially completed job must have at least one failed record. Use Complete() if all records succeeded."));
        }

        Status = SyncJobStatus.PartiallyCompleted;
        CompletedAt = DateTime.UtcNow;
        Statistics = finalStatistics;

        RaiseDomainEvent(new SyncJobCompletedEvent(
            Id,
            CompletedAt.Value,
            finalStatistics.TotalRecords,
            finalStatistics.SuccessfulRecords));

        return Result.Success();
    }

    /// <summary>
    /// Gets the duration of the sync job execution.
    /// </summary>
    /// <returns>The duration if the job has started; otherwise null</returns>
    public TimeSpan? GetDuration()
    {
        if (!StartedAt.HasValue)
        {
            return null;
        }

        var endTime = CompletedAt ?? DateTime.UtcNow;
        return endTime - StartedAt.Value;
    }

    /// <summary>
    /// Determines whether the sync job is in a terminal state.
    /// </summary>
    /// <returns>True if the job is completed, failed, cancelled, or partially completed; otherwise false</returns>
    public bool IsTerminal()
    {
        return Status is SyncJobStatus.Completed
            or SyncJobStatus.Failed
            or SyncJobStatus.Cancelled
            or SyncJobStatus.PartiallyCompleted;
    }

    /// <summary>
    /// Determines whether the sync job has errors.
    /// </summary>
    /// <returns>True if the job has recorded errors; otherwise false</returns>
    public bool HasErrors()
    {
        return _errors.Count > 0;
    }
}
