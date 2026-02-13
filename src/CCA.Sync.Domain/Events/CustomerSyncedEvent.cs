using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when a customer has been successfully synced.
/// </summary>
public sealed class CustomerSyncedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerSyncedEvent"/> class.
    /// </summary>
    /// <param name="customerId">The ID of the synced customer</param>
    /// <param name="syncedAt">The timestamp when the sync occurred</param>
    /// <param name="accountCount">The number of utility accounts synced</param>
    public CustomerSyncedEvent(Guid customerId, DateTime syncedAt, int accountCount)
    {
        CustomerId = customerId;
        SyncedAt = syncedAt;
        AccountCount = accountCount;
    }

    /// <summary>
    /// Gets the ID of the synced customer.
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets the timestamp when the sync occurred.
    /// </summary>
    public DateTime SyncedAt { get; }

    /// <summary>
    /// Gets the number of utility accounts synced.
    /// </summary>
    public int AccountCount { get; }
}
