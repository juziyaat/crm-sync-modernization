namespace CCA.Sync.Domain.Common;

/// <summary>
/// Base class for value objects. Provides property-based equality.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Gets the equality components that define value object equality.
    /// </summary>
    /// <returns>An enumerable of objects that comprise the value object's identity</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>
    /// Determines whether two value objects are equal based on their components.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is ValueObject valueObject &&
               ValueObjectsEqual(this, valueObject);
    }

    /// <summary>
    /// Determines whether two value objects are equal based on their components.
    /// </summary>
    public bool Equals(ValueObject? other)
    {
        return other is not null && ValueObjectsEqual(this, other);
    }

    /// <summary>
    /// Gets the hash code based on the equality components.
    /// </summary>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(default(int), (hashcode, value) =>
            {
                var valueHashCode = value?.GetHashCode() ?? 0;
                return HashCode.Combine(hashcode, valueHashCode);
            });
    }

    /// <summary>
    /// Determines whether two value objects are equal.
    /// </summary>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two value objects are not equal.
    /// </summary>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }

    private static bool ValueObjectsEqual(ValueObject left, ValueObject right)
    {
        return left.GetEqualityComponents()
            .SequenceEqual(right.GetEqualityComponents());
    }
}
