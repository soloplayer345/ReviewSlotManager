namespace ServiceLayer.Services;

public interface IBaseService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    Task<List<TDto>> Read(int pageSize, int pageNumber);
    Task<TDto> Read(int id);
}
