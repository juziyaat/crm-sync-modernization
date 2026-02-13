using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when an LDC account's credentials are updated.
/// </summary>
public sealed class LdcAccountCredentialsUpdatedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LdcAccountCredentialsUpdatedEvent"/> class.
    /// </summary>
    /// <param name="ldcAccountId">The ID of the LDC account</param>
    /// <param name="username">The new username</param>
    public LdcAccountCredentialsUpdatedEvent(Guid ldcAccountId, string username)
    {
        LdcAccountId = ldcAccountId;
        Username = username;
    }

    /// <summary>
    /// Gets the ID of the LDC account.
    /// </summary>
    public Guid LdcAccountId { get; }

    /// <summary>
    /// Gets the new username.
    /// Note: Password is intentionally NOT included for security reasons.
    /// </summary>
    public string Username { get; }
}
