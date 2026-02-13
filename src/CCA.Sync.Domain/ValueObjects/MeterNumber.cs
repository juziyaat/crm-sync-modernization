using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.ValueObjects;

/// <summary>
/// Represents a utility meter number as a value object.
/// </summary>
public sealed class MeterNumber : ValueObject, IEquatable<MeterNumber>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MeterNumber"/> class.
    /// </summary>
    private MeterNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the meter number value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a meter number from a string.
    /// </summary>
    /// <param name="value">The meter number string</param>
    /// <returns>A result containing the meter number or an error</returns>
    public static Result<MeterNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<MeterNumber>.Failure(
                new Error("MeterNumber.Empty", "Meter number cannot be empty."));
        }

        var trimmedValue = value.Trim();

        // Meter numbers typically contain alphanumeric characters
        // Length should be between 6 and 30 characters
        if (trimmedValue.Length < 6 || trimmedValue.Length > 30)
        {
            return Result<MeterNumber>.Failure(
                new Error("MeterNumber.InvalidLength", "Meter number must be between 6 and 30 characters."));
        }

        // Validate that it contains only alphanumeric characters
        if (!System.Text.RegularExpressions.Regex.IsMatch(trimmedValue, @"^[a-zA-Z0-9]+$"))
        {
            return Result<MeterNumber>.Failure(
                new Error("MeterNumber.InvalidFormat", "Meter number can only contain alphanumeric characters."));
        }

        return Result<MeterNumber>.Success(new MeterNumber(trimmedValue));
    }

    /// <summary>
    /// Gets the equality components for this meter number.
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Determines whether two meter numbers are equal.
    /// </summary>
    public bool Equals(MeterNumber? other)
    {
        return other is not null && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the string representation of this meter number.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Converts a meter number to a string implicitly.
    /// </summary>
    public static implicit operator string(MeterNumber meterNumber) => meterNumber.Value;

    /// <summary>
    /// Determines whether two meter numbers are equal.
    /// </summary>
    public static bool operator ==(MeterNumber? left, MeterNumber? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two meter numbers are not equal.
    /// </summary>
    public static bool operator !=(MeterNumber? left, MeterNumber? right)
    {
        return !Equals(left, right);
    }
}
