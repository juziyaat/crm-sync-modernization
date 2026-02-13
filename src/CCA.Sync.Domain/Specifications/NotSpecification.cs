namespace CCA.Sync.Domain.Specifications;

using System.Linq.Expressions;

/// <summary>
/// Composite specification that negates another specification.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public sealed class NotSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _specification;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotSpecification{T}"/> class.
    /// </summary>
    /// <param name="specification">The specification to negate</param>
    public NotSpecification(ISpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(specification);

        _specification = specification;

        // Negate the criteria expression
        if (_specification.Criteria is not null)
        {
            var parameter = Expression.Parameter(typeof(T));
            var innerExpression = Expression.Invoke(_specification.Criteria, parameter);
            var negated = Expression.Not(innerExpression);

            Criteria = Expression.Lambda<Func<T, bool>>(negated, parameter);
        }

        // Add includes from the original specification
        foreach (var include in _specification.Includes)
        {
            AddInclude(include);
        }

        foreach (var includeString in _specification.IncludeStrings)
        {
            AddIncludeString(includeString);
        }
    }

    /// <summary>
    /// Determines whether the negation of the specification is satisfied by the given entity.
    /// </summary>
    public override bool IsSatisfiedBy(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return !_specification.IsSatisfiedBy(entity);
    }
}
