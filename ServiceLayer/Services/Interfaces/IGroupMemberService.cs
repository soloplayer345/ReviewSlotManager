using RepositoryLayer.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services.Interfaces;

public interface IGroupMemberService : IBaseService<GroupMember, GroupMemberDto>
{
    Task<int> Count();
    Task<List<GroupMemberDto>> GetByGroup(int groupId);
    Task<GroupMemberDto> Create(CreateGroupMemberDto dto);
}
