namespace CCA.Sync.Domain.Specifications;

using System.Linq.Expressions;

/// <summary>
/// Composite specification that combines two specifications with AND logic.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public sealed class AndSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    /// <summary>
    /// Initializes a new instance of the <see cref="AndSpecification{T}"/> class.
    /// </summary>
    /// <param name="left">The left specification</param>
    /// <param name="right">The right specification</param>
    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        _left = left;
        _right = right;

        // Combine criteria expressions
        if (_left.Criteria is not null && _right.Criteria is not null)
        {
            var parameter = Expression.Parameter(typeof(T));
            var leftExpression = Expression.Invoke(_left.Criteria, parameter);
            var rightExpression = Expression.Invoke(_right.Criteria, parameter);
            var combined = Expression.AndAlso(leftExpression, rightExpression);

            Criteria = Expression.Lambda<Func<T, bool>>(combined, parameter);
        }
        else if (_left.Criteria is not null)
        {
            Criteria = _left.Criteria;
        }
        else if (_right.Criteria is not null)
        {
            Criteria = _right.Criteria;
        }

        // Add includes from both specifications
        foreach (var include in _left.Includes)
        {
            AddInclude(include);
        }

        foreach (var include in _right.Includes)
        {
            AddInclude(include);
        }

        foreach (var includeString in _left.IncludeStrings)
        {
            AddIncludeString(includeString);
        }

        foreach (var includeString in _right.IncludeStrings)
        {
            AddIncludeString(includeString);
        }
    }

    /// <summary>
    /// Determines whether both specifications are satisfied by the given entity.
    /// </summary>
    public override bool IsSatisfiedBy(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return _left.IsSatisfiedBy(entity) && _right.IsSatisfiedBy(entity);
    }
}
