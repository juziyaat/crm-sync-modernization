using System.Text.RegularExpressions;
using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.ValueObjects;

/// <summary>
/// Represents an email address as a value object.
/// </summary>
public sealed class EmailAddress : ValueObject, IEquatable<EmailAddress>
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    );

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailAddress"/> class.
    /// </summary>
    /// <param name="value">The email address string</param>
    private EmailAddress(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the email address value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates an email address from a string.
    /// </summary>
    /// <param name="value">The email address string</param>
    /// <returns>A result containing the email address or an error</returns>
    public static Result<EmailAddress> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<EmailAddress>.Failure(
                new Error("EmailAddress.Empty", "Email address cannot be empty."));
        }

        var trimmedValue = value.Trim();

        if (trimmedValue.Length > 254)
        {
            return Result<EmailAddress>.Failure(
                new Error("EmailAddress.TooLong", "Email address cannot exceed 254 characters."));
        }

        if (!EmailRegex.IsMatch(trimmedValue))
        {
            return Result<EmailAddress>.Failure(
                new Error("EmailAddress.Invalid", "Email address format is invalid."));
        }

        return Result<EmailAddress>.Success(new EmailAddress(trimmedValue));
    }

    /// <summary>
    /// Gets the equality components for this email address.
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Determines whether two email addresses are equal.
    /// </summary>
    public bool Equals(EmailAddress? other)
    {
        return other is not null && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the string representation of this email address.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Converts an email address to a string implicitly.
    /// </summary>
    public static implicit operator string(EmailAddress emailAddress) => emailAddress.Value;

    /// <summary>
    /// Determines whether two email addresses are equal.
    /// </summary>
    public static bool operator ==(EmailAddress? left, EmailAddress? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two email addresses are not equal.
    /// </summary>
    public static bool operator !=(EmailAddress? left, EmailAddress? right)
    {
        return !Equals(left, right);
    }
}
