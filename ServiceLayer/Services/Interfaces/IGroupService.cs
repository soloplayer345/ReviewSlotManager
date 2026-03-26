using RepositoryLayer.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services.Interfaces;

public interface IGroupService : IBaseService<Group, GroupDto>
{
    Task<int> Count();
    Task<List<GroupDto>> GetBySemester(int semesterId);
    Task<GroupDto> Create(CreateGroupDto dto);
    Task<GroupDto> Update(int id, UpdateGroupDto dto);
}
