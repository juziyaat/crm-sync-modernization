using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.Events;

/// <summary>
/// Raised when a customer's contact information is updated.
/// </summary>
public sealed class CustomerContactInfoUpdatedEvent : DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerContactInfoUpdatedEvent"/> class.
    /// </summary>
    /// <param name="customerId">The ID of the customer</param>
    /// <param name="emailAddress">The updated email address</param>
    /// <param name="phoneNumber">The updated phone number</param>
    /// <param name="street">The updated street address</param>
    /// <param name="city">The updated city</param>
    /// <param name="state">The updated state</param>
    /// <param name="zipCode">The updated ZIP code</param>
    public CustomerContactInfoUpdatedEvent(
        Guid customerId,
        string? emailAddress,
        string? phoneNumber,
        string? street,
        string? city,
        string? state,
        string? zipCode)
    {
        CustomerId = customerId;
        EmailAddress = emailAddress;
        PhoneNumber = phoneNumber;
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    /// <summary>
    /// Gets the ID of the customer.
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets the updated email address (if provided).
    /// </summary>
    public string? EmailAddress { get; }

    /// <summary>
    /// Gets the updated phone number (if provided).
    /// </summary>
    public string? PhoneNumber { get; }

    /// <summary>
    /// Gets the updated street address (if provided).
    /// </summary>
    public string? Street { get; }

    /// <summary>
    /// Gets the updated city (if provided).
    /// </summary>
    public string? City { get; }

    /// <summary>
    /// Gets the updated state (if provided).
    /// </summary>
    public string? State { get; }

    /// <summary>
    /// Gets the updated ZIP code (if provided).
    /// </summary>
    public string? ZipCode { get; }
}
