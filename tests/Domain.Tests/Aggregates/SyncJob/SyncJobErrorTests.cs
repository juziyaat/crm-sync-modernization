using CCA.Sync.Domain.Aggregates.SyncJob;
using FluentAssertions;
using Xunit;

namespace CCA.Sync.Domain.Tests.Aggregates.SyncJob;

/// <summary>
/// Unit tests for the SyncJobError entity.
/// </summary>
public class SyncJobErrorTests
{
    [Fact]
    public void Create_WithValidParameters_Succeeds()
    {
        // Act
        var result = SyncJobError.Create("CONN_TIMEOUT", "Connection timed out", "ACCT-123");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBe(Guid.Empty);
        result.Value.ErrorCode.Should().Be("CONN_TIMEOUT");
        result.Value.ErrorMessage.Should().Be("Connection timed out");
        result.Value.RecordIdentifier.Should().Be("ACCT-123");
        result.Value.OccurredAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_WithoutRecordIdentifier_Succeeds()
    {
        // Act
        var result = SyncJobError.Create("GENERAL_ERR", "Something went wrong");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.RecordIdentifier.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidErrorCode_Fails(string? errorCode)
    {
        // Act
        var result = SyncJobError.Create(errorCode!, "Some message");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJobError.InvalidErrorCode");
        result.Error.Description.Should().Contain("cannot be empty");
    }

    [Fact]
    public void Create_WithErrorCodeExceedingMaxLength_Fails()
    {
        // Act
        var result = SyncJobError.Create(new string('A', 101), "Some message");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJobError.InvalidErrorCode");
        result.Error.Description.Should().Contain("cannot exceed 100 characters");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidErrorMessage_Fails(string? errorMessage)
    {
        // Act
        var result = SyncJobError.Create("ERR_CODE", errorMessage!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJobError.InvalidErrorMessage");
        result.Error.Description.Should().Contain("cannot be empty");
    }

    [Fact]
    public void Create_WithErrorMessageExceedingMaxLength_Fails()
    {
        // Act
        var result = SyncJobError.Create("ERR_CODE", new string('A', 2001));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJobError.InvalidErrorMessage");
        result.Error.Description.Should().Contain("cannot exceed 2000 characters");
    }

    [Fact]
    public void Create_WithRecordIdentifierExceedingMaxLength_Fails()
    {
        // Act
        var result = SyncJobError.Create("ERR_CODE", "Some message", new string('A', 501));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJobError.InvalidRecordIdentifier");
        result.Error.Description.Should().Contain("cannot exceed 500 characters");
    }

    [Fact]
    public void Create_TrimsValues()
    {
        // Act
        var result = SyncJobError.Create("  CONN_ERR  ", "  Connection failed  ", "  REC-1  ");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.ErrorCode.Should().Be("CONN_ERR");
        result.Value.ErrorMessage.Should().Be("Connection failed");
        result.Value.RecordIdentifier.Should().Be("REC-1");
    }
}
