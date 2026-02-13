using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when a utility account is removed from a customer.
/// </summary>
public sealed class UtilityAccountRemovedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UtilityAccountRemovedEvent"/> class.
    /// </summary>
    /// <param name="customerId">The ID of the customer</param>
    /// <param name="accountId">The ID of the removed utility account</param>
    public UtilityAccountRemovedEvent(Guid customerId, Guid accountId)
    {
        CustomerId = customerId;
        AccountId = accountId;
    }

    /// <summary>
    /// Gets the ID of the customer.
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets the ID of the removed utility account.
    /// </summary>
    public Guid AccountId { get; }
}
