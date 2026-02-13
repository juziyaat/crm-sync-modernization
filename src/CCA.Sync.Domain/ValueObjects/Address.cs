using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.ValueObjects;

/// <summary>
/// Represents a US address as a value object.
/// </summary>
public sealed class Address : ValueObject, IEquatable<Address>
{
    // US state codes (2 letters)
    private static readonly HashSet<string> ValidStates = new()
    {
        "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA",
        "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD",
        "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ",
        "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC",
        "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY",
        "DC", "AS", "GU", "MP", "PR", "UM", "VI"
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="Address"/> class.
    /// </summary>
    private Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    /// <summary>
    /// Gets the street address.
    /// </summary>
    public string Street { get; }

    /// <summary>
    /// Gets the city name.
    /// </summary>
    public string City { get; }

    /// <summary>
    /// Gets the state code (2 letters).
    /// </summary>
    public string State { get; }

    /// <summary>
    /// Gets the ZIP code.
    /// </summary>
    public string ZipCode { get; }

    /// <summary>
    /// Creates an address from component parts.
    /// </summary>
    /// <param name="street">The street address</param>
    /// <param name="city">The city name</param>
    /// <param name="state">The state code (2 letters)</param>
    /// <param name="zipCode">The ZIP code</param>
    /// <returns>A result containing the address or an error</returns>
    public static Result<Address> Create(string street, string city, string state, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street))
        {
            return Result<Address>.Failure(
                new Error("Address.StreetEmpty", "Street address cannot be empty."));
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            return Result<Address>.Failure(
                new Error("Address.CityEmpty", "City cannot be empty."));
        }

        if (string.IsNullOrWhiteSpace(state))
        {
            return Result<Address>.Failure(
                new Error("Address.StateEmpty", "State cannot be empty."));
        }

        var trimmedState = state.Trim().ToUpperInvariant();

        if (trimmedState.Length != 2 || !ValidStates.Contains(trimmedState))
        {
            return Result<Address>.Failure(
                new Error("Address.StateInvalid", "State must be a valid 2-letter US state code."));
        }

        if (string.IsNullOrWhiteSpace(zipCode))
        {
            return Result<Address>.Failure(
                new Error("Address.ZipCodeEmpty", "ZIP code cannot be empty."));
        }

        var trimmedZipCode = zipCode.Trim();

        // US ZIP code formats: 12345 or 12345-6789
        if (!System.Text.RegularExpressions.Regex.IsMatch(trimmedZipCode, @"^\d{5}(?:-\d{4})?$"))
        {
            return Result<Address>.Failure(
                new Error("Address.ZipCodeInvalid", "ZIP code must be in format 12345 or 12345-6789."));
        }

        return Result<Address>.Success(new Address(
            street.Trim(),
            city.Trim(),
            trimmedState,
            trimmedZipCode));
    }

    /// <summary>
    /// Gets the equality components for this address.
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return ZipCode;
    }

    /// <summary>
    /// Determines whether two addresses are equal.
    /// </summary>
    public bool Equals(Address? other)
    {
        return other is not null &&
               Street.Equals(other.Street, StringComparison.OrdinalIgnoreCase) &&
               City.Equals(other.City, StringComparison.OrdinalIgnoreCase) &&
               State == other.State &&
               ZipCode == other.ZipCode;
    }

    /// <summary>
    /// Gets the string representation of this address.
    /// </summary>
    public override string ToString() => $"{Street}, {City}, {State} {ZipCode}";

    /// <summary>
    /// Determines whether two addresses are equal.
    /// </summary>
    public static bool operator ==(Address? left, Address? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two addresses are not equal.
    /// </summary>
    public static bool operator !=(Address? left, Address? right)
    {
        return !Equals(left, right);
    }
}
