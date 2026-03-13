namespace RepositoryLayer.Repositories;

public class BaseRepository<TEntity>
    where TEntity : class
{
    private readonly List<TEntity> _source;
    private readonly Func<TEntity, int> _idSelector;
    private readonly Action<TEntity, int> _idSetter;

    public BaseRepository(List<TEntity> source, Func<TEntity, int> idSelector, Action<TEntity, int> idSetter)
    {
        _source = source;
        _idSelector = idSelector;
        _idSetter = idSetter;
    }

    public virtual Task<List<TEntity>> Read(int pageSize = 20, int pageNumber = 1)
    {
        var skip = Math.Max(0, (pageNumber - 1) * pageSize);
        var result = _source.Skip(skip).Take(pageSize).ToList();
        return Task.FromResult(result);
    }

    public virtual Task<TEntity?> Read(int id)
    {
        var entity = _source.FirstOrDefault(x => _idSelector(x) == id);
        return Task.FromResult(entity);
    }

    public virtual Task<TEntity> Create(TEntity entity)
    {
        var newId = _source.Select(_idSelector).DefaultIfEmpty(0).Max() + 1;
        _idSetter(entity, newId);
        _source.Add(entity);
        return Task.FromResult(entity);
    }

    public virtual Task Update(TEntity entity)
    {
        var id = _idSelector(entity);
        var index = _source.FindIndex(x => _idSelector(x) == id);
        if (index >= 0)
        {
            _source[index] = entity;
        }

        return Task.CompletedTask;
    }

    public virtual Task Delete(int id)
    {
        _source.RemoveAll(x => _idSelector(x) == id);
        return Task.CompletedTask;
    }
}
