using System.Text.RegularExpressions;
using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.ValueObjects;

/// <summary>
/// Represents a US phone number as a value object.
/// </summary>
public sealed class PhoneNumber : ValueObject, IEquatable<PhoneNumber>
{
    // Matches various US phone number formats: (123) 456-7890, 123-456-7890, 1234567890, +1-123-456-7890, etc.
    private static readonly Regex PhoneNumberRegex = new(
        @"^(\+?1[-.\s]?)?\(?([2-9]{1}[0-9]{2})\)?[-.\s]?([2-9]{1}[0-9]{2})[-.\s]?([0-9]{4})$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    );

    /// <summary>
    /// Initializes a new instance of the <see cref="PhoneNumber"/> class.
    /// </summary>
    /// <param name="value">The normalized phone number (10 digits)</param>
    private PhoneNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the normalized phone number (10 digits, no formatting).
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a phone number from a string.
    /// </summary>
    /// <param name="value">The phone number string (supports various formats)</param>
    /// <returns>A result containing the phone number or an error</returns>
    public static Result<PhoneNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<PhoneNumber>.Failure(
                new Error("PhoneNumber.Empty", "Phone number cannot be empty."));
        }

        var trimmedValue = value.Trim();

        if (!PhoneNumberRegex.IsMatch(trimmedValue))
        {
            return Result<PhoneNumber>.Failure(
                new Error("PhoneNumber.Invalid", "Phone number format is invalid. Expected US phone number format."));
        }

        // Extract digits only
        var digitsOnly = Regex.Replace(trimmedValue, @"\D", "");

        // If it starts with 1 (country code), remove it
        if (digitsOnly.StartsWith('1') && digitsOnly.Length == 11)
        {
            digitsOnly = digitsOnly[1..];
        }

        // Ensure we have exactly 10 digits
        if (digitsOnly.Length != 10)
        {
            return Result<PhoneNumber>.Failure(
                new Error("PhoneNumber.Invalid", "Phone number must contain exactly 10 digits."));
        }

        return Result<PhoneNumber>.Success(new PhoneNumber(digitsOnly));
    }

    /// <summary>
    /// Formats the phone number for display as (XXX) XXX-XXXX.
    /// </summary>
    /// <returns>The formatted phone number</returns>
    public string FormatDisplay() => $"({Value[..3]}) {Value[3..6]}-{Value[6..]}";

    /// <summary>
    /// Formats the phone number in E.164 format as +1XXXXXXXXXX.
    /// </summary>
    /// <returns>The E.164 formatted phone number</returns>
    public string FormatE164() => $"+1{Value}";

    /// <summary>
    /// Gets the equality components for this phone number.
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Determines whether two phone numbers are equal.
    /// </summary>
    public bool Equals(PhoneNumber? other)
    {
        return other is not null && Value == other.Value;
    }

    /// <summary>
    /// Gets the string representation of this phone number (display format).
    /// </summary>
    public override string ToString() => FormatDisplay();

    /// <summary>
    /// Converts a phone number to a string implicitly (normalized 10-digit format).
    /// </summary>
    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;

    /// <summary>
    /// Determines whether two phone numbers are equal.
    /// </summary>
    public static bool operator ==(PhoneNumber? left, PhoneNumber? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two phone numbers are not equal.
    /// </summary>
    public static bool operator !=(PhoneNumber? left, PhoneNumber? right)
    {
        return !Equals(left, right);
    }
}
