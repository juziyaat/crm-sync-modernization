using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.ValueObjects;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when a new customer is created.
/// </summary>
public sealed class CustomerCreatedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerCreatedEvent"/> class.
    /// </summary>
    /// <param name="customerId">The ID of the created customer</param>
    /// <param name="tenantId">The ID of the tenant</param>
    /// <param name="customerName">The name of the customer</param>
    /// <param name="emailAddress">The email address of the customer</param>
    public CustomerCreatedEvent(Guid customerId, Guid tenantId, string customerName, string emailAddress)
    {
        CustomerId = customerId;
        TenantId = tenantId;
        CustomerName = customerName;
        EmailAddress = emailAddress;
    }

    /// <summary>
    /// Gets the ID of the created customer.
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets the ID of the tenant.
    /// </summary>
    public Guid TenantId { get; }

    /// <summary>
    /// Gets the name of the customer.
    /// </summary>
    public string CustomerName { get; }

    /// <summary>
    /// Gets the email address of the customer.
    /// </summary>
    public string EmailAddress { get; }
}
