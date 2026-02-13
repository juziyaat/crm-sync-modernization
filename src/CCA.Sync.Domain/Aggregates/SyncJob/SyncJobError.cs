using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.Aggregates.SyncJob;

/// <summary>
/// Represents an error that occurred during a sync job's execution.
/// This is an entity owned by the SyncJob aggregate.
/// </summary>
public sealed class SyncJobError : Entity<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyncJobError"/> class.
    /// </summary>
    private SyncJobError()
    {
    }

    /// <summary>
    /// Gets the error code categorizing the type of error.
    /// </summary>
    public string ErrorCode { get; private set; } = null!;

    /// <summary>
    /// Gets the detailed error message.
    /// </summary>
    public string ErrorMessage { get; private set; } = null!;

    /// <summary>
    /// Gets the identifier of the record that caused the error (if applicable).
    /// </summary>
    public string? RecordIdentifier { get; private set; }

    /// <summary>
    /// Gets the timestamp when the error occurred.
    /// </summary>
    public DateTime OccurredAt { get; private set; }

    /// <summary>
    /// Creates a new SyncJobError instance.
    /// </summary>
    /// <param name="errorCode">The error code categorizing the type of error</param>
    /// <param name="errorMessage">The detailed error message</param>
    /// <param name="recordIdentifier">The identifier of the record that caused the error (optional)</param>
    /// <returns>A result containing the sync job error or an error</returns>
    public static Result<SyncJobError> Create(string errorCode, string errorMessage, string? recordIdentifier = null)
    {
        if (string.IsNullOrWhiteSpace(errorCode))
        {
            return Result<SyncJobError>.Failure(
                new Error("SyncJobError.InvalidErrorCode", "Error code cannot be empty."));
        }

        if (errorCode.Length > 100)
        {
            return Result<SyncJobError>.Failure(
                new Error("SyncJobError.InvalidErrorCode", "Error code cannot exceed 100 characters."));
        }

        if (string.IsNullOrWhiteSpace(errorMessage))
        {
            return Result<SyncJobError>.Failure(
                new Error("SyncJobError.InvalidErrorMessage", "Error message cannot be empty."));
        }

        if (errorMessage.Length > 2000)
        {
            return Result<SyncJobError>.Failure(
                new Error("SyncJobError.InvalidErrorMessage", "Error message cannot exceed 2000 characters."));
        }

        if (recordIdentifier is not null && recordIdentifier.Length > 500)
        {
            return Result<SyncJobError>.Failure(
                new Error("SyncJobError.InvalidRecordIdentifier", "Record identifier cannot exceed 500 characters."));
        }

        var syncJobError = new SyncJobError
        {
            Id = Guid.NewGuid(),
            ErrorCode = errorCode.Trim(),
            ErrorMessage = errorMessage.Trim(),
            RecordIdentifier = recordIdentifier?.Trim(),
            OccurredAt = DateTime.UtcNow
        };

        return Result<SyncJobError>.Success(syncJobError);
    }
}
