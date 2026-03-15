using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;

namespace RepositoryLayer.Repositories;

public class BaseRepository<TEntity>
    where TEntity : class
{
    private readonly ReviewSlotDbContext _context;
    private readonly DbSet<TEntity> _source;
    private readonly Expression<Func<TEntity, int>> _idSelector;

    public BaseRepository(ReviewSlotDbContext context, Expression<Func<TEntity, int>> idSelector)
    {
        _context = context;
        _source = context.Set<TEntity>();
        _idSelector = idSelector;
    }

    public virtual async Task<List<TEntity>> Read(int pageSize = 20, int pageNumber = 1)
    {
        var skip = Math.Max(0, (pageNumber - 1) * pageSize);
        return await _source.AsNoTracking().Skip(skip).Take(pageSize).ToListAsync();
    }

    public virtual async Task<TEntity?> Read(int id)
    {
        return await _source.AsNoTracking().FirstOrDefaultAsync(BuildIdPredicate(id));
    }

    public virtual async Task<TEntity> Create(TEntity entity)
    {
        await _source.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task Update(TEntity entity)
    {
        _source.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task Delete(int id)
    {
        var entity = await _source.FirstOrDefaultAsync(BuildIdPredicate(id));
        if (entity is null)
        {
            return;
        }

        _source.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private Expression<Func<TEntity, bool>> BuildIdPredicate(int id)
    {
        var parameter = _idSelector.Parameters[0];
        var body = Expression.Equal(_idSelector.Body, Expression.Constant(id));
        return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
    }
}
