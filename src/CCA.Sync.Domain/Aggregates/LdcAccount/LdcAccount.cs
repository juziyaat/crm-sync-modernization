using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.Enums;
using CCA.Sync.Domain.Events;
using CCA.Sync.Domain.ValueObjects;

namespace CCA.Sync.Domain.Aggregates.LdcAccount;

/// <summary>
/// Represents a CCA's account with a Local Distribution Company (LDC).
/// This is the aggregate root for LDC account management.
/// </summary>
public sealed class LdcAccount : AggregateRoot<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LdcAccount"/> class.
    /// </summary>
    private LdcAccount()
    {
    }

    /// <summary>
    /// Gets the tenant identifier.
    /// </summary>
    public TenantId TenantId { get; private set; } = null!;

    /// <summary>
    /// Gets the LDC provider.
    /// </summary>
    public LdcProvider Provider { get; private set; }

    /// <summary>
    /// Gets the account name/description.
    /// </summary>
    public string AccountName { get; private set; } = null!;

    /// <summary>
    /// Gets the username for LDC portal access.
    /// This value should be encrypted at rest.
    /// </summary>
    public string Username { get; private set; } = null!;

    /// <summary>
    /// Gets the password for LDC portal access.
    /// IMPORTANT: This value MUST be encrypted at rest.
    /// </summary>
    public string EncryptedPassword { get; private set; } = null!;

    /// <summary>
    /// Gets the synchronization configuration.
    /// </summary>
    public SyncConfiguration SyncConfiguration { get; private set; } = null!;

    /// <summary>
    /// Gets the current sync status of the LDC account.
    /// </summary>
    public SyncStatus SyncStatus { get; private set; } = SyncStatus.Pending;

    /// <summary>
    /// Gets the timestamp of the last successful sync.
    /// </summary>
    public DateTime? LastSyncedAt { get; private set; }

    /// <summary>
    /// Gets the timestamp when the account was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the timestamp when the account was last modified.
    /// </summary>
    public DateTime? ModifiedAt { get; private set; }

    /// <summary>
    /// Creates a new LDC account.
    /// </summary>
    /// <param name="tenantId">The tenant identifier</param>
    /// <param name="provider">The LDC provider</param>
    /// <param name="accountName">The account name/description</param>
    /// <param name="username">The username for LDC portal access</param>
    /// <param name="encryptedPassword">The encrypted password for LDC portal access</param>
    /// <param name="syncConfiguration">The synchronization configuration (optional, defaults to default configuration)</param>
    /// <returns>A result containing the LDC account or an error</returns>
    public static Result<LdcAccount> Create(
        TenantId tenantId,
        LdcProvider provider,
        string accountName,
        string username,
        string encryptedPassword,
        SyncConfiguration? syncConfiguration = null)
    {
        ArgumentNullException.ThrowIfNull(tenantId);

        if (string.IsNullOrWhiteSpace(accountName))
        {
            return Result<LdcAccount>.Failure(
                new Error("LdcAccount.InvalidAccountName", "Account name cannot be empty."));
        }

        if (accountName.Length > 200)
        {
            return Result<LdcAccount>.Failure(
                new Error("LdcAccount.InvalidAccountName", "Account name cannot exceed 200 characters."));
        }

        if (string.IsNullOrWhiteSpace(username))
        {
            return Result<LdcAccount>.Failure(
                new Error("LdcAccount.InvalidUsername", "Username cannot be empty."));
        }

        if (username.Length > 100)
        {
            return Result<LdcAccount>.Failure(
                new Error("LdcAccount.InvalidUsername", "Username cannot exceed 100 characters."));
        }

        if (string.IsNullOrWhiteSpace(encryptedPassword))
        {
            return Result<LdcAccount>.Failure(
                new Error("LdcAccount.InvalidPassword", "Password cannot be empty."));
        }

        var account = new LdcAccount
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Provider = provider,
            AccountName = accountName.Trim(),
            Username = username.Trim(),
            EncryptedPassword = encryptedPassword,
            SyncConfiguration = syncConfiguration ?? SyncConfiguration.CreateDefault(),
            CreatedAt = DateTime.UtcNow,
            SyncStatus = SyncStatus.Pending
        };

        account.RaiseDomainEvent(new LdcAccountCreatedEvent(
            account.Id,
            tenantId.Value,
            provider,
            accountName.Trim()));

        return Result<LdcAccount>.Success(account);
    }

    /// <summary>
    /// Updates the LDC account credentials.
    /// </summary>
    /// <param name="username">The new username</param>
    /// <param name="encryptedPassword">The new encrypted password</param>
    /// <returns>A result indicating success or failure</returns>
    public Result UpdateCredentials(string username, string encryptedPassword)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Result.Failure(
                new Error("LdcAccount.InvalidUsername", "Username cannot be empty."));
        }

        if (username.Length > 100)
        {
            return Result.Failure(
                new Error("LdcAccount.InvalidUsername", "Username cannot exceed 100 characters."));
        }

        if (string.IsNullOrWhiteSpace(encryptedPassword))
        {
            return Result.Failure(
                new Error("LdcAccount.InvalidPassword", "Password cannot be empty."));
        }

        Username = username.Trim();
        EncryptedPassword = encryptedPassword;
        ModifiedAt = DateTime.UtcNow;

        RaiseDomainEvent(new LdcAccountCredentialsUpdatedEvent(Id, Username));

        return Result.Success();
    }

    /// <summary>
    /// Enables synchronization for the LDC account.
    /// </summary>
    /// <returns>A result indicating success or failure</returns>
    public Result EnableSync()
    {
        if (SyncConfiguration.IsEnabled)
        {
            return Result.Failure(
                new Error("LdcAccount.SyncAlreadyEnabled", "Synchronization is already enabled."));
        }

        var result = SyncConfiguration.Create(
            true,
            SyncConfiguration.SyncIntervalMinutes,
            SyncConfiguration.MaxRetries,
            SyncConfiguration.TimeoutSeconds);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        SyncConfiguration = result.Value;
        ModifiedAt = DateTime.UtcNow;

        RaiseDomainEvent(new LdcAccountSyncEnabledEvent(Id, Provider));

        return Result.Success();
    }

    /// <summary>
    /// Disables synchronization for the LDC account.
    /// </summary>
    /// <returns>A result indicating success or failure</returns>
    public Result DisableSync()
    {
        if (!SyncConfiguration.IsEnabled)
        {
            return Result.Failure(
                new Error("LdcAccount.SyncAlreadyDisabled", "Synchronization is already disabled."));
        }

        var result = SyncConfiguration.Create(
            false,
            SyncConfiguration.SyncIntervalMinutes,
            SyncConfiguration.MaxRetries,
            SyncConfiguration.TimeoutSeconds);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        SyncConfiguration = result.Value;
        ModifiedAt = DateTime.UtcNow;

        RaiseDomainEvent(new LdcAccountSyncDisabledEvent(Id, Provider));

        return Result.Success();
    }

    /// <summary>
    /// Updates the synchronization configuration.
    /// </summary>
    /// <param name="syncConfiguration">The new synchronization configuration</param>
    /// <returns>A result indicating success or failure</returns>
    public Result UpdateSyncConfiguration(SyncConfiguration syncConfiguration)
    {
        ArgumentNullException.ThrowIfNull(syncConfiguration);

        var wasEnabled = SyncConfiguration.IsEnabled;
        SyncConfiguration = syncConfiguration;
        ModifiedAt = DateTime.UtcNow;

        // Raise appropriate events based on state change
        if (!wasEnabled && syncConfiguration.IsEnabled)
        {
            RaiseDomainEvent(new LdcAccountSyncEnabledEvent(Id, Provider));
        }
        else if (wasEnabled && !syncConfiguration.IsEnabled)
        {
            RaiseDomainEvent(new LdcAccountSyncDisabledEvent(Id, Provider));
        }

        return Result.Success();
    }

    /// <summary>
    /// Marks the LDC account as synced.
    /// </summary>
    public void MarkAsSynced()
    {
        SyncStatus = SyncStatus.Synced;
        LastSyncedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the LDC account sync as failed.
    /// </summary>
    public void MarkSyncAsFailed()
    {
        SyncStatus = SyncStatus.Failed;
    }

    /// <summary>
    /// Marks the LDC account sync as in progress.
    /// </summary>
    public void MarkSyncAsInProgress()
    {
        SyncStatus = SyncStatus.InProgress;
    }

    /// <summary>
    /// Determines whether the account is ready for synchronization.
    /// </summary>
    /// <returns>True if the account is ready to sync; otherwise false</returns>
    public bool IsReadyForSync()
    {
        return SyncConfiguration.IsEnabled
               && !string.IsNullOrWhiteSpace(Username)
               && !string.IsNullOrWhiteSpace(EncryptedPassword);
    }
}
