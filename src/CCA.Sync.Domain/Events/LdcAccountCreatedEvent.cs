using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.Enums;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when a new LDC account is created.
/// </summary>
public sealed class LdcAccountCreatedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LdcAccountCreatedEvent"/> class.
    /// </summary>
    /// <param name="ldcAccountId">The ID of the created LDC account</param>
    /// <param name="tenantId">The ID of the tenant</param>
    /// <param name="provider">The LDC provider</param>
    /// <param name="accountName">The account name</param>
    public LdcAccountCreatedEvent(Guid ldcAccountId, Guid tenantId, LdcProvider provider, string accountName)
    {
        LdcAccountId = ldcAccountId;
        TenantId = tenantId;
        Provider = provider;
        AccountName = accountName;
    }

    /// <summary>
    /// Gets the ID of the created LDC account.
    /// </summary>
    public Guid LdcAccountId { get; }

    /// <summary>
    /// Gets the ID of the tenant.
    /// </summary>
    public Guid TenantId { get; }

    /// <summary>
    /// Gets the LDC provider.
    /// </summary>
    public LdcProvider Provider { get; }

    /// <summary>
    /// Gets the account name.
    /// </summary>
    public string AccountName { get; }
}
