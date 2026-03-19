namespace RepositoryLayer.Repositories;

public interface IBaseRepository<TEntity>
    where TEntity : class
{
    Task<List<TEntity>> Read(int pageSize = 20, int pageNumber = 1);
    Task<TEntity?> Read(int id);
    Task<TEntity> Create(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(int id);
}
