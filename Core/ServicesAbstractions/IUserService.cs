using Shared.Dto.Auth;

namespace ServicesAbstractions
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(string id);
        Task AssignRoleAsync(string userId, string role);
        Task RemoveRoleAsync(string userId, string role);
        Task ToggleLockoutAsync(string userId);
        Task DeleteUserAsync(string id);
    }
}
