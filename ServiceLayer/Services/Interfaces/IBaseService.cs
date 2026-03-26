namespace ServiceLayer.Services.Interfaces;

public interface IBaseService<TDto>
    where TDto : class
{
    Task<List<TDto>> Read(int pageSize, int pageNumber);
    Task<TDto> Read(int id);
}

public interface IBaseService<TEntity, TDto> : IBaseService<TDto>
    where TEntity : class
    where TDto : class
{
    Task<TDto> Create(TDto dto);
    Task<TDto> Update(int id, TDto dto);
    Task Delete(int id);
}
