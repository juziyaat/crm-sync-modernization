using CCA.Sync.Domain.Enums;
using CCA.Sync.Domain.Events;
using CCA.Sync.Domain.ValueObjects;
using FluentAssertions;
using Xunit;
using LdcAccountAggregate = CCA.Sync.Domain.Aggregates.LdcAccount.LdcAccount;
using SyncConfigurationValue = CCA.Sync.Domain.Aggregates.LdcAccount.SyncConfiguration;

namespace CCA.Sync.Domain.Tests.Aggregates.LdcAccount;

/// <summary>
/// Unit tests for the LdcAccount aggregate root.
/// </summary>
public class LdcAccountTests
{
    // Helper methods
    private static TenantId CreateTenantId()
    {
        return TenantId.Create(Guid.NewGuid()).Value;
    }

    private static SyncConfigurationValue CreateSyncConfiguration(bool isEnabled = true)
    {
        return SyncConfigurationValue.Create(isEnabled, 60, 3, 300).Value;
    }

    // LdcAccount Creation Tests
    [Fact]
    public void Create_WithValidParameters_SucceedsAndRaisesLdcAccountCreatedEvent()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var provider = LdcProvider.PGE;
        var accountName = "PG&E Production Account";
        var username = "pge_user";
        var encryptedPassword = "encrypted_password_hash";

        // Act
        var result = LdcAccountAggregate.Create(tenantId, provider, accountName, username, encryptedPassword);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var account = result.Value;
        account.Id.Should().NotBe(Guid.Empty);
        account.TenantId.Should().Be(tenantId);
        account.Provider.Should().Be(provider);
        account.AccountName.Should().Be(accountName);
        account.Username.Should().Be(username);
        account.EncryptedPassword.Should().Be(encryptedPassword);
        account.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        account.SyncStatus.Should().Be(SyncStatus.Pending);
        account.SyncConfiguration.Should().NotBeNull();
        account.SyncConfiguration.IsEnabled.Should().BeTrue();
        account.DomainEvents.Should().HaveCount(1);
        account.DomainEvents.First().Should().BeOfType<LdcAccountCreatedEvent>();
    }

    [Fact]
    public void Create_WithCustomSyncConfiguration_UsesProvidedConfiguration()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var syncConfig = SyncConfigurationValue.Create(false, 120, 5, 600).Value;

        // Act
        var result = LdcAccountAggregate.Create(tenantId, LdcProvider.SCE, "Test Account", "user", "pass", syncConfig);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SyncConfiguration.Should().Be(syncConfig);
        result.Value.SyncConfiguration.IsEnabled.Should().BeFalse();
        result.Value.SyncConfiguration.SyncIntervalMinutes.Should().Be(120);
    }

    [Fact]
    public void Create_WithoutSyncConfiguration_UsesDefaultConfiguration()
    {
        // Arrange
        var tenantId = CreateTenantId();

        // Act
        var result = LdcAccountAggregate.Create(tenantId, LdcProvider.PGE, "Test Account", "user", "pass");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SyncConfiguration.IsEnabled.Should().BeTrue();
        result.Value.SyncConfiguration.SyncIntervalMinutes.Should().Be(60);
        result.Value.SyncConfiguration.MaxRetries.Should().Be(3);
        result.Value.SyncConfiguration.TimeoutSeconds.Should().Be(300);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidAccountName_Fails(string? accountName)
    {
        // Arrange
        var tenantId = CreateTenantId();

        // Act
        var result = LdcAccountAggregate.Create(tenantId, LdcProvider.PGE, accountName!, "user", "pass");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("LdcAccount.InvalidAccountName");
        result.Error.Description.Should().Contain("cannot be empty");
    }

    [Fact]
    public void Create_WithAccountNameExceedingMaxLength_Fails()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var longAccountName = new string('A', 201);

        // Act
        var result = LdcAccountAggregate.Create(tenantId, LdcProvider.PGE, longAccountName, "user", "pass");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("LdcAccount.InvalidAccountName");
        result.Error.Description.Should().Contain("cannot exceed 200 characters");
    }

    [Fact]
    public void Create_WithAccountNameAtMaxLength_Succeeds()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var accountName = new string('A', 200);

        // Act
        var result = LdcAccountAggregate.Create(tenantId, LdcProvider.PGE, accountName, "user", "pass");

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidUsername_Fails(string? username)
    {
        // Arrange
        var tenantId = CreateTenantId();

        // Act
        var result = LdcAccountAggregate.Create(tenantId, LdcProvider.PGE, "Test Account", username!, "pass");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("LdcAccount.InvalidUsername");
        result.Error.Description.Should().Contain("cannot be empty");
    }

    [Fact]
    public void Create_WithUsernameExceedingMaxLength_Fails()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var longUsername = new string('A', 101);

        // Act
        var result = LdcAccountAggregate.Create(tenantId, LdcProvider.PGE, "Test Account", longUsername, "pass");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("LdcAccount.InvalidUsername");
        result.Error.Description.Should().Contain("cannot exceed 100 characters");
    }

    [Fact]
    public void Create_WithUsernameAtMaxLength_Succeeds()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var username = new string('A', 100);

        // Act
        var result = LdcAccountAggregate.Create(tenantId, LdcProvider.PGE, "Test Account", username, "pass");

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidPassword_Fails(string? password)
    {
        // Arrange
        var tenantId = CreateTenantId();

        // Act
        var result = LdcAccountAggregate.Create(tenantId, LdcProvider.PGE, "Test Account", "user", password!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("LdcAccount.InvalidPassword");
        result.Error.Description.Should().Contain("cannot be empty");
    }

    [Fact]
    public void Create_TrimsAccountNameAndUsername()
    {
        // Arrange
        var tenantId = CreateTenantId();

        // Act
        var result = LdcAccountAggregate.Create(tenantId, LdcProvider.PGE, "  Test Account  ", "  user  ", "pass");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.AccountName.Should().Be("Test Account");
        result.Value.Username.Should().Be("user");
    }

    [Fact]
    public void Create_WithNullTenantId_ThrowsArgumentNullException()
    {
        // Arrange & Act
        var act = () => LdcAccount.Create(null!, LdcProvider.PGE, "Test Account", "user", "pass");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    // Credential Update Tests
    [Fact]
    public void UpdateCredentials_WithValidCredentials_SucceedsAndRaisesEvent()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "old_user", "old_pass").Value;
        account.ClearDomainEvents();

        // Act
        var result = account.UpdateCredentials("new_user", "new_encrypted_pass");

        // Assert
        result.IsSuccess.Should().BeTrue();
        account.Username.Should().Be("new_user");
        account.EncryptedPassword.Should().Be("new_encrypted_pass");
        account.ModifiedAt.Should().NotBeNull();
        account.ModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        account.DomainEvents.Should().HaveCount(1);
        var evt = account.DomainEvents.First() as LdcAccountCredentialsUpdatedEvent;
        evt.Should().NotBeNull();
        evt!.Username.Should().Be("new_user");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateCredentials_WithInvalidUsername_Fails(string? username)
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;

        // Act
        var result = account.UpdateCredentials(username!, "new_pass");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("LdcAccount.InvalidUsername");
    }

    [Fact]
    public void UpdateCredentials_WithUsernameTooLong_Fails()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;
        var longUsername = new string('A', 101);

        // Act
        var result = account.UpdateCredentials(longUsername, "new_pass");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("LdcAccount.InvalidUsername");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateCredentials_WithInvalidPassword_Fails(string? password)
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;

        // Act
        var result = account.UpdateCredentials("new_user", password!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("LdcAccount.InvalidPassword");
    }

    [Fact]
    public void UpdateCredentials_TrimsUsername()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;

        // Act
        account.UpdateCredentials("  new_user  ", "new_pass");

        // Assert
        account.Username.Should().Be("new_user");
    }

    // Sync Enable/Disable Tests
    [Fact]
    public void EnableSync_WhenDisabled_SucceedsAndRaisesEvent()
    {
        // Arrange
        var syncConfig = SyncConfigurationValue.Create(false, 60, 3, 300).Value;
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.SCE, "Test Account", "user", "pass", syncConfig).Value;
        account.ClearDomainEvents();

        // Act
        var result = account.EnableSync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        account.SyncConfiguration.IsEnabled.Should().BeTrue();
        account.ModifiedAt.Should().NotBeNull();
        account.DomainEvents.Should().HaveCount(1);
        account.DomainEvents.First().Should().BeOfType<LdcAccountSyncEnabledEvent>();
    }

    [Fact]
    public void EnableSync_WhenAlreadyEnabled_Fails()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;

        // Act
        var result = account.EnableSync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("LdcAccount.SyncAlreadyEnabled");
    }

    [Fact]
    public void DisableSync_WhenEnabled_SucceedsAndRaisesEvent()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.SDG_E, "Test Account", "user", "pass").Value;
        account.ClearDomainEvents();

        // Act
        var result = account.DisableSync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        account.SyncConfiguration.IsEnabled.Should().BeFalse();
        account.ModifiedAt.Should().NotBeNull();
        account.DomainEvents.Should().HaveCount(1);
        account.DomainEvents.First().Should().BeOfType<LdcAccountSyncDisabledEvent>();
    }

    [Fact]
    public void DisableSync_WhenAlreadyDisabled_Fails()
    {
        // Arrange
        var syncConfig = SyncConfigurationValue.Create(false, 60, 3, 300).Value;
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass", syncConfig).Value;

        // Act
        var result = account.DisableSync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("LdcAccount.SyncAlreadyDisabled");
    }

    // Sync Configuration Update Tests
    [Fact]
    public void UpdateSyncConfiguration_WithValidConfiguration_Succeeds()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;
        var newConfig = SyncConfigurationValue.Create(true, 120, 5, 600).Value;
        account.ClearDomainEvents();

        // Act
        var result = account.UpdateSyncConfiguration(newConfig);

        // Assert
        result.IsSuccess.Should().BeTrue();
        account.SyncConfiguration.Should().Be(newConfig);
        account.ModifiedAt.Should().NotBeNull();
    }

    [Fact]
    public void UpdateSyncConfiguration_FromDisabledToEnabled_RaisesSyncEnabledEvent()
    {
        // Arrange
        var disabledConfig = SyncConfigurationValue.Create(false, 60, 3, 300).Value;
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass", disabledConfig).Value;
        var enabledConfig = SyncConfigurationValue.Create(true, 60, 3, 300).Value;
        account.ClearDomainEvents();

        // Act
        account.UpdateSyncConfiguration(enabledConfig);

        // Assert
        account.DomainEvents.Should().HaveCount(1);
        account.DomainEvents.First().Should().BeOfType<LdcAccountSyncEnabledEvent>();
    }

    [Fact]
    public void UpdateSyncConfiguration_FromEnabledToDisabled_RaisesSyncDisabledEvent()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;
        var disabledConfig = SyncConfigurationValue.Create(false, 60, 3, 300).Value;
        account.ClearDomainEvents();

        // Act
        account.UpdateSyncConfiguration(disabledConfig);

        // Assert
        account.DomainEvents.Should().HaveCount(1);
        account.DomainEvents.First().Should().BeOfType<LdcAccountSyncDisabledEvent>();
    }

    [Fact]
    public void UpdateSyncConfiguration_WithSameEnabledState_DoesNotRaiseEvent()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;
        var newConfig = SyncConfigurationValue.Create(true, 120, 5, 600).Value;
        account.ClearDomainEvents();

        // Act
        account.UpdateSyncConfiguration(newConfig);

        // Assert
        account.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void UpdateSyncConfiguration_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;

        // Act
        var act = () => account.UpdateSyncConfiguration(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    // Sync Status Tests
    [Fact]
    public void MarkAsSynced_UpdatesSyncStatusAndLastSyncedAt()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;

        // Act
        account.MarkAsSynced();

        // Assert
        account.SyncStatus.Should().Be(SyncStatus.Synced);
        account.LastSyncedAt.Should().NotBeNull();
        account.LastSyncedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void MarkSyncAsFailed_UpdatesSyncStatus()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;

        // Act
        account.MarkSyncAsFailed();

        // Assert
        account.SyncStatus.Should().Be(SyncStatus.Failed);
    }

    [Fact]
    public void MarkSyncAsInProgress_UpdatesSyncStatus()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;

        // Act
        account.MarkSyncAsInProgress();

        // Assert
        account.SyncStatus.Should().Be(SyncStatus.InProgress);
    }

    // IsReadyForSync Tests
    [Fact]
    public void IsReadyForSync_WhenEnabledWithCredentials_ReturnsTrue()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;

        // Act
        var isReady = account.IsReadyForSync();

        // Assert
        isReady.Should().BeTrue();
    }

    [Fact]
    public void IsReadyForSync_WhenDisabled_ReturnsFalse()
    {
        // Arrange
        var syncConfig = SyncConfigurationValue.Create(false, 60, 3, 300).Value;
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass", syncConfig).Value;

        // Act
        var isReady = account.IsReadyForSync();

        // Assert
        isReady.Should().BeFalse();
    }

    // Domain Event Tests
    [Fact]
    public void Create_LdcAccountCreatedEvent_ContainsCorrectData()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var provider = LdcProvider.SCE;
        var accountName = "SCE Test Account";

        // Act
        var account = LdcAccountAggregate.Create(tenantId, provider, accountName, "user", "pass").Value;

        // Assert
        var evt = account.DomainEvents.First() as LdcAccountCreatedEvent;
        evt.Should().NotBeNull();
        evt!.LdcAccountId.Should().Be(account.Id);
        evt.TenantId.Should().Be(tenantId.Value);
        evt.Provider.Should().Be(provider);
        evt.AccountName.Should().Be(accountName);
    }

    [Fact]
    public void UpdateCredentials_LdcAccountCredentialsUpdatedEvent_DoesNotIncludePassword()
    {
        // Arrange
        var account = LdcAccountAggregate.Create(CreateTenantId(), LdcProvider.PGE, "Test Account", "user", "pass").Value;
        account.ClearDomainEvents();

        // Act
        account.UpdateCredentials("new_user", "new_secret_pass");

        // Assert
        var evt = account.DomainEvents.First() as LdcAccountCredentialsUpdatedEvent;
        evt.Should().NotBeNull();
        evt!.Username.Should().Be("new_user");
        // Verify password is NOT in the event (security)
        evt.GetType().GetProperties().Should().NotContain(p => p.Name.Contains("Password", StringComparison.OrdinalIgnoreCase));
    }
}
