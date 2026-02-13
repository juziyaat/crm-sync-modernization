using CCA.Sync.Domain.Common;

namespace CCA.Sync.Domain.ValueObjects;

/// <summary>
/// Represents a tenant identifier as a value object.
/// </summary>
public sealed class TenantId : ValueObject, IEquatable<TenantId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TenantId"/> class.
    /// </summary>
    private TenantId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the tenant identifier value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Creates a tenant ID from a GUID.
    /// </summary>
    /// <param name="value">The GUID value</param>
    /// <returns>A result containing the tenant ID or an error</returns>
    public static Result<TenantId> Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            return Result<TenantId>.Failure(
                new Error("TenantId.Empty", "Tenant ID cannot be empty."));
        }

        return Result<TenantId>.Success(new TenantId(value));
    }

    /// <summary>
    /// Creates a tenant ID from a string representation of a GUID.
    /// </summary>
    /// <param name="value">The GUID string</param>
    /// <returns>A result containing the tenant ID or an error</returns>
    public static Result<TenantId> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<TenantId>.Failure(
                new Error("TenantId.Empty", "Tenant ID cannot be empty."));
        }

        if (!Guid.TryParse(value.Trim(), out var guid))
        {
            return Result<TenantId>.Failure(
                new Error("TenantId.InvalidFormat", "Tenant ID must be a valid GUID format."));
        }

        return Create(guid);
    }

    /// <summary>
    /// Generates a new random tenant ID.
    /// </summary>
    /// <returns>A new tenant ID</returns>
    public static TenantId New() => new(Guid.NewGuid());

    /// <summary>
    /// Gets the equality components for this tenant ID.
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Determines whether two tenant IDs are equal.
    /// </summary>
    public bool Equals(TenantId? other)
    {
        return other is not null && Value == other.Value;
    }

    /// <summary>
    /// Gets the string representation of this tenant ID.
    /// </summary>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// Converts a tenant ID to a GUID implicitly.
    /// </summary>
    public static implicit operator Guid(TenantId tenantId) => tenantId.Value;

    /// <summary>
    /// Converts a tenant ID to a string implicitly.
    /// </summary>
    public static implicit operator string(TenantId tenantId) => tenantId.Value.ToString();

    /// <summary>
    /// Determines whether two tenant IDs are equal.
    /// </summary>
    public static bool operator ==(TenantId? left, TenantId? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two tenant IDs are not equal.
    /// </summary>
    public static bool operator !=(TenantId? left, TenantId? right)
    {
        return !Equals(left, right);
    }
}
