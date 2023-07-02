using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class Repository<TEntity> : Base.Repository<TEntity> where TEntity : Domain.Base.BaseEntity
{
    internal Repository(DbContext databaseContext) : base(databaseContext)
    {
    }

    public override async Task<bool> RemoveByIdAsync
        (object id, CancellationToken cancellationToken = default)
    {
        TEntity? entity = await DbSet
            .Where(current => current.Id == Convert.ToInt32(id))
            .FirstOrDefaultAsync();

        if (entity is null)
        {
            throw new ArgumentNullException(paramName: nameof(id));
        }

        entity.IsDeleted = true;

        return true;
    }

    public override async Task RemoveAsync
        (TEntity? entity, CancellationToken cancellationToken = default)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(paramName: nameof(entity));
        }

        TEntity? searchEntity =
            await DbSet.Where(current => current.Id == Convert.ToInt32(entity.Id))
            .FirstOrDefaultAsync();

        if (searchEntity is null)
        {
            throw new ArgumentNullException(paramName: nameof(searchEntity));
        }

        searchEntity.IsDeleted = true;
    }

    public override async Task RemoveRangeAsync
        (IEnumerable<TEntity?> entities, CancellationToken cancellationToken = default)
    {
        foreach (var item in entities)
        {
            await RemoveAsync(item);
        }
    }

    public override async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
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

    public override async Task<IEnumerable<TEntity?>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result =
            await DbSet
            .Where(current => current.IsDeleted == false)
            .ToListAsync(cancellationToken: cancellationToken);

        return result;
    }

    public override async Task<TEntity?> FindAsync(object id, CancellationToken cancellationToken = default)
    {
        var result = await DbSet
            .Where(current => current.IsDeleted == false)
            .Where(current => current.Id == Convert.ToInt32(id))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return result;
    }

}