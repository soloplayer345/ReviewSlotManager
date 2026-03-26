using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services.Interfaces;
using AutoMapper;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;
using RepositoryLayer.Repositories;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public class UserService : BaseService<User, UserDto>, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
        : base(userRepository, mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public Task<int> Count()
    {
        return _userRepository.Count();
    }

    public async Task<UserDto> Create(CreateUserDto dto)
    {
        var existingUser = await _userRepository.GetByEmail(dto.Email);
        if (existingUser is not null)
            throw new InvalidOperationException($"Email '{dto.Email}' is already in use.");

        if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
            throw new ArgumentException($"Invalid role '{dto.Role}'.");

        var entity = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = role,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _userRepository.Create(entity);
        return _mapper.Map<UserDto>(created);
    }

    public async Task<UserDto> Update(int id, UpdateUserDto dto)
    {
        var existing = await _userRepository.Read(id)
            ?? throw new KeyNotFoundException($"User {id} not found.");

        if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
            throw new ArgumentException($"Invalid role '{dto.Role}'.");

        existing.FullName = dto.FullName;
        existing.Email = dto.Email;
        existing.Role = role;
        existing.UpdatedAt = DateTime.UtcNow;

        await _userRepository.Update(existing);
        return _mapper.Map<UserDto>(existing);
    }
}
