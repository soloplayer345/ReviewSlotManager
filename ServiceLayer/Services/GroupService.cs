using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services.Interfaces;
using AutoMapper;
using RepositoryLayer.Entities;
using RepositoryLayer.Repositories;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public class GroupService : BaseService<Group, GroupDto>, IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;

    public GroupService(IGroupRepository groupRepository, IMapper mapper)
        : base(groupRepository, mapper)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    public Task<int> Count()
    {
        return _groupRepository.Count();
    }

    public async Task<List<GroupDto>> GetBySemester(int semesterId)
    {
        var groups = await _groupRepository.GetBySemester(semesterId);
        return _mapper.Map<List<GroupDto>>(groups);
    }

    public async Task<GroupDto> Create(CreateGroupDto dto)
    {
        var entity = _mapper.Map<Group>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        var created = await _groupRepository.Create(entity);
        return _mapper.Map<GroupDto>(created);
    }

    public async Task<GroupDto> Update(int id, UpdateGroupDto dto)
    {
        var existing = await _groupRepository.Read(id)
            ?? throw new KeyNotFoundException($"Group {id} not found.");
        existing.GroupName = dto.GroupName;
        existing.ProjectTitle = dto.ProjectTitle;
        existing.GvhdId = dto.GvhdId;
        existing.UpdatedAt = DateTime.UtcNow;
        await _groupRepository.Update(existing);
        return _mapper.Map<GroupDto>(existing);
    }
}
