using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.Aggregates.LdcAccount;

/// <summary>
/// Represents the synchronization configuration for an LDC account.
/// </summary>
public sealed class SyncConfiguration : ValueObject
{
    /// <summary>
    /// Maximum allowed retry attempts.
    /// </summary>
    public const int MaxRetryAttempts = 10;

    /// <summary>
    /// Minimum sync interval in minutes.
    /// </summary>
    public const int MinSyncIntervalMinutes = 5;

    /// <summary>
    /// Maximum sync interval in minutes (24 hours).
    /// </summary>
    public const int MaxSyncIntervalMinutes = 1440;

    /// <summary>
    /// Initializes a new instance of the <see cref="SyncConfiguration"/> class.
    /// </summary>
    private SyncConfiguration()
    {
    }

    /// <summary>
    /// Gets a value indicating whether synchronization is enabled.
    /// </summary>
    public bool IsEnabled { get; private set; }

    /// <summary>
    /// Gets the synchronization interval in minutes.
    /// </summary>
    public int SyncIntervalMinutes { get; private set; }

    /// <summary>
    /// Gets the maximum number of retry attempts on sync failure.
    /// </summary>
    public int MaxRetries { get; private set; }

    /// <summary>
    /// Gets the timeout for sync operations in seconds.
    /// </summary>
    public int TimeoutSeconds { get; private set; }

    /// <summary>
    /// Creates a new sync configuration.
    /// </summary>
    /// <param name="isEnabled">Whether synchronization is enabled</param>
    /// <param name="syncIntervalMinutes">The synchronization interval in minutes</param>
    /// <param name="maxRetries">The maximum number of retry attempts</param>
    /// <param name="timeoutSeconds">The timeout for sync operations in seconds</param>
    /// <returns>A result containing the sync configuration or an error</returns>
    public static Result<SyncConfiguration> Create(
        bool isEnabled,
        int syncIntervalMinutes,
        int maxRetries,
        int timeoutSeconds)
    {
        if (syncIntervalMinutes < MinSyncIntervalMinutes)
        {
            return Result<SyncConfiguration>.Failure(
                new Error(
                    "SyncConfiguration.InvalidInterval",
                    $"Sync interval must be at least {MinSyncIntervalMinutes} minutes."));
        }

        if (syncIntervalMinutes > MaxSyncIntervalMinutes)
        {
            return Result<SyncConfiguration>.Failure(
                new Error(
                    "SyncConfiguration.InvalidInterval",
                    $"Sync interval cannot exceed {MaxSyncIntervalMinutes} minutes (24 hours)."));
        }

        if (maxRetries < 0)
        {
            return Result<SyncConfiguration>.Failure(
                new Error(
                    "SyncConfiguration.InvalidRetries",
                    "Max retries cannot be negative."));
        }

        if (maxRetries > MaxRetryAttempts)
        {
            return Result<SyncConfiguration>.Failure(
                new Error(
                    "SyncConfiguration.InvalidRetries",
                    $"Max retries cannot exceed {MaxRetryAttempts}."));
        }

        if (timeoutSeconds <= 0)
        {
            return Result<SyncConfiguration>.Failure(
                new Error(
                    "SyncConfiguration.InvalidTimeout",
                    "Timeout must be greater than zero."));
        }

        var configuration = new SyncConfiguration
        {
            IsEnabled = isEnabled,
            SyncIntervalMinutes = syncIntervalMinutes,
            MaxRetries = maxRetries,
            TimeoutSeconds = timeoutSeconds
        };

        return Result<SyncConfiguration>.Success(configuration);
    }

    /// <summary>
    /// Creates a default sync configuration.
    /// </summary>
    /// <returns>A sync configuration with default settings</returns>
    public static SyncConfiguration CreateDefault()
    {
        return new SyncConfiguration
        {
            IsEnabled = true,
            SyncIntervalMinutes = 60, // 1 hour
            MaxRetries = 3,
            TimeoutSeconds = 300 // 5 minutes
        };
    }

    /// <summary>
    /// Creates a sync configuration with sync disabled.
    /// </summary>
    /// <returns>A sync configuration with sync disabled</returns>
    public static SyncConfiguration CreateDisabled()
    {
        return new SyncConfiguration
        {
            IsEnabled = false,
            SyncIntervalMinutes = 60,
            MaxRetries = 3,
            TimeoutSeconds = 300
        };
    }

    /// <summary>
    /// Returns the properties used for equality comparison.
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return IsEnabled;
        yield return SyncIntervalMinutes;
        yield return MaxRetries;
        yield return TimeoutSeconds;
    }
}
