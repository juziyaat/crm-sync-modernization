namespace CCA.Sync.Domain.Enums;

/// <summary>
/// Represents the type of synchronization job being executed.
/// </summary>
public enum SyncJobType
{
    /// <summary>
    /// A full synchronization of all data between systems.
    /// </summary>
    FullSync = 0,

    /// <summary>
    /// An incremental synchronization of only changed data since the last sync.
    /// </summary>
    IncrementalSync = 1,

    /// <summary>
    /// Synchronization of a specific LDC account's data.
    /// </summary>
    AccountSync = 2,

    /// <summary>
    /// Synchronization of customer data between systems.
    /// </summary>
    CustomerSync = 3
}
