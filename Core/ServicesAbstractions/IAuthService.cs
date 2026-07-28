using Shared.Dto.Auth;

namespace ServicesAbstractions
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> GetCurrentUserAsync(string userId);
    }
}
