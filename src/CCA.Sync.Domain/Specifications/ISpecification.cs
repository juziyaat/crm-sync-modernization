namespace CCA.Sync.Domain.Specifications;

using System.Linq.Expressions;

/// <summary>
/// Interface for specifications used with the Specification pattern.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Gets the criteria expression for this specification.
    /// </summary>
    Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// Gets the includes for eager loading.
    /// </summary>
    IReadOnlyList<Expression<Func<T, object>>> Includes { get; }

    /// <summary>
    /// Gets the string-based includes for related data.
    /// </summary>
    IReadOnlyList<string> IncludeStrings { get; }

    /// <summary>
    /// Gets a value indicating whether to track changes for this specification.
    /// </summary>
    bool IsSatisfiedBy(T entity);
}
