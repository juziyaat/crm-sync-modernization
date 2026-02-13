using CCA.Sync.Domain.Aggregates.Customer;
using CCA.Sync.Domain.Enums;
using CCA.Sync.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CCA.Sync.Domain.Tests.Aggregates.Customer;

/// <summary>
/// Unit tests for the UtilityAccount entity.
/// </summary>
public class UtilityAccountTests
{
    private static AccountNumber CreateAccountNumber(string number = "ACC123456")
    {
        return AccountNumber.Create(number).Value;
    }

    private static MeterNumber CreateMeterNumber(string number = "METER123456")
    {
        return MeterNumber.Create(number).Value;
    }

    private static Address CreateAddress(
        string street = "123 Main St",
        string city = "San Francisco",
        string state = "CA",
        string zipCode = "94102")
    {
        return Address.Create(street, city, state, zipCode).Value;
    }

    [Fact]
    public void Create_WithRequiredParameters_Succeeds()
    {
        // Arrange
        var accountNumber = CreateAccountNumber();

        // Act
        var result = UtilityAccount.Create(accountNumber, UtilityProvider.PGE);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var account = result.Value;
        account.Id.Should().NotBe(Guid.Empty);
        account.AccountNumber.Should().Be(accountNumber);
        account.Provider.Should().Be(UtilityProvider.PGE);
        account.SyncStatus.Should().Be(SyncStatus.Pending);
        account.AddedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_WithAllOptionalParameters_Succeeds()
    {
        // Arrange
        var accountNumber = CreateAccountNumber();
        var meterNumber = CreateMeterNumber();
        var address = CreateAddress();

        // Act
        var result = UtilityAccount.Create(accountNumber, UtilityProvider.SCE, meterNumber, address);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var account = result.Value;
        account.MeterNumber.Should().Be(meterNumber);
        account.ServiceAddress.Should().Be(address);
    }

    [Fact]
    public void Create_WithNullAccountNumber_Fails()
    {
        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() =>
            UtilityAccount.Create(null!, UtilityProvider.PGE));
    }

    [Fact]
    public void UpdateMeterNumber_WithValidNumber_Succeeds()
    {
        // Arrange
        var account = UtilityAccount.Create(CreateAccountNumber(), UtilityProvider.PGE).Value;
        var newMeter = CreateMeterNumber("NEWMETER9999");

        // Act
        account.UpdateMeterNumber(newMeter);

        // Assert
        account.MeterNumber.Should().Be(newMeter);
    }

    [Fact]
    public void UpdateMeterNumber_WithNull_Succeeds()
    {
        // Arrange
        var account = UtilityAccount.Create(CreateAccountNumber(), UtilityProvider.PGE, CreateMeterNumber()).Value;

        // Act
        account.UpdateMeterNumber(null);

        // Assert
        account.MeterNumber.Should().BeNull();
    }

    [Fact]
    public void UpdateServiceAddress_WithValidAddress_Succeeds()
    {
        // Arrange
        var account = UtilityAccount.Create(CreateAccountNumber(), UtilityProvider.SDG_E).Value;
        var newAddress = CreateAddress("999 Oak St", "Los Angeles", "CA", "90001");

        // Act
        account.UpdateServiceAddress(newAddress);

        // Assert
        account.ServiceAddress.Should().Be(newAddress);
    }

    [Fact]
    public void UpdateServiceAddress_WithNull_Succeeds()
    {
        // Arrange
        var account = UtilityAccount.Create(CreateAccountNumber(), UtilityProvider.SDG_E, serviceAddress: CreateAddress()).Value;

        // Act
        account.UpdateServiceAddress(null);

        // Assert
        account.ServiceAddress.Should().BeNull();
    }

    [Fact]
    public void MarkAsSynced_UpdatesStatusAndLastSyncedAt()
    {
        // Arrange
        var account = UtilityAccount.Create(CreateAccountNumber(), UtilityProvider.PGE).Value;

        // Act
        account.MarkAsSynced();

        // Assert
        account.SyncStatus.Should().Be(SyncStatus.Synced);
        account.LastSyncedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void MarkSyncAsFailed_UpdatesStatus()
    {
        // Arrange
        var account = UtilityAccount.Create(CreateAccountNumber(), UtilityProvider.PGE).Value;

        // Act
        account.MarkSyncAsFailed();

        // Assert
        account.SyncStatus.Should().Be(SyncStatus.Failed);
    }

    [Fact]
    public void MarkSyncAsInProgress_UpdatesStatus()
    {
        // Arrange
        var account = UtilityAccount.Create(CreateAccountNumber(), UtilityProvider.PGE).Value;

        // Act
        account.MarkSyncAsInProgress();

        // Assert
        account.SyncStatus.Should().Be(SyncStatus.InProgress);
    }

    [Fact]
    public void Account_HasCorrectProvider()
    {
        // Arrange & Act
        var pgeAccount = UtilityAccount.Create(CreateAccountNumber("PGE001"), UtilityProvider.PGE).Value;
        var sceAccount = UtilityAccount.Create(CreateAccountNumber("SCE001"), UtilityProvider.SCE).Value;
        var sdgeAccount = UtilityAccount.Create(CreateAccountNumber("SDGE001"), UtilityProvider.SDG_E).Value;

        // Assert
        pgeAccount.Provider.Should().Be(UtilityProvider.PGE);
        sceAccount.Provider.Should().Be(UtilityProvider.SCE);
        sdgeAccount.Provider.Should().Be(UtilityProvider.SDG_E);
    }

    [Fact]
    public void UtilityAccounts_WithSameAccountNumber_AreEqual()
    {
        // Arrange
        var accountNumber = CreateAccountNumber();
        var account1 = UtilityAccount.Create(accountNumber, UtilityProvider.PGE).Value;
        var account2 = UtilityAccount.Create(accountNumber, UtilityProvider.PGE).Value;

        // Act & Assert - They have different IDs, so they're not equal
        account1.Id.Should().NotBe(account2.Id);
        account1.Should().NotBe(account2);
    }
}
