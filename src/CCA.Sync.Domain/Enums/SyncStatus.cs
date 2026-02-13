namespace CCA.Sync.Domain.Enums;

/// <summary>
/// Represents the synchronization status of a customer or utility account.
/// </summary>
public enum SyncStatus
{
    /// <summary>
    /// The customer/account has not been synced yet.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// The customer/account is currently being synced.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The customer/account has been successfully synced.
    /// </summary>
    Synced = 2,

    /// <summary>
    /// The sync operation failed for the customer/account.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// The customer/account sync was partially successful.
    /// </summary>
    PartiallySuccessful = 4
}
