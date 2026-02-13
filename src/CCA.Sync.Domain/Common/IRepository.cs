namespace CCA.Sync.Domain.Common;

using CCA.Sync.Domain.Specifications;

/// <summary>
/// Generic repository interface for aggregate roots.
/// </summary>
/// <typeparam name="TAggregate">The type of the aggregate root</typeparam>
/// <typeparam name="TId">The type of the aggregate root identifier</typeparam>
public interface IRepository<TAggregate, in TId>
    where TAggregate : AggregateRoot<TId>
    where TId : notnull
{
    /// <summary>
    /// Gets an aggregate root by its identifier.
    /// </summary>
    /// <param name="id">The aggregate root identifier</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The aggregate root or null if not found</returns>
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all aggregate roots matching a specification.
    /// </summary>
    /// <param name="specification">The specification</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The aggregate roots matching the specification</returns>
    Task<IReadOnlyList<TAggregate>> GetBySpecificationAsync(
        ISpecification<TAggregate> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds an aggregate root.
    /// </summary>
    /// <param name="aggregate">The aggregate root to add</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an aggregate root.
    /// </summary>
    /// <param name="aggregate">The aggregate root to update</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an aggregate root.
    /// </summary>
    /// <param name="aggregate">The aggregate root to delete</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
}
