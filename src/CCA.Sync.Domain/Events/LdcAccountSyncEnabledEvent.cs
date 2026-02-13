using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.Enums;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when synchronization is enabled for an LDC account.
/// </summary>
public sealed class LdcAccountSyncEnabledEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LdcAccountSyncEnabledEvent"/> class.
    /// </summary>
    /// <param name="ldcAccountId">The ID of the LDC account</param>
    /// <param name="provider">The LDC provider</param>
    public LdcAccountSyncEnabledEvent(Guid ldcAccountId, LdcProvider provider)
    {
        LdcAccountId = ldcAccountId;
        Provider = provider;
    }

    /// <summary>
    /// Gets the ID of the LDC account.
    /// </summary>
    public Guid LdcAccountId { get; }

    /// <summary>
    /// Gets the LDC provider.
    /// </summary>
    public LdcProvider Provider { get; }
}
