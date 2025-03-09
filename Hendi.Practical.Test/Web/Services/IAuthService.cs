using Application.DTOs;

namespace Web.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(RegisterRequestDto dto);
        Task<string> LoginAsync(AuthRequestDto dto);
    }
}
