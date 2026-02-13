namespace CCA.Sync.Domain.Specifications;

using System.Linq.Expressions;

/// <summary>
/// Base class for specifications.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public abstract class Specification<T> : ISpecification<T>
{
    private readonly List<Expression<Func<T, object>>> _includes = [];
    private readonly List<string> _includeStrings = [];

    /// <summary>
    /// Gets or sets the criteria expression for this specification.
    /// </summary>
    public Expression<Func<T, bool>>? Criteria { get; protected set; }

    /// <summary>
    /// Gets the includes for eager loading.
    /// </summary>
    public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes.AsReadOnly();

    /// <summary>
    /// Gets the string-based includes for related data.
    /// </summary>
    public IReadOnlyList<string> IncludeStrings => _includeStrings.AsReadOnly();

    /// <summary>
    /// Adds an include for eager loading.
    /// </summary>
    /// <param name="includeExpression">The include expression</param>
    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        ArgumentNullException.ThrowIfNull(includeExpression);
        _includes.Add(includeExpression);
    }

    /// <summary>
    /// Adds a string-based include for related data.
    /// </summary>
    /// <param name="includeString">The include string</param>
    protected virtual void AddIncludeString(string includeString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(includeString);
        _includeStrings.Add(includeString);
    }

    /// <summary>
    /// Determines whether this specification is satisfied by the given entity.
    /// </summary>
    /// <param name="entity">The entity to check</param>
    /// <returns>True if the specification is satisfied; otherwise, false</returns>
    public virtual bool IsSatisfiedBy(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (Criteria is null)
        {
            return true;
        }

        var compiledExpression = Criteria.Compile();
        return compiledExpression(entity);
    }
}
