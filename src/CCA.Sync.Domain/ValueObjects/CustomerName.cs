using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.ValueObjects;

/// <summary>
/// Represents a customer's name as a value object.
/// </summary>
public sealed class CustomerName : ValueObject, IEquatable<CustomerName>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerName"/> class.
    /// </summary>
    private CustomerName(string firstName, string lastName, string? middleName)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    /// <summary>
    /// Gets the first name.
    /// </summary>
    public string FirstName { get; }

    /// <summary>
    /// Gets the last name.
    /// </summary>
    public string LastName { get; }

    /// <summary>
    /// Gets the middle name, if provided.
    /// </summary>
    public string? MiddleName { get; }

    /// <summary>
    /// Gets the full name (FirstName [MiddleName] LastName).
    /// </summary>
    public string FullName
    {
        get
        {
            if (string.IsNullOrWhiteSpace(MiddleName))
            {
                return $"{FirstName} {LastName}";
            }

            return $"{FirstName} {MiddleName} {LastName}";
        }
    }

    /// <summary>
    /// Creates a customer name from component parts.
    /// </summary>
    /// <param name="firstName">The first name</param>
    /// <param name="lastName">The last name</param>
    /// <param name="middleName">The middle name (optional)</param>
    /// <returns>A result containing the customer name or an error</returns>
    public static Result<CustomerName> Create(string firstName, string lastName, string? middleName = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result<CustomerName>.Failure(
                new Error("CustomerName.FirstNameEmpty", "First name cannot be empty."));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result<CustomerName>.Failure(
                new Error("CustomerName.LastNameEmpty", "Last name cannot be empty."));
        }

        var trimmedFirstName = firstName.Trim();
        var trimmedLastName = lastName.Trim();
        var trimmedMiddleName = middleName?.Trim();

        if (trimmedFirstName.Length > 100)
        {
            return Result<CustomerName>.Failure(
                new Error("CustomerName.FirstNameTooLong", "First name cannot exceed 100 characters."));
        }

        if (trimmedLastName.Length > 100)
        {
            return Result<CustomerName>.Failure(
                new Error("CustomerName.LastNameTooLong", "Last name cannot exceed 100 characters."));
        }

        if (!string.IsNullOrEmpty(trimmedMiddleName) && trimmedMiddleName.Length > 100)
        {
            return Result<CustomerName>.Failure(
                new Error("CustomerName.MiddleNameTooLong", "Middle name cannot exceed 100 characters."));
        }

        return Result<CustomerName>.Success(new CustomerName(
            trimmedFirstName,
            trimmedLastName,
            trimmedMiddleName));
    }

    /// <summary>
    /// Gets the equality components for this customer name.
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return MiddleName;
    }

    /// <summary>
    /// Determines whether two customer names are equal.
    /// </summary>
    public bool Equals(CustomerName? other)
    {
        return other is not null &&
               FirstName.Equals(other.FirstName, StringComparison.OrdinalIgnoreCase) &&
               LastName.Equals(other.LastName, StringComparison.OrdinalIgnoreCase) &&
               (MiddleName ?? "").Equals(other.MiddleName ?? "", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the string representation of this customer name (full name).
    /// </summary>
    public override string ToString() => FullName;

    /// <summary>
    /// Converts a customer name to a string implicitly (full name).
    /// </summary>
    public static implicit operator string(CustomerName name) => name.FullName;

    /// <summary>
    /// Determines whether two customer names are equal.
    /// </summary>
    public static bool operator ==(CustomerName? left, CustomerName? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two customer names are not equal.
    /// </summary>
    public static bool operator !=(CustomerName? left, CustomerName? right)
    {
        return !Equals(left, right);
    }
}
