using ServiceLayer.DTOs;

namespace ServiceLayer.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}
