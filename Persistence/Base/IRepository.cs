using Domain;
using System.Linq.Expressions;

namespace Persistence.Base;

public interface IRepository<TEntity>
{
	Task AddAsync(TEntity? entity, CancellationToken cancellationToken = default);
	Task AddRangeAsync(IEnumerable<TEntity?> entities, CancellationToken cancellationToken = default);
	Task RemoveAsync(TEntity? entity, CancellationToken cancellationToken = default);
	Task<bool> RemoveByIdAsync(object id, CancellationToken cancellationToken = default);
	Task RemoveRangeAsync(IEnumerable<TEntity?> entities, CancellationToken cancellationToken = default);
	Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
	Task<IEnumerable<TEntity?>> GetAllAsync(CancellationToken cancellationToken = default);

	Task<IEnumerable<TEntity?>> Find(Expression<Func<TEntity?, bool>> predicate,
		CancellationToken cancellationToken = default);
	Task<TEntity?> FindAsync(object id, CancellationToken cancellationToken = default);
}