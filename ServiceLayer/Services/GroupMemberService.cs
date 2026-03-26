using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services.Interfaces;
using AutoMapper;
using RepositoryLayer.Entities;
using RepositoryLayer.Repositories;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public class GroupMemberService : BaseService<GroupMember, GroupMemberDto>, IGroupMemberService
{
    private readonly IGroupMemberRepository _groupMemberRepository;
    private readonly IMapper _mapper;

    public GroupMemberService(IGroupMemberRepository groupMemberRepository, IMapper mapper)
        : base(groupMemberRepository, mapper)
    {
        _groupMemberRepository = groupMemberRepository;
        _mapper = mapper;
    }

    public Task<int> Count()
    {
        return _groupMemberRepository.Count();
    }

    public async Task<List<GroupMemberDto>> GetByGroup(int groupId)
    {
        var members = await _groupMemberRepository.GetByGroup(groupId);
        return _mapper.Map<List<GroupMemberDto>>(members);
    }

    public async Task<GroupMemberDto> Create(CreateGroupMemberDto dto)
    {
        var entity = _mapper.Map<GroupMember>(dto);
        entity.JoinedAt = DateTime.UtcNow;
        var created = await _groupMemberRepository.Create(entity);
        return _mapper.Map<GroupMemberDto>(created);
    }
}
