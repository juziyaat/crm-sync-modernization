using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.Enums;
using CCA.Sync.Domain.Events;
using CCA.Sync.Domain.ValueObjects;

namespace CCA.Sync.Domain.Aggregates.Customer;

/// <summary>
/// Represents a customer enrolled with a CCA. This is the aggregate root for customer-related operations.
/// </summary>
public sealed class Customer : AggregateRoot<Guid>
{
    private readonly List<UtilityAccount> _utilityAccounts = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="Customer"/> class.
    /// </summary>
    private Customer()
    {
    }

    /// <summary>
    /// Gets the tenant identifier.
    /// </summary>
    public TenantId TenantId { get; private set; } = null!;

    /// <summary>
    /// Gets the customer's name.
    /// </summary>
    public CustomerName Name { get; private set; } = null!;

    /// <summary>
    /// Gets the customer's email address.
    /// </summary>
    public EmailAddress EmailAddress { get; private set; } = null!;

    /// <summary>
    /// Gets the customer's phone number (optional).
    /// </summary>
    public PhoneNumber? PhoneNumber { get; private set; }

    /// <summary>
    /// Gets the customer's service address (optional).
    /// </summary>
    public Address? ServiceAddress { get; private set; }

    /// <summary>
    /// Gets the current sync status of the customer.
    /// </summary>
    public SyncStatus SyncStatus { get; private set; } = SyncStatus.Pending;

    /// <summary>
    /// Gets the timestamp of the last successful sync.
    /// </summary>
    public DateTime? LastSyncedAt { get; private set; }

    /// <summary>
    /// Gets the timestamp when the customer was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the read-only collection of utility accounts.
    /// </summary>
    public IReadOnlyCollection<UtilityAccount> UtilityAccounts => _utilityAccounts.AsReadOnly();

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="tenantId">The tenant identifier</param>
    /// <param name="name">The customer's name</param>
    /// <param name="emailAddress">The customer's email address</param>
    /// <param name="phoneNumber">The customer's phone number (optional)</param>
    /// <param name="serviceAddress">The customer's service address (optional)</param>
    /// <returns>A result containing the customer or an error</returns>
    public static Result<Customer> Create(
        TenantId tenantId,
        CustomerName name,
        EmailAddress emailAddress,
        PhoneNumber? phoneNumber = null,
        Address? serviceAddress = null)
    {
        ArgumentNullException.ThrowIfNull(tenantId);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(emailAddress);

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = name,
            EmailAddress = emailAddress,
            PhoneNumber = phoneNumber,
            ServiceAddress = serviceAddress,
            CreatedAt = DateTime.UtcNow,
            SyncStatus = SyncStatus.Pending
        };

        customer.RaiseDomainEvent(new CustomerCreatedEvent(
            customer.Id,
            tenantId.Value,
            name.FullName,
            emailAddress.Value));

        return Result<Customer>.Success(customer);
    }

    /// <summary>
    /// Adds a utility account to the customer.
    /// </summary>
    /// <param name="accountNumber">The account number</param>
    /// <param name="provider">The utility provider</param>
    /// <param name="meterNumber">The meter number (optional)</param>
    /// <param name="serviceAddress">The service address (optional)</param>
    /// <returns>A result containing the utility account or an error</returns>
    public Result<UtilityAccount> AddUtilityAccount(
        AccountNumber accountNumber,
        UtilityProvider provider,
        MeterNumber? meterNumber = null,
        Address? serviceAddress = null)
    {
        ArgumentNullException.ThrowIfNull(accountNumber);

        // Check if an account with the same account number already exists
        if (_utilityAccounts.Any(ua => ua.AccountNumber.Equals(accountNumber)))
        {
            return Result<UtilityAccount>.Failure(
                new Error("UtilityAccount.Duplicate", $"An account with number {accountNumber} already exists for this customer."));
        }

        var result = UtilityAccount.Create(accountNumber, provider, meterNumber, serviceAddress);

        if (result.IsFailure)
        {
            return result;
        }

        var account = result.Value;
        _utilityAccounts.Add(account);

        RaiseDomainEvent(new UtilityAccountAddedEvent(
            Id,
            account.Id,
            account.AccountNumber.Value,
            account.Provider));

        return Result<UtilityAccount>.Success(account);
    }

    /// <summary>
    /// Removes a utility account from the customer.
    /// </summary>
    /// <param name="accountId">The ID of the account to remove</param>
    /// <returns>A result indicating success or failure</returns>
    public Result RemoveUtilityAccount(Guid accountId)
    {
        var account = _utilityAccounts.FirstOrDefault(ua => ua.Id == accountId);

        if (account is null)
        {
            return Result.Failure(
                new Error("UtilityAccount.NotFound", "The specified utility account was not found."));
        }

        _utilityAccounts.Remove(account);

        RaiseDomainEvent(new UtilityAccountRemovedEvent(Id, accountId));

        return Result.Success();
    }

    /// <summary>
    /// Gets a utility account by its ID.
    /// </summary>
    /// <param name="accountId">The ID of the account</param>
    /// <returns>The utility account or null if not found</returns>
    public UtilityAccount? GetUtilityAccount(Guid accountId)
    {
        return _utilityAccounts.FirstOrDefault(ua => ua.Id == accountId);
    }

    /// <summary>
    /// Gets a utility account by its account number.
    /// </summary>
    /// <param name="accountNumber">The account number</param>
    /// <returns>The utility account or null if not found</returns>
    public UtilityAccount? GetUtilityAccountByNumber(AccountNumber accountNumber)
    {
        return _utilityAccounts.FirstOrDefault(ua => ua.AccountNumber.Equals(accountNumber));
    }

    /// <summary>
    /// Updates the customer's contact information.
    /// </summary>
    /// <param name="emailAddress">The new email address</param>
    /// <param name="phoneNumber">The new phone number (optional)</param>
    /// <param name="serviceAddress">The new service address (optional)</param>
    /// <returns>A result indicating success or failure</returns>
    public Result UpdateContactInfo(
        EmailAddress? emailAddress = null,
        PhoneNumber? phoneNumber = null,
        Address? serviceAddress = null)
    {
        var updated = false;

        if (emailAddress is not null && !emailAddress.Equals(EmailAddress))
        {
            EmailAddress = emailAddress;
            updated = true;
        }

        if (phoneNumber is not null && !phoneNumber.Equals(PhoneNumber))
        {
            PhoneNumber = phoneNumber;
            updated = true;
        }

        if (serviceAddress is not null && !serviceAddress.Equals(ServiceAddress))
        {
            ServiceAddress = serviceAddress;
            updated = true;
        }

        if (updated)
        {
            RaiseDomainEvent(new CustomerContactInfoUpdatedEvent(
                Id,
                emailAddress?.Value,
                phoneNumber?.Value,
                serviceAddress?.Street,
                serviceAddress?.City,
                serviceAddress?.State,
                serviceAddress?.ZipCode));
        }

        return Result.Success();
    }

    /// <summary>
    /// Marks the customer as synced.
    /// </summary>
    public void MarkAsSynced()
    {
        SyncStatus = SyncStatus.Synced;
        LastSyncedAt = DateTime.UtcNow;

        RaiseDomainEvent(new CustomerSyncedEvent(
            Id,
            LastSyncedAt.Value,
            _utilityAccounts.Count));
    }

    /// <summary>
    /// Marks the customer sync as failed.
    /// </summary>
    /// <param name="reason">The reason for the sync failure</param>
    public void MarkSyncAsFailed(string reason)
    {
        ArgumentNullException.ThrowIfNull(reason);

        SyncStatus = SyncStatus.Failed;

        RaiseDomainEvent(new CustomerSyncFailedEvent(
            Id,
            DateTime.UtcNow,
            reason));
    }

    /// <summary>
    /// Marks the customer sync as in progress.
    /// </summary>
    public void MarkSyncAsInProgress()
    {
        SyncStatus = SyncStatus.InProgress;
    }

    /// <summary>
    /// Gets the count of synced utility accounts.
    /// </summary>
    /// <returns>The count of synced accounts</returns>
    public int GetSyncedAccountCount()
    {
        return _utilityAccounts.Count(ua => ua.SyncStatus == SyncStatus.Synced);
    }

    /// <summary>
    /// Gets the count of failed utility account syncs.
    /// </summary>
    /// <returns>The count of failed accounts</returns>
    public int GetFailedAccountCount()
    {
        return _utilityAccounts.Count(ua => ua.SyncStatus == SyncStatus.Failed);
    }

    /// <summary>
    /// Determines if all utility accounts have been synced.
    /// </summary>
    /// <returns>True if all accounts are synced; otherwise false</returns>
    public bool AreAllAccountsSynced()
    {
        return _utilityAccounts.Count > 0 && _utilityAccounts.All(ua => ua.SyncStatus == SyncStatus.Synced);
    }

    /// <summary>
    /// Determines if the customer has any utility accounts.
    /// </summary>
    /// <returns>True if the customer has utility accounts; otherwise false</returns>
    public bool HasUtilityAccounts()
    {
        return _utilityAccounts.Count > 0;
    }
}
