using CCA.Sync.Domain.Enums;
using CCA.Sync.Domain.Events;
using CCA.Sync.Domain.ValueObjects;
using FluentAssertions;
using Xunit;
using SyncJobAggregate = CCA.Sync.Domain.Aggregates.SyncJob.SyncJob;
using SyncJobStats = CCA.Sync.Domain.Aggregates.SyncJob.SyncJobStatistics;

namespace CCA.Sync.Domain.Tests.Aggregates.SyncJob;

/// <summary>
/// Unit tests for the SyncJob aggregate root.
/// </summary>
public class SyncJobTests
{
    // Helper methods
    private static TenantId CreateTenantId()
    {
        return TenantId.Create(Guid.NewGuid()).Value;
    }

    private static SyncJobAggregate CreatePendingJob(SyncJobType jobType = SyncJobType.FullSync, Guid? ldcAccountId = null)
    {
        return SyncJobAggregate.Create(CreateTenantId(), jobType, ldcAccountId).Value;
    }

    private static SyncJobAggregate CreateRunningJob(int totalRecords = 100)
    {
        var job = CreatePendingJob();
        job.Start(totalRecords);
        job.ClearDomainEvents();
        return job;
    }

    // Creation Tests
    [Fact]
    public void Create_WithValidParameters_SucceedsAndRaisesEvent()
    {
        // Arrange
        var tenantId = CreateTenantId();

        // Act
        var result = SyncJobAggregate.Create(tenantId, SyncJobType.FullSync);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var job = result.Value;
        job.Id.Should().NotBe(Guid.Empty);
        job.TenantId.Should().Be(tenantId);
        job.JobType.Should().Be(SyncJobType.FullSync);
        job.Status.Should().Be(SyncJobStatus.Pending);
        job.LdcAccountId.Should().BeNull();
        job.CorrelationId.Should().BeNull();
        job.Statistics.Should().NotBeNull();
        job.Statistics.TotalRecords.Should().Be(0);
        job.Errors.Should().BeEmpty();
        job.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        job.StartedAt.Should().BeNull();
        job.CompletedAt.Should().BeNull();
        job.DomainEvents.Should().HaveCount(1);
        job.DomainEvents.First().Should().BeOfType<SyncJobCreatedEvent>();
    }

    [Fact]
    public void Create_WithLdcAccountId_SetsLdcAccountId()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var ldcAccountId = Guid.NewGuid();

        // Act
        var result = SyncJobAggregate.Create(tenantId, SyncJobType.AccountSync, ldcAccountId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.LdcAccountId.Should().Be(ldcAccountId);
        result.Value.JobType.Should().Be(SyncJobType.AccountSync);
    }

    [Fact]
    public void Create_WithCorrelationId_SetsCorrelationId()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var correlationId = Guid.NewGuid();

        // Act
        var result = SyncJobAggregate.Create(tenantId, SyncJobType.IncrementalSync, correlationId: correlationId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.CorrelationId.Should().Be(correlationId);
    }

    [Fact]
    public void Create_WithEmptyLdcAccountId_Fails()
    {
        // Arrange
        var tenantId = CreateTenantId();

        // Act
        var result = SyncJobAggregate.Create(tenantId, SyncJobType.AccountSync, Guid.Empty);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.InvalidLdcAccountId");
    }

    [Fact]
    public void Create_WithEmptyCorrelationId_Fails()
    {
        // Arrange
        var tenantId = CreateTenantId();

        // Act
        var result = SyncJobAggregate.Create(tenantId, SyncJobType.FullSync, correlationId: Guid.Empty);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.InvalidCorrelationId");
    }

    [Fact]
    public void Create_WithNullTenantId_ThrowsArgumentNullException()
    {
        // Act
        var act = () => SyncJobAggregate.Create(null!, SyncJobType.FullSync);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Create_SyncJobCreatedEvent_ContainsCorrectData()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var ldcAccountId = Guid.NewGuid();

        // Act
        var job = SyncJobAggregate.Create(tenantId, SyncJobType.AccountSync, ldcAccountId).Value;

        // Assert
        var evt = job.DomainEvents.First() as SyncJobCreatedEvent;
        evt.Should().NotBeNull();
        evt!.SyncJobId.Should().Be(job.Id);
        evt.TenantId.Should().Be(tenantId.Value);
        evt.JobType.Should().Be(SyncJobType.AccountSync);
        evt.LdcAccountId.Should().Be(ldcAccountId);
    }

    // Start Tests
    [Fact]
    public void Start_WhenPending_SucceedsAndRaisesEvent()
    {
        // Arrange
        var job = CreatePendingJob();
        job.ClearDomainEvents();

        // Act
        var result = job.Start(100);

        // Assert
        result.IsSuccess.Should().BeTrue();
        job.Status.Should().Be(SyncJobStatus.Running);
        job.StartedAt.Should().NotBeNull();
        job.StartedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        job.Statistics.TotalRecords.Should().Be(100);
        job.Statistics.ProcessedRecords.Should().Be(0);
        job.DomainEvents.Should().HaveCount(1);
        job.DomainEvents.First().Should().BeOfType<SyncJobStartedEvent>();
    }

    [Fact]
    public void Start_WhenAlreadyRunning_Fails()
    {
        // Arrange
        var job = CreateRunningJob();

        // Act
        var result = job.Start(50);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.InvalidStateTransition");
    }

    [Fact]
    public void Start_WhenCompleted_Fails()
    {
        // Arrange
        var job = CreateRunningJob();
        var stats = SyncJobStats.Create(100, 100, 100, 0, 0).Value;
        job.Complete(stats);

        // Act
        var result = job.Start(50);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.InvalidStateTransition");
    }

    [Fact]
    public void Start_WithNegativeTotalRecords_Fails()
    {
        // Arrange
        var job = CreatePendingJob();

        // Act
        var result = job.Start(-1);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.InvalidTotalRecords");
    }

    [Fact]
    public void Start_WithZeroTotalRecords_Succeeds()
    {
        // Arrange
        var job = CreatePendingJob();

        // Act
        var result = job.Start(0);

        // Assert
        result.IsSuccess.Should().BeTrue();
        job.Statistics.TotalRecords.Should().Be(0);
    }

    // UpdateProgress Tests
    [Fact]
    public void UpdateProgress_WhenRunning_SucceedsAndRaisesEvent()
    {
        // Arrange
        var job = CreateRunningJob(100);
        var stats = SyncJobStats.Create(100, 50, 45, 3, 2).Value;

        // Act
        var result = job.UpdateProgress(stats);

        // Assert
        result.IsSuccess.Should().BeTrue();
        job.Statistics.Should().Be(stats);
        job.DomainEvents.Should().HaveCount(1);
        var evt = job.DomainEvents.First() as SyncJobProgressUpdatedEvent;
        evt.Should().NotBeNull();
        evt!.ProcessedRecords.Should().Be(50);
        evt.TotalRecords.Should().Be(100);
        evt.ProgressPercentage.Should().Be(50);
    }

    [Fact]
    public void UpdateProgress_WhenNotRunning_Fails()
    {
        // Arrange
        var job = CreatePendingJob();

        // Act
        var stats = SyncJobStats.Create(100, 50, 50, 0, 0).Value;
        var result = job.UpdateProgress(stats);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.InvalidStateTransition");
    }

    [Fact]
    public void UpdateProgress_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        var job = CreateRunningJob();

        // Act
        var act = () => job.UpdateProgress(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    // RecordError Tests
    [Fact]
    public void RecordError_WhenRunning_SucceedsAndAddsError()
    {
        // Arrange
        var job = CreateRunningJob();

        // Act
        var result = job.RecordError("CONN_TIMEOUT", "Connection timed out", "ACCT-123");

        // Assert
        result.IsSuccess.Should().BeTrue();
        job.Errors.Should().HaveCount(1);
        job.Errors.First().ErrorCode.Should().Be("CONN_TIMEOUT");
        job.Errors.First().ErrorMessage.Should().Be("Connection timed out");
        job.Errors.First().RecordIdentifier.Should().Be("ACCT-123");
        job.HasErrors().Should().BeTrue();
    }

    [Fact]
    public void RecordError_WhenNotRunning_Fails()
    {
        // Arrange
        var job = CreatePendingJob();

        // Act
        var result = job.RecordError("ERR", "Some error");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.InvalidStateTransition");
    }

    [Fact]
    public void RecordError_WithMultipleErrors_TracksAllErrors()
    {
        // Arrange
        var job = CreateRunningJob();

        // Act
        job.RecordError("ERR_1", "First error", "REC-1");
        job.RecordError("ERR_2", "Second error", "REC-2");
        job.RecordError("ERR_3", "Third error");

        // Assert
        job.Errors.Should().HaveCount(3);
    }

    // Complete Tests
    [Fact]
    public void Complete_WhenRunning_SucceedsAndRaisesEvent()
    {
        // Arrange
        var job = CreateRunningJob(100);
        var finalStats = SyncJobStats.Create(100, 100, 100, 0, 0).Value;

        // Act
        var result = job.Complete(finalStats);

        // Assert
        result.IsSuccess.Should().BeTrue();
        job.Status.Should().Be(SyncJobStatus.Completed);
        job.CompletedAt.Should().NotBeNull();
        job.CompletedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        job.Statistics.Should().Be(finalStats);
        job.IsTerminal().Should().BeTrue();
        job.DomainEvents.Should().HaveCount(1);
        var evt = job.DomainEvents.First() as SyncJobCompletedEvent;
        evt.Should().NotBeNull();
        evt!.TotalRecords.Should().Be(100);
        evt.SuccessfulRecords.Should().Be(100);
    }

    [Fact]
    public void Complete_WhenNotRunning_Fails()
    {
        // Arrange
        var job = CreatePendingJob();
        var stats = SyncJobStats.Create(100, 100, 100, 0, 0).Value;

        // Act
        var result = job.Complete(stats);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.InvalidStateTransition");
    }

    [Fact]
    public void Complete_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        var job = CreateRunningJob();

        // Act
        var act = () => job.Complete(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    // Fail Tests
    [Fact]
    public void Fail_WhenRunning_SucceedsAndRaisesEvent()
    {
        // Arrange
        var job = CreateRunningJob();

        // Act
        var result = job.Fail("Database connection lost");

        // Assert
        result.IsSuccess.Should().BeTrue();
        job.Status.Should().Be(SyncJobStatus.Failed);
        job.CompletedAt.Should().NotBeNull();
        job.IsTerminal().Should().BeTrue();
        job.DomainEvents.Should().HaveCount(1);
        var evt = job.DomainEvents.First() as SyncJobFailedEvent;
        evt.Should().NotBeNull();
        evt!.Reason.Should().Be("Database connection lost");
    }

    [Fact]
    public void Fail_WhenNotRunning_Fails()
    {
        // Arrange
        var job = CreatePendingJob();

        // Act
        var result = job.Fail("Some reason");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.InvalidStateTransition");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Fail_WithInvalidReason_Fails(string? reason)
    {
        // Arrange
        var job = CreateRunningJob();

        // Act
        var result = job.Fail(reason!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.InvalidFailureReason");
    }

    // Cancel Tests
    [Fact]
    public void Cancel_WhenPending_SucceedsAndRaisesEvent()
    {
        // Arrange
        var job = CreatePendingJob();
        job.ClearDomainEvents();

        // Act
        var result = job.Cancel("No longer needed");

        // Assert
        result.IsSuccess.Should().BeTrue();
        job.Status.Should().Be(SyncJobStatus.Cancelled);
        job.CompletedAt.Should().NotBeNull();
        job.IsTerminal().Should().BeTrue();
        job.DomainEvents.Should().HaveCount(1);
        var evt = job.DomainEvents.First() as SyncJobCancelledEvent;
        evt.Should().NotBeNull();
        evt!.Reason.Should().Be("No longer needed");
    }

    [Fact]
    public void Cancel_WhenRunning_SucceedsAndRaisesEvent()
    {
        // Arrange
        var job = CreateRunningJob();

        // Act
        var result = job.Cancel("User requested cancellation");

        // Assert
        result.IsSuccess.Should().BeTrue();
        job.Status.Should().Be(SyncJobStatus.Cancelled);
        job.IsTerminal().Should().BeTrue();
        job.DomainEvents.Should().HaveCount(1);
        job.DomainEvents.First().Should().BeOfType<SyncJobCancelledEvent>();
    }

    [Fact]
    public void Cancel_WhenCompleted_Fails()
    {
        // Arrange
        var job = CreateRunningJob();
        var stats = SyncJobStats.Create(100, 100, 100, 0, 0).Value;
        job.Complete(stats);
        job.ClearDomainEvents();

        // Act
        var result = job.Cancel("Too late");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.InvalidStateTransition");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Cancel_WithInvalidReason_Fails(string? reason)
    {
        // Arrange
        var job = CreatePendingJob();

        // Act
        var result = job.Cancel(reason!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.InvalidCancellationReason");
    }

    // CompletePartially Tests
    [Fact]
    public void CompletePartially_WhenRunningWithFailedRecords_SucceedsAndRaisesEvent()
    {
        // Arrange
        var job = CreateRunningJob(100);
        var stats = SyncJobStats.Create(100, 100, 85, 10, 5).Value;

        // Act
        var result = job.CompletePartially(stats);

        // Assert
        result.IsSuccess.Should().BeTrue();
        job.Status.Should().Be(SyncJobStatus.PartiallyCompleted);
        job.CompletedAt.Should().NotBeNull();
        job.Statistics.Should().Be(stats);
        job.IsTerminal().Should().BeTrue();
        job.DomainEvents.Should().HaveCount(1);
        var evt = job.DomainEvents.First() as SyncJobCompletedEvent;
        evt.Should().NotBeNull();
        evt!.TotalRecords.Should().Be(100);
        evt.SuccessfulRecords.Should().Be(85);
    }

    [Fact]
    public void CompletePartially_WithZeroFailedRecords_Fails()
    {
        // Arrange
        var job = CreateRunningJob(100);
        var stats = SyncJobStats.Create(100, 100, 100, 0, 0).Value;

        // Act
        var result = job.CompletePartially(stats);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.NoFailedRecords");
    }

    [Fact]
    public void CompletePartially_WhenNotRunning_Fails()
    {
        // Arrange
        var job = CreatePendingJob();
        var stats = SyncJobStats.Create(100, 100, 90, 10, 0).Value;

        // Act
        var result = job.CompletePartially(stats);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SyncJob.InvalidStateTransition");
    }

    // GetDuration Tests
    [Fact]
    public void GetDuration_WhenNotStarted_ReturnsNull()
    {
        // Arrange
        var job = CreatePendingJob();

        // Act
        var duration = job.GetDuration();

        // Assert
        duration.Should().BeNull();
    }

    [Fact]
    public void GetDuration_WhenRunning_ReturnsElapsedTime()
    {
        // Arrange
        var job = CreatePendingJob();
        job.Start(100);

        // Act
        var duration = job.GetDuration();

        // Assert
        duration.Should().NotBeNull();
        duration!.Value.Should().BeGreaterOrEqualTo(TimeSpan.Zero);
    }

    [Fact]
    public void GetDuration_WhenCompleted_ReturnsFixedDuration()
    {
        // Arrange
        var job = CreateRunningJob(100);
        var stats = SyncJobStats.Create(100, 100, 100, 0, 0).Value;
        job.Complete(stats);

        // Act
        var duration = job.GetDuration();

        // Assert
        duration.Should().NotBeNull();
        duration!.Value.Should().BeGreaterOrEqualTo(TimeSpan.Zero);
    }

    // IsTerminal Tests
    [Fact]
    public void IsTerminal_WhenPending_ReturnsFalse()
    {
        // Arrange
        var job = CreatePendingJob();

        // Assert
        job.IsTerminal().Should().BeFalse();
    }

    [Fact]
    public void IsTerminal_WhenRunning_ReturnsFalse()
    {
        // Arrange
        var job = CreateRunningJob();

        // Assert
        job.IsTerminal().Should().BeFalse();
    }

    // HasErrors Tests
    [Fact]
    public void HasErrors_WithNoErrors_ReturnsFalse()
    {
        // Arrange
        var job = CreateRunningJob();

        // Assert
        job.HasErrors().Should().BeFalse();
    }

    [Fact]
    public void HasErrors_WithErrors_ReturnsTrue()
    {
        // Arrange
        var job = CreateRunningJob();
        job.RecordError("ERR", "Test error");

        // Assert
        job.HasErrors().Should().BeTrue();
    }

    // Full Lifecycle Tests
    [Fact]
    public void FullLifecycle_SuccessfulSync_TransitionsThroughAllStates()
    {
        // Arrange
        var tenantId = CreateTenantId();
        var ldcAccountId = Guid.NewGuid();
        var correlationId = Guid.NewGuid();

        // Create
        var job = SyncJobAggregate.Create(tenantId, SyncJobType.AccountSync, ldcAccountId, correlationId).Value;
        job.Status.Should().Be(SyncJobStatus.Pending);

        // Start
        job.Start(50);
        job.Status.Should().Be(SyncJobStatus.Running);

        // Update progress
        var midStats = SyncJobStats.Create(50, 25, 25, 0, 0).Value;
        job.UpdateProgress(midStats);
        job.Statistics.ProgressPercentage.Should().Be(50);

        // Complete
        var finalStats = SyncJobStats.Create(50, 50, 50, 0, 0).Value;
        job.Complete(finalStats);
        job.Status.Should().Be(SyncJobStatus.Completed);
        job.IsTerminal().Should().BeTrue();

        // Verify all events were raised
        job.DomainEvents.Should().HaveCount(4);
        job.DomainEvents.ElementAt(0).Should().BeOfType<SyncJobCreatedEvent>();
        job.DomainEvents.ElementAt(1).Should().BeOfType<SyncJobStartedEvent>();
        job.DomainEvents.ElementAt(2).Should().BeOfType<SyncJobProgressUpdatedEvent>();
        job.DomainEvents.ElementAt(3).Should().BeOfType<SyncJobCompletedEvent>();
    }

    [Fact]
    public void FullLifecycle_FailedSync_TransitionsCorrectly()
    {
        // Create and start
        var job = CreatePendingJob();
        job.Start(100);

        // Record some errors
        job.RecordError("PARSE_ERR", "Failed to parse response", "REC-42");
        job.RecordError("CONN_ERR", "Connection refused");

        // Fail the job
        job.Fail("Too many errors encountered");

        // Verify
        job.Status.Should().Be(SyncJobStatus.Failed);
        job.HasErrors().Should().BeTrue();
        job.Errors.Should().HaveCount(2);
        job.IsTerminal().Should().BeTrue();
    }
}
