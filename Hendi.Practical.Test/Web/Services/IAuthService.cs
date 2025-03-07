using Application.DTOs;

namespace Web.Services
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(RegisterRequestDto dto);
        Task<string> LoginAsync(AuthRequestDto dto);
    }
}
