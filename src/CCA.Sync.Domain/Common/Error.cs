namespace CCA.Sync.Domain.Common;

/// <summary>
/// Represents an error with a code and description.
/// </summary>
public sealed class Error : ValueObject, IEquatable<Error>
{
    /// <summary>
    /// Gets an empty error instance.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="code">The error code</param>
    /// <param name="description">The error description</param>
    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the equality components for this error.
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Code;
        yield return Description;
    }

    /// <summary>
    /// Gets the string representation of this error.
    /// </summary>
    public override string ToString()
    {
        return $"{Code}: {Description}";
    }

    /// <summary>
    /// Determines whether two errors are equal.
    /// </summary>
    public bool Equals(Error? other)
    {
        return other is not null &&
               Code == other.Code &&
               Description == other.Description;
    }
}
