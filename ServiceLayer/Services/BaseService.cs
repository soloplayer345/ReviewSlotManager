using AutoMapper;
using RepositoryLayer.Repositories;

namespace ServiceLayer.Services;

public class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    private readonly IBaseRepository<TEntity> _repository;
    private readonly IMapper _mapper;

    public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual async Task<List<TDto>> Read(int pageSize, int pageNumber)
    {
        var entities = await _repository.Read(pageSize, pageNumber);
        return _mapper.Map<List<TDto>>(entities);
    }

    public virtual async Task<TDto> Read(int id)
    {
        var entity = await _repository.Read(id) ?? throw new KeyNotFoundException("Entity not found.");
        return _mapper.Map<TDto>(entity);
    }
}
