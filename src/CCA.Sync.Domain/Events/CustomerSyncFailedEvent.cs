using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when a customer sync operation fails.
/// </summary>
public sealed class CustomerSyncFailedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerSyncFailedEvent"/> class.
    /// </summary>
    /// <param name="customerId">The ID of the customer</param>
    /// <param name="failedAt">The timestamp when the sync failed</param>
    /// <param name="reason">The reason for the sync failure</param>
    public CustomerSyncFailedEvent(Guid customerId, DateTime failedAt, string reason)
    {
        CustomerId = customerId;
        FailedAt = failedAt;
        Reason = reason;
    }

    /// <summary>
    /// Gets the ID of the customer.
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets the timestamp when the sync failed.
    /// </summary>
    public DateTime FailedAt { get; }

    /// <summary>
    /// Gets the reason for the sync failure.
    /// </summary>
    public string Reason { get; }
}
