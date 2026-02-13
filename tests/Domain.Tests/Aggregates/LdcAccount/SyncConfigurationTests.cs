using FluentAssertions;
using Xunit;
using SyncConfigurationValue = CCA.Sync.Domain.Aggregates.LdcAccount.SyncConfiguration;

namespace CCA.Sync.Domain.Tests.Aggregates.LdcAccount;

/// <summary>
/// Unit tests for the SyncConfiguration value object.
/// </summary>
public class SyncConfigurationTests
{
    [Fact]
    public void Create_WithValidParameters_Succeeds()
    {
        // Arrange & Act
        var result = SyncConfigurationValue.Create(true, 60, 3, 300);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var config = result.Value;
        config.IsEnabled.Should().BeTrue();
        config.SyncIntervalMinutes.Should().Be(60);
        config.MaxRetries.Should().Be(3);
        config.TimeoutSeconds.Should().Be(300);
    }

    [Fact]
    public void Create_WithDisabledSync_Succeeds()
    {
        // Arrange & Act
        var result = SyncConfigurationValue.Create(false, 60, 3, 300);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.IsEnabled.Should().BeFalse();
    }

    [Theory]
    [InlineData(4)]
    [InlineData(0)]
    [InlineData(-10)]
    public void Create_WithSyncIntervalBelowMinimum_Fails(int interval)
    {
        // Arrange & Act
        var result = SyncConfigurationValue.Create(true, interval, 3, 300);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncConfiguration.InvalidInterval");
        result.Error.Description.Should().Contain($"at least {SyncConfigurationValue.MinSyncIntervalMinutes}");
    }

    [Theory]
    [InlineData(1441)]
    [InlineData(2000)]
    public void Create_WithSyncIntervalAboveMaximum_Fails(int interval)
    {
        // Arrange & Act
        var result = SyncConfigurationValue.Create(true, interval, 3, 300);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncConfiguration.InvalidInterval");
        result.Error.Description.Should().Contain($"cannot exceed {SyncConfigurationValue.MaxSyncIntervalMinutes}");
    }

    [Theory]
    [InlineData(5)]
    [InlineData(60)]
    [InlineData(120)]
    [InlineData(1440)]
    public void Create_WithValidSyncInterval_Succeeds(int interval)
    {
        // Arrange & Act
        var result = SyncConfigurationValue.Create(true, interval, 3, 300);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SyncIntervalMinutes.Should().Be(interval);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-5)]
    public void Create_WithNegativeMaxRetries_Fails(int maxRetries)
    {
        // Arrange & Act
        var result = SyncConfigurationValue.Create(true, 60, maxRetries, 300);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncConfiguration.InvalidRetries");
        result.Error.Description.Should().Contain("cannot be negative");
    }

    [Theory]
    [InlineData(11)]
    [InlineData(20)]
    public void Create_WithMaxRetriesAboveLimit_Fails(int maxRetries)
    {
        // Arrange & Act
        var result = SyncConfigurationValue.Create(true, 60, maxRetries, 300);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncConfiguration.InvalidRetries");
        result.Error.Description.Should().Contain($"cannot exceed {SyncConfigurationValue.MaxRetryAttempts}");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(10)]
    public void Create_WithValidMaxRetries_Succeeds(int maxRetries)
    {
        // Arrange & Act
        var result = SyncConfigurationValue.Create(true, 60, maxRetries, 300);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.MaxRetries.Should().Be(maxRetries);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Create_WithInvalidTimeout_Fails(int timeout)
    {
        // Arrange & Act
        var result = SyncConfigurationValue.Create(true, 60, 3, timeout);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncConfiguration.InvalidTimeout");
        result.Error.Description.Should().Contain("greater than zero");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(60)]
    [InlineData(300)]
    [InlineData(600)]
    public void Create_WithValidTimeout_Succeeds(int timeout)
    {
        // Arrange & Act
        var result = SyncConfigurationValue.Create(true, 60, 3, timeout);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TimeoutSeconds.Should().Be(timeout);
    }

    [Fact]
    public void CreateDefault_ReturnsEnabledConfigurationWithDefaultValues()
    {
        // Arrange & Act
        var config = SyncConfigurationValue.CreateDefault();

        // Assert
        config.IsEnabled.Should().BeTrue();
        config.SyncIntervalMinutes.Should().Be(60);
        config.MaxRetries.Should().Be(3);
        config.TimeoutSeconds.Should().Be(300);
    }

    [Fact]
    public void CreateDisabled_ReturnsDisabledConfiguration()
    {
        // Arrange & Act
        var config = SyncConfigurationValue.CreateDisabled();

        // Assert
        config.IsEnabled.Should().BeFalse();
        config.SyncIntervalMinutes.Should().Be(60);
        config.MaxRetries.Should().Be(3);
        config.TimeoutSeconds.Should().Be(300);
    }

    [Fact]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        // Arrange
        var config1 = SyncConfigurationValue.Create(true, 60, 3, 300).Value;
        var config2 = SyncConfigurationValue.Create(true, 60, 3, 300).Value;

        // Act & Assert
        config1.Equals(config2).Should().BeTrue();
        (config1 == config2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentIsEnabled_ReturnsFalse()
    {
        // Arrange
        var config1 = SyncConfigurationValue.Create(true, 60, 3, 300).Value;
        var config2 = SyncConfigurationValue.Create(false, 60, 3, 300).Value;

        // Act & Assert
        config1.Equals(config2).Should().BeFalse();
        (config1 != config2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentSyncInterval_ReturnsFalse()
    {
        // Arrange
        var config1 = SyncConfigurationValue.Create(true, 60, 3, 300).Value;
        var config2 = SyncConfigurationValue.Create(true, 120, 3, 300).Value;

        // Act & Assert
        config1.Equals(config2).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentMaxRetries_ReturnsFalse()
    {
        // Arrange
        var config1 = SyncConfigurationValue.Create(true, 60, 3, 300).Value;
        var config2 = SyncConfigurationValue.Create(true, 60, 5, 300).Value;

        // Act & Assert
        config1.Equals(config2).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentTimeout_ReturnsFalse()
    {
        // Arrange
        var config1 = SyncConfigurationValue.Create(true, 60, 3, 300).Value;
        var config2 = SyncConfigurationValue.Create(true, 60, 3, 600).Value;

        // Act & Assert
        config1.Equals(config2).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithSameValues_ReturnsSameHashCode()
    {
        // Arrange
        var config1 = SyncConfigurationValue.Create(true, 60, 3, 300).Value;
        var config2 = SyncConfigurationValue.Create(true, 60, 3, 300).Value;

        // Act & Assert
        config1.GetHashCode().Should().Be(config2.GetHashCode());
    }
}
