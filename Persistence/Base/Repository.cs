using System.Linq.Expressions;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Base
{
	public abstract class Repository<TEntity> :
		IRepository<TEntity> where TEntity : Domain.Base.BaseEntity
	{
		protected Repository
			(DbContext databaseContext) : base()
		{
			DatabaseContext =
				databaseContext ??
				throw new ArgumentNullException(paramName: nameof(databaseContext));

			DbSet =
				DatabaseContext.Set<TEntity>();
		}

		// **********
		protected DbSet<TEntity> DbSet { get; }
		// **********

		// **********
		protected DbContext DatabaseContext { get; }
		// **********

		public virtual async Task AddAsync(TEntity? entity, CancellationToken cancellationToken = default)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(paramName: nameof(entity));
			}

			await DbSet.AddAsync
				(entity: entity, cancellationToken: cancellationToken);
		}

		public virtual async Task
			AddRangeAsync(IEnumerable<TEntity?> entities, CancellationToken cancellationToken = default)
		{
			if (entities == null)
			{
				throw new ArgumentNullException(paramName: nameof(entities));
			}

			await DbSet.AddRangeAsync
				(entities: entities!, cancellationToken: cancellationToken);
		}

		public virtual async Task
			RemoveAsync(TEntity? entity, CancellationToken cancellationToken = default)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(paramName: nameof(entity));
			}

			await Task.Run(() =>
			{
				DbSet.Remove(entity: entity);

				//var attachedEntity =
				//	DatabaseContext.Attach(entity: entity);

				//attachedEntity.State =
				//	Microsoft.EntityFrameworkCore.EntityState.Deleted;
			}, cancellationToken: cancellationToken);
		}

		public virtual async Task<bool>
			RemoveByIdAsync(object id, CancellationToken cancellationToken = default)
		{
			TEntity? entity =
				await FindAsync(id: id, cancellationToken: cancellationToken);

			await RemoveAsync
				(entity: entity, cancellationToken: cancellationToken);

			return true;
		}

		public virtual async Task
			RemoveRangeAsync(IEnumerable<TEntity?> entities, CancellationToken cancellationToken = default)
		{
			if (entities == null)
			{
				throw new ArgumentNullException(paramName: nameof(entities));
			}

			foreach (var entity in entities)
			{
				await RemoveAsync
					(entity: entity, cancellationToken: cancellationToken);
			}
		}

		public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
		{
			await Task.Run(() =>
			{
				var attachedEntity = DatabaseContext.Attach(entity: entity);

				if (attachedEntity.State != EntityState.Modified)
				{
					attachedEntity.State = EntityState.Modified;
				}
			}, cancellationToken: cancellationToken);
		}

		public virtual async Task<IEnumerable<TEntity?>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			// ToListAsync -> Extension Method -> using Microsoft.EntityFrameworkCore;
			var result =
				await DbSet.ToListAsync(cancellationToken: cancellationToken);

			return result;
		}

		public virtual async Task<IEnumerable<TEntity?>> Find(Expression<Func<TEntity?, bool>> predicate, CancellationToken cancellationToken = default)
		{
			// ToListAsync -> Extension Method -> using Microsoft.EntityFrameworkCore;
			var result =
					await DbSet.Where(predicate: predicate)
						.ToListAsync(cancellationToken: cancellationToken);

			return result;
		}

		public virtual async Task<TEntity?> FindAsync(object id, CancellationToken cancellationToken = default)
		{
			var result =
				await DbSet.FindAsync(keyValues: new[] { id }, cancellationToken: cancellationToken);

			return result;
		}

		//public Task<IEnumerable<TEntity>>
		//    GetSomeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		//{
		//    throw new NotImplementedException();
		//}

		//public Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default)
		//{
		//    throw new NotImplementedException();
		//}
	}
}
