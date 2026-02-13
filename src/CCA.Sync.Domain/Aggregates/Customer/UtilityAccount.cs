using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.Enums;
using CCA.Sync.Domain.ValueObjects;

namespace CCA.Sync.Domain.Aggregates.Customer;

/// <summary>
/// Represents a utility account owned by a customer.
/// </summary>
public sealed class UtilityAccount : Entity<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UtilityAccount"/> class.
    /// </summary>
    private UtilityAccount()
    {
    }

    /// <summary>
    /// Gets the account number.
    /// </summary>
    public AccountNumber AccountNumber { get; private set; } = null!;

    /// <summary>
    /// Gets the utility provider.
    /// </summary>
    public UtilityProvider Provider { get; private set; }

    /// <summary>
    /// Gets the meter number.
    /// </summary>
    public MeterNumber? MeterNumber { get; private set; }

    /// <summary>
    /// Gets the service address for this account.
    /// </summary>
    public Address? ServiceAddress { get; private set; }

    /// <summary>
    /// Gets the current sync status of this account.
    /// </summary>
    public SyncStatus SyncStatus { get; private set; } = SyncStatus.Pending;

    /// <summary>
    /// Gets the timestamp of the last successful sync.
    /// </summary>
    public DateTime? LastSyncedAt { get; private set; }

    /// <summary>
    /// Gets the timestamp when this account was added to the customer.
    /// </summary>
    public DateTime AddedAt { get; private set; }

    /// <summary>
    /// Creates a new utility account.
    /// </summary>
    /// <param name="accountNumber">The account number</param>
    /// <param name="provider">The utility provider</param>
    /// <param name="meterNumber">The meter number (optional)</param>
    /// <param name="serviceAddress">The service address (optional)</param>
    /// <returns>A result containing the utility account or an error</returns>
    public static Result<UtilityAccount> Create(
        AccountNumber accountNumber,
        UtilityProvider provider,
        MeterNumber? meterNumber = null,
        Address? serviceAddress = null)
    {
        ArgumentNullException.ThrowIfNull(accountNumber);

        var account = new UtilityAccount
        {
            Id = Guid.NewGuid(),
            AccountNumber = accountNumber,
            Provider = provider,
            MeterNumber = meterNumber,
            ServiceAddress = serviceAddress,
            AddedAt = DateTime.UtcNow,
            SyncStatus = SyncStatus.Pending
        };

        return Result<UtilityAccount>.Success(account);
    }

    /// <summary>
    /// Updates the meter number for this account.
    /// </summary>
    /// <param name="meterNumber">The new meter number</param>
    public void UpdateMeterNumber(MeterNumber? meterNumber)
    {
        MeterNumber = meterNumber;
    }

    /// <summary>
    /// Updates the service address for this account.
    /// </summary>
    /// <param name="serviceAddress">The new service address</param>
    public void UpdateServiceAddress(Address? serviceAddress)
    {
        ServiceAddress = serviceAddress;
    }

    /// <summary>
    /// Marks the account as successfully synced.
    /// </summary>
    public void MarkAsSynced()
    {
        SyncStatus = SyncStatus.Synced;
        LastSyncedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the account sync as failed.
    /// </summary>
    public void MarkSyncAsFailed()
    {
        SyncStatus = SyncStatus.Failed;
    }

    /// <summary>
    /// Marks the account sync as in progress.
    /// </summary>
    public void MarkSyncAsInProgress()
    {
        SyncStatus = SyncStatus.InProgress;
    }
}
