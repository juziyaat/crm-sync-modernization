using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.Aggregates.SyncJob;

/// <summary>
/// Represents the statistics of a sync job's execution, tracking record processing progress.
/// </summary>
public sealed class SyncJobStatistics : ValueObject, IEquatable<SyncJobStatistics>
{
    private SyncJobStatistics(int totalRecords, int processedRecords, int successfulRecords, int failedRecords, int skippedRecords)
    {
        TotalRecords = totalRecords;
        ProcessedRecords = processedRecords;
        SuccessfulRecords = successfulRecords;
        FailedRecords = failedRecords;
        SkippedRecords = skippedRecords;
    }

    /// <summary>
    /// Gets the total number of records to be processed.
    /// </summary>
    public int TotalRecords { get; }

    /// <summary>
    /// Gets the number of records that have been processed (successful + failed + skipped).
    /// </summary>
    public int ProcessedRecords { get; }

    /// <summary>
    /// Gets the number of records that were successfully synced.
    /// </summary>
    public int SuccessfulRecords { get; }

    /// <summary>
    /// Gets the number of records that failed to sync.
    /// </summary>
    public int FailedRecords { get; }

    /// <summary>
    /// Gets the number of records that were skipped during sync.
    /// </summary>
    public int SkippedRecords { get; }

    /// <summary>
    /// Gets the progress percentage of the sync job (0-100).
    /// </summary>
    public double ProgressPercentage => TotalRecords > 0
        ? Math.Round((double)ProcessedRecords / TotalRecords * 100, 2)
        : 0;

    /// <summary>
    /// Creates a new SyncJobStatistics instance.
    /// </summary>
    /// <param name="totalRecords">The total number of records to be processed</param>
    /// <param name="processedRecords">The number of records that have been processed</param>
    /// <param name="successfulRecords">The number of records successfully synced</param>
    /// <param name="failedRecords">The number of records that failed to sync</param>
    /// <param name="skippedRecords">The number of records that were skipped</param>
    /// <returns>A result containing the statistics or an error</returns>
    public static Result<SyncJobStatistics> Create(
        int totalRecords,
        int processedRecords,
        int successfulRecords,
        int failedRecords,
        int skippedRecords)
    {
        if (totalRecords < 0)
        {
            return Result<SyncJobStatistics>.Failure(
                new Error("SyncJobStatistics.InvalidTotalRecords", "Total records cannot be negative."));
        }

        if (processedRecords < 0)
        {
            return Result<SyncJobStatistics>.Failure(
                new Error("SyncJobStatistics.InvalidProcessedRecords", "Processed records cannot be negative."));
        }

        if (successfulRecords < 0)
        {
            return Result<SyncJobStatistics>.Failure(
                new Error("SyncJobStatistics.InvalidSuccessfulRecords", "Successful records cannot be negative."));
        }

        if (failedRecords < 0)
        {
            return Result<SyncJobStatistics>.Failure(
                new Error("SyncJobStatistics.InvalidFailedRecords", "Failed records cannot be negative."));
        }

        if (skippedRecords < 0)
        {
            return Result<SyncJobStatistics>.Failure(
                new Error("SyncJobStatistics.InvalidSkippedRecords", "Skipped records cannot be negative."));
        }

        if (processedRecords > totalRecords)
        {
            return Result<SyncJobStatistics>.Failure(
                new Error("SyncJobStatistics.ProcessedExceedsTotal", "Processed records cannot exceed total records."));
        }

        var sumOfDetails = successfulRecords + failedRecords + skippedRecords;
        if (sumOfDetails > processedRecords)
        {
            return Result<SyncJobStatistics>.Failure(
                new Error("SyncJobStatistics.DetailExceedsProcessed", "Sum of successful, failed, and skipped records cannot exceed processed records."));
        }

        return Result<SyncJobStatistics>.Success(
            new SyncJobStatistics(totalRecords, processedRecords, successfulRecords, failedRecords, skippedRecords));
    }

    /// <summary>
    /// Creates an initial statistics instance with the specified total records.
    /// </summary>
    /// <param name="totalRecords">The total number of records to be processed</param>
    /// <returns>A result containing the initial statistics or an error</returns>
    public static Result<SyncJobStatistics> CreateInitial(int totalRecords)
    {
        return Create(totalRecords, 0, 0, 0, 0);
    }

    /// <summary>
    /// Creates an empty statistics instance (zero total records).
    /// </summary>
    /// <returns>An empty statistics instance</returns>
    public static SyncJobStatistics CreateEmpty()
    {
        return new SyncJobStatistics(0, 0, 0, 0, 0);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return TotalRecords;
        yield return ProcessedRecords;
        yield return SuccessfulRecords;
        yield return FailedRecords;
        yield return SkippedRecords;
    }

    /// <inheritdoc/>
    public bool Equals(SyncJobStatistics? other)
    {
        return other is not null &&
               TotalRecords == other.TotalRecords &&
               ProcessedRecords == other.ProcessedRecords &&
               SuccessfulRecords == other.SuccessfulRecords &&
               FailedRecords == other.FailedRecords &&
               SkippedRecords == other.SkippedRecords;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Total: {TotalRecords}, Processed: {ProcessedRecords}, Successful: {SuccessfulRecords}, Failed: {FailedRecords}, Skipped: {SkippedRecords} ({ProgressPercentage}%)";
    }
}
