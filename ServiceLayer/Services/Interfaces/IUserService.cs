using RepositoryLayer.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services.Interfaces;

public interface IUserService : IBaseService<User, UserDto>
{
    Task<int> Count();
    Task<UserDto> Create(CreateUserDto dto);
    Task<UserDto> Update(int id, UpdateUserDto dto);
}
