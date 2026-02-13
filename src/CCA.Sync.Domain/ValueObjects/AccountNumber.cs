using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.ValueObjects;

/// <summary>
/// Represents a utility account number as a value object.
/// </summary>
public sealed class AccountNumber : ValueObject, IEquatable<AccountNumber>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AccountNumber"/> class.
    /// </summary>
    private AccountNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the account number value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates an account number from a string.
    /// </summary>
    /// <param name="value">The account number string</param>
    /// <returns>A result containing the account number or an error</returns>
    public static Result<AccountNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<AccountNumber>.Failure(
                new Error("AccountNumber.Empty", "Account number cannot be empty."));
        }

        var trimmedValue = value.Trim();

        // Account numbers typically contain alphanumeric characters, hyphens, and underscores
        // Length should be between 3 and 50 characters
        if (trimmedValue.Length < 3 || trimmedValue.Length > 50)
        {
            return Result<AccountNumber>.Failure(
                new Error("AccountNumber.InvalidLength", "Account number must be between 3 and 50 characters."));
        }

        // Validate that it contains only alphanumeric characters, hyphens, and underscores
        if (!System.Text.RegularExpressions.Regex.IsMatch(trimmedValue, @"^[a-zA-Z0-9\-_]+$"))
        {
            return Result<AccountNumber>.Failure(
                new Error("AccountNumber.InvalidFormat", "Account number can only contain alphanumeric characters, hyphens, and underscores."));
        }

        return Result<AccountNumber>.Success(new AccountNumber(trimmedValue));
    }

    /// <summary>
    /// Gets the equality components for this account number.
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Determines whether two account numbers are equal.
    /// </summary>
    public bool Equals(AccountNumber? other)
    {
        return other is not null && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the string representation of this account number.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Converts an account number to a string implicitly.
    /// </summary>
    public static implicit operator string(AccountNumber accountNumber) => accountNumber.Value;

    /// <summary>
    /// Determines whether two account numbers are equal.
    /// </summary>
    public static bool operator ==(AccountNumber? left, AccountNumber? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two account numbers are not equal.
    /// </summary>
    public static bool operator !=(AccountNumber? left, AccountNumber? right)
    {
        return !Equals(left, right);
    }
}
