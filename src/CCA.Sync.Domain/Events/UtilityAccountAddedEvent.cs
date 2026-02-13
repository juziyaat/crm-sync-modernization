using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.Enums;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when a utility account is added to a customer.
/// </summary>
public sealed class UtilityAccountAddedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UtilityAccountAddedEvent"/> class.
    /// </summary>
    /// <param name="customerId">The ID of the customer</param>
    /// <param name="accountId">The ID of the utility account</param>
    /// <param name="accountNumber">The account number</param>
    /// <param name="provider">The utility provider</param>
    public UtilityAccountAddedEvent(Guid customerId, Guid accountId, string accountNumber, UtilityProvider provider)
    {
        CustomerId = customerId;
        AccountId = accountId;
        AccountNumber = accountNumber;
        Provider = provider;
    }

    /// <summary>
    /// Gets the ID of the customer.
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets the ID of the utility account.
    /// </summary>
    public Guid AccountId { get; }

    /// <summary>
    /// Gets the account number.
    /// </summary>
    public string AccountNumber { get; }

    /// <summary>
    /// Gets the utility provider.
    /// </summary>
    public UtilityProvider Provider { get; }
}
