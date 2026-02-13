using CCA.Sync.Domain.Aggregates.Customer;
using CCA.Sync.Domain.Enums;
using CCA.Sync.Domain.Events;
using CCA.Sync.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CCA.Sync.Domain.Tests.Aggregates.Customer;

/// <summary>
/// Unit tests for the Customer aggregate root.
/// </summary>
public class CustomerTests
{
    // Helper methods
    private static TenantId CreateTenantId()
    {
        return TenantId.Create(Guid.NewGuid()).Value;
    }

    private static CustomerName CreateCustomerName(string firstName = "John", string lastName = "Doe")
    {
        return CustomerName.Create(firstName, lastName).Value;
    }

    private static EmailAddress CreateEmailAddress(string email = "john@example.com")
    {
        return EmailAddress.Create(email).Value;
    }

    private static PhoneNumber CreatePhoneNumber(string phone = "5551234567")
    {
        return PhoneNumber.Create(phone).Value;
    }

    private static Address CreateAddress(
        string street = "123 Main St",
        string city = "San Francisco",
        string state = "CA",
        string zipCode = "94102")
    {
        return Address.Create(street, city, state, zipCode).Value;
    }

    private static AccountNumber CreateAccountNumber(string number = "ACC123456")
    {
        return AccountNumber.Create(number).Value;
    }

    private static MeterNumber CreateMeterNumber(string number = "METER123456")
    {
        return MeterNumber.Create(number).Value;
    }

    // Customer Creation Tests
    [Fact]
    public void Create_WithValidParameters_SucceedsAndRaisesCustomerCreatedEvent()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var name = CreateCustomerName();
        var email = CreateEmailAddress();

        // Act
        var result = Customer.Create(tenantId, name, email);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var customer = result.Value;
        customer.Id.Should().NotBe(Guid.Empty);
        customer.TenantId.Should().Be(tenantId);
        customer.Name.Should().Be(name);
        customer.EmailAddress.Should().Be(email);
        customer.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        customer.SyncStatus.Should().Be(SyncStatus.Pending);
        customer.DomainEvents.Should().HaveCount(1);
        customer.DomainEvents.First().Should().BeOfType<CustomerCreatedEvent>();
    }

    [Fact]
    public void Create_WithPhoneNumberAndAddress_IncludesOptionalData()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var name = CreateCustomerName();
        var email = CreateEmailAddress();
        var phone = CreatePhoneNumber();
        var address = CreateAddress();

        // Act
        var result = Customer.Create(tenantId, name, email, phone, address);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var customer = result.Value;
        customer.PhoneNumber.Should().Be(phone);
        customer.ServiceAddress.Should().Be(address);
    }

    [Fact]
    public void Create_WithNullTenantId_Fails()
    {
        // Arrange
        var name = CreateCustomerName();
        var email = CreateEmailAddress();

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => Customer.Create(null!, name, email));
    }

    [Fact]
    public void Create_WithNullName_Fails()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var email = CreateEmailAddress();

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => Customer.Create(tenantId, null!, email));
    }

    [Fact]
    public void Create_WithNullEmail_Fails()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var name = CreateCustomerName();

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => Customer.Create(tenantId, name, null!));
    }

    // Utility Account Tests
    [Fact]
    public void AddUtilityAccount_WithValidParameters_SucceedsAndRaisesEvent()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        var accountNumber = CreateAccountNumber();
        var meterNumber = CreateMeterNumber();
        var address = CreateAddress();

        // Act
        var result = customer.AddUtilityAccount(accountNumber, UtilityProvider.PGE, meterNumber, address);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var account = result.Value;
        account.Id.Should().NotBe(Guid.Empty);
        account.AccountNumber.Should().Be(accountNumber);
        account.Provider.Should().Be(UtilityProvider.PGE);
        account.MeterNumber.Should().Be(meterNumber);
        account.ServiceAddress.Should().Be(address);
        account.SyncStatus.Should().Be(SyncStatus.Pending);

        customer.UtilityAccounts.Should().HaveCount(1);
        customer.DomainEvents.Should().Contain(e => e is UtilityAccountAddedEvent);
    }

    [Fact]
    public void AddUtilityAccount_WithoutOptionalFields_Succeeds()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        var accountNumber = CreateAccountNumber();

        // Act
        var result = customer.AddUtilityAccount(accountNumber, UtilityProvider.SCE);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var account = result.Value;
        account.MeterNumber.Should().BeNull();
        account.ServiceAddress.Should().BeNull();
    }

    [Fact]
    public void AddUtilityAccount_WithDuplicateAccountNumber_Fails()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        var accountNumber = CreateAccountNumber("ACC123");

        customer.AddUtilityAccount(accountNumber, UtilityProvider.PGE);

        // Act
        var result = customer.AddUtilityAccount(accountNumber, UtilityProvider.SCE);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("UtilityAccount.Duplicate");
        customer.UtilityAccounts.Should().HaveCount(1);
    }

    [Fact]
    public void AddUtilityAccount_WithNullAccountNumber_Fails()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => customer.AddUtilityAccount(null!, UtilityProvider.PGE));
    }

    [Fact]
    public void RemoveUtilityAccount_WithValidId_SucceedsAndRaisesEvent()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        var account = customer.AddUtilityAccount(CreateAccountNumber(), UtilityProvider.PGE).Value;

        customer.ClearDomainEvents();

        // Act
        var result = customer.RemoveUtilityAccount(account.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        customer.UtilityAccounts.Should().HaveCount(0);
        customer.DomainEvents.Should().Contain(e => e is UtilityAccountRemovedEvent);
    }

    [Fact]
    public void RemoveUtilityAccount_WithInvalidId_Fails()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;

        // Act
        var result = customer.RemoveUtilityAccount(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("UtilityAccount.NotFound");
    }

    [Fact]
    public void GetUtilityAccount_WithValidId_ReturnsAccount()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        var account = customer.AddUtilityAccount(CreateAccountNumber(), UtilityProvider.PGE).Value;

        // Act
        var result = customer.GetUtilityAccount(account.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(account.Id);
    }

    [Fact]
    public void GetUtilityAccount_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;

        // Act
        var result = customer.GetUtilityAccount(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetUtilityAccountByNumber_WithValidNumber_ReturnsAccount()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        var accountNumber = CreateAccountNumber("ACC999");
        var account = customer.AddUtilityAccount(accountNumber, UtilityProvider.SDG_E).Value;

        // Act
        var result = customer.GetUtilityAccountByNumber(accountNumber);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(account.Id);
    }

    [Fact]
    public void GetUtilityAccountByNumber_WithInvalidNumber_ReturnsNull()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;

        // Act
        var result = customer.GetUtilityAccountByNumber(CreateAccountNumber("INVALID"));

        // Assert
        result.Should().BeNull();
    }

    // Contact Info Update Tests
    [Fact]
    public void UpdateContactInfo_WithNewEmail_SucceedsAndRaisesEvent()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress("old@example.com")).Value;
        var newEmail = CreateEmailAddress("new@example.com");
        customer.ClearDomainEvents();

        // Act
        var result = customer.UpdateContactInfo(newEmail);

        // Assert
        result.IsSuccess.Should().BeTrue();
        customer.EmailAddress.Should().Be(newEmail);
        customer.DomainEvents.Should().Contain(e => e is CustomerContactInfoUpdatedEvent);
    }

    [Fact]
    public void UpdateContactInfo_WithNewPhone_SucceedsAndRaisesEvent()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        var newPhone = CreatePhoneNumber("5559876543");
        customer.ClearDomainEvents();

        // Act
        var result = customer.UpdateContactInfo(phoneNumber: newPhone);

        // Assert
        result.IsSuccess.Should().BeTrue();
        customer.PhoneNumber.Should().Be(newPhone);
        customer.DomainEvents.Should().Contain(e => e is CustomerContactInfoUpdatedEvent);
    }

    [Fact]
    public void UpdateContactInfo_WithNewAddress_SucceedsAndRaisesEvent()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        var newAddress = CreateAddress("456 Market St", "Los Angeles", "CA", "90001");
        customer.ClearDomainEvents();

        // Act
        var result = customer.UpdateContactInfo(serviceAddress: newAddress);

        // Assert
        result.IsSuccess.Should().BeTrue();
        customer.ServiceAddress.Should().Be(newAddress);
        customer.DomainEvents.Should().Contain(e => e is CustomerContactInfoUpdatedEvent);
    }

    [Fact]
    public void UpdateContactInfo_WithSameData_DoesNotRaiseEvent()
    {
        // Arrange
        var email = CreateEmailAddress();
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), email).Value;
        customer.ClearDomainEvents();

        // Act
        var result = customer.UpdateContactInfo(email);

        // Assert
        result.IsSuccess.Should().BeTrue();
        customer.DomainEvents.Should().HaveCount(0);
    }

    // Sync Status Tests
    [Fact]
    public void MarkAsSynced_UpdatesStatusAndRaisesEvent()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        customer.AddUtilityAccount(CreateAccountNumber(), UtilityProvider.PGE);
        customer.ClearDomainEvents();

        // Act
        customer.MarkAsSynced();

        // Assert
        customer.SyncStatus.Should().Be(SyncStatus.Synced);
        customer.LastSyncedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        customer.DomainEvents.Should().Contain(e => e is CustomerSyncedEvent);
    }

    [Fact]
    public void MarkSyncAsFailed_UpdatesStatusAndRaisesEvent()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        var reason = "Connection timeout";
        customer.ClearDomainEvents();

        // Act
        customer.MarkSyncAsFailed(reason);

        // Assert
        customer.SyncStatus.Should().Be(SyncStatus.Failed);
        customer.DomainEvents.Should().Contain(e => e is CustomerSyncFailedEvent);
    }

    [Fact]
    public void MarkSyncAsFailed_WithNullReason_Fails()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => customer.MarkSyncAsFailed(null!));
    }

    [Fact]
    public void MarkSyncAsInProgress_UpdatesStatus()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;

        // Act
        customer.MarkSyncAsInProgress();

        // Assert
        customer.SyncStatus.Should().Be(SyncStatus.InProgress);
    }

    // Query Methods Tests
    [Fact]
    public void GetSyncedAccountCount_ReturnsCorrectCount()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        var account1 = customer.AddUtilityAccount(CreateAccountNumber("ACC001"), UtilityProvider.PGE).Value;
        var account2 = customer.AddUtilityAccount(CreateAccountNumber("ACC002"), UtilityProvider.SCE).Value;
        var account3 = customer.AddUtilityAccount(CreateAccountNumber("ACC003"), UtilityProvider.SDG_E).Value;

        account1.MarkAsSynced();
        account2.MarkAsSynced();

        // Act
        var count = customer.GetSyncedAccountCount();

        // Assert
        count.Should().Be(2);
    }

    [Fact]
    public void GetFailedAccountCount_ReturnsCorrectCount()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        var account1 = customer.AddUtilityAccount(CreateAccountNumber("ACC001"), UtilityProvider.PGE).Value;
        var account2 = customer.AddUtilityAccount(CreateAccountNumber("ACC002"), UtilityProvider.SCE).Value;

        account1.MarkSyncAsFailed();

        // Act
        var count = customer.GetFailedAccountCount();

        // Assert
        count.Should().Be(1);
    }

    [Fact]
    public void AreAllAccountsSynced_WithAllSynced_ReturnsTrue()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        var account1 = customer.AddUtilityAccount(CreateAccountNumber("ACC001"), UtilityProvider.PGE).Value;
        var account2 = customer.AddUtilityAccount(CreateAccountNumber("ACC002"), UtilityProvider.SCE).Value;

        account1.MarkAsSynced();
        account2.MarkAsSynced();

        // Act
        var result = customer.AreAllAccountsSynced();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void AreAllAccountsSynced_WithPartialSync_ReturnsFalse()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        var account1 = customer.AddUtilityAccount(CreateAccountNumber("ACC001"), UtilityProvider.PGE).Value;
        var account2 = customer.AddUtilityAccount(CreateAccountNumber("ACC002"), UtilityProvider.SCE).Value;

        account1.MarkAsSynced();

        // Act
        var result = customer.AreAllAccountsSynced();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void AreAllAccountsSynced_WithNoAccounts_ReturnsFalse()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;

        // Act
        var result = customer.AreAllAccountsSynced();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasUtilityAccounts_WithAccounts_ReturnsTrue()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        customer.AddUtilityAccount(CreateAccountNumber(), UtilityProvider.PGE);

        // Act
        var result = customer.HasUtilityAccounts();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasUtilityAccounts_WithoutAccounts_ReturnsFalse()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;

        // Act
        var result = customer.HasUtilityAccounts();

        // Assert
        result.Should().BeFalse();
    }

    // Aggregate Behavior Tests
    [Fact]
    public void Customer_CanHaveMultipleUtilityAccounts()
    {
        // Arrange & Act
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;

        for (int i = 0; i < 5; i++)
        {
            customer.AddUtilityAccount(CreateAccountNumber($"ACC{i:D3}"), (UtilityProvider)(i % 3));
        }

        // Assert
        customer.UtilityAccounts.Should().HaveCount(5);
        customer.UtilityAccounts.Select(ua => ua.Provider).Should()
            .Contain(new[] { UtilityProvider.PGE, UtilityProvider.SCE, UtilityProvider.SDG_E });
    }

    [Fact]
    public void Customer_UtilityAccountsAreReadOnly()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        customer.AddUtilityAccount(CreateAccountNumber(), UtilityProvider.PGE);

        // Act & Assert
        var accounts = customer.UtilityAccounts;
        _ = Assert.Throws<NotSupportedException>(() => accounts.Add(
            UtilityAccount.Create(CreateAccountNumber("ACC999"), UtilityProvider.SCE).Value));
    }

    [Fact]
    public void Customer_DomainEventsCanBeCleared()
    {
        // Arrange
        var customer = Customer.Create(CreateTenantId(), CreateCustomerName(), CreateEmailAddress()).Value;
        customer.AddUtilityAccount(CreateAccountNumber(), UtilityProvider.PGE);

        // Act
        customer.ClearDomainEvents();

        // Assert
        customer.DomainEvents.Should().BeEmpty();
    }
}
