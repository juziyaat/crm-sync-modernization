using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.Enums;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when a new sync job is created.
/// </summary>
public sealed class SyncJobCreatedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyncJobCreatedEvent"/> class.
    /// </summary>
    /// <param name="syncJobId">The ID of the created sync job</param>
    /// <param name="tenantId">The ID of the tenant</param>
    /// <param name="jobType">The type of sync job</param>
    /// <param name="ldcAccountId">The optional LDC account ID associated with the job</param>
    public SyncJobCreatedEvent(Guid syncJobId, Guid tenantId, SyncJobType jobType, Guid? ldcAccountId)
    {
        SyncJobId = syncJobId;
        TenantId = tenantId;
        JobType = jobType;
        LdcAccountId = ldcAccountId;
    }

    /// <summary>
    /// Gets the ID of the created sync job.
    /// </summary>
    public Guid SyncJobId { get; }

    /// <summary>
    /// Gets the ID of the tenant.
    /// </summary>
    public Guid TenantId { get; }

    /// <summary>
    /// Gets the type of sync job.
    /// </summary>
    public SyncJobType JobType { get; }

    /// <summary>
    /// Gets the optional LDC account ID associated with the job.
    /// </summary>
    public Guid? LdcAccountId { get; }
}
