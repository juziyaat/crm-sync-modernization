using CCA.Sync.Domain.Aggregates.SyncJob;
using FluentAssertions;
using Xunit;

namespace CCA.Sync.Domain.Tests.Aggregates.SyncJob;

/// <summary>
/// Unit tests for the SyncJobStatistics value object.
/// </summary>
public class SyncJobStatisticsTests
{
    [Fact]
    public void Create_WithValidParameters_Succeeds()
    {
        // Act
        var result = SyncJobStatistics.Create(100, 50, 40, 5, 5);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalRecords.Should().Be(100);
        result.Value.ProcessedRecords.Should().Be(50);
        result.Value.SuccessfulRecords.Should().Be(40);
        result.Value.FailedRecords.Should().Be(5);
        result.Value.SkippedRecords.Should().Be(5);
    }

    [Fact]
    public void Create_WithNegativeTotalRecords_Fails()
    {
        // Act
        var result = SyncJobStatistics.Create(-1, 0, 0, 0, 0);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJobStatistics.InvalidTotalRecords");
    }

    [Fact]
    public void Create_WithNegativeProcessedRecords_Fails()
    {
        // Act
        var result = SyncJobStatistics.Create(100, -1, 0, 0, 0);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJobStatistics.InvalidProcessedRecords");
    }

    [Fact]
    public void Create_WithNegativeSuccessfulRecords_Fails()
    {
        // Act
        var result = SyncJobStatistics.Create(100, 50, -1, 0, 0);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJobStatistics.InvalidSuccessfulRecords");
    }

    [Fact]
    public void Create_WithNegativeFailedRecords_Fails()
    {
        // Act
        var result = SyncJobStatistics.Create(100, 50, 0, -1, 0);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJobStatistics.InvalidFailedRecords");
    }

    [Fact]
    public void Create_WithNegativeSkippedRecords_Fails()
    {
        // Act
        var result = SyncJobStatistics.Create(100, 50, 0, 0, -1);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJobStatistics.InvalidSkippedRecords");
    }

    [Fact]
    public void Create_WithProcessedExceedingTotal_Fails()
    {
        // Act
        var result = SyncJobStatistics.Create(100, 101, 101, 0, 0);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJobStatistics.ProcessedExceedsTotal");
    }

    [Fact]
    public void Create_WithDetailExceedingProcessed_Fails()
    {
        // Act (successful + failed + skipped = 60, but processed is only 50)
        var result = SyncJobStatistics.Create(100, 50, 30, 20, 10);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJobStatistics.DetailExceedsProcessed");
    }

    [Fact]
    public void CreateInitial_WithValidTotalRecords_Succeeds()
    {
        // Act
        var result = SyncJobStatistics.CreateInitial(100);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalRecords.Should().Be(100);
        result.Value.ProcessedRecords.Should().Be(0);
        result.Value.SuccessfulRecords.Should().Be(0);
        result.Value.FailedRecords.Should().Be(0);
        result.Value.SkippedRecords.Should().Be(0);
    }

    [Fact]
    public void CreateEmpty_ReturnsZeroStatistics()
    {
        // Act
        var stats = SyncJobStatistics.CreateEmpty();

        // Assert
        stats.TotalRecords.Should().Be(0);
        stats.ProcessedRecords.Should().Be(0);
        stats.SuccessfulRecords.Should().Be(0);
        stats.FailedRecords.Should().Be(0);
        stats.SkippedRecords.Should().Be(0);
        stats.ProgressPercentage.Should().Be(0);
    }

    [Fact]
    public void ProgressPercentage_CalculatesCorrectly()
    {
        // Act
        var stats = SyncJobStatistics.Create(200, 100, 90, 5, 5).Value;

        // Assert
        stats.ProgressPercentage.Should().Be(50);
    }

    [Fact]
    public void ProgressPercentage_WithZeroTotal_ReturnsZero()
    {
        // Act
        var stats = SyncJobStatistics.CreateEmpty();

        // Assert
        stats.ProgressPercentage.Should().Be(0);
    }

    [Fact]
    public void ProgressPercentage_At100Percent_ReturnsCorrectValue()
    {
        // Act
        var stats = SyncJobStatistics.Create(100, 100, 100, 0, 0).Value;

        // Assert
        stats.ProgressPercentage.Should().Be(100);
    }

    [Fact]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        // Arrange
        var stats1 = SyncJobStatistics.Create(100, 50, 40, 5, 5).Value;
        var stats2 = SyncJobStatistics.Create(100, 50, 40, 5, 5).Value;

        // Assert
        stats1.Should().Be(stats2);
    }

    [Fact]
    public void Equals_WithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var stats1 = SyncJobStatistics.Create(100, 50, 40, 5, 5).Value;
        var stats2 = SyncJobStatistics.Create(100, 60, 50, 5, 5).Value;

        // Assert
        stats1.Should().NotBe(stats2);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var stats = SyncJobStatistics.Create(100, 50, 40, 5, 5).Value;

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("Total: 100");
        result.Should().Contain("Processed: 50");
        result.Should().Contain("Successful: 40");
        result.Should().Contain("Failed: 5");
        result.Should().Contain("Skipped: 5");
        result.Should().Contain("50%");
    }
}
