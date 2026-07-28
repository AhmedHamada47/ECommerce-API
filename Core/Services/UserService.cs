using Domain.Exceptions;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Dto.Auth;
using ServicesAbstractions;

namespace Services
{
    internal class UserService(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager) : IUserService
    {
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await userManager.Users.ToListAsync();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    DisplayName = user.DisplayName,
                    Roles = roles,
                    IsLockedOut = await userManager.IsLockedOutAsync(user)
                });
            }

            return userDtos;
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id)
                ?? throw new BusinessException(BusinessErrorCode.UserNotFound, id);

            var roles = await userManager.GetRolesAsync(user);
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Roles = roles,
                IsLockedOut = await userManager.IsLockedOutAsync(user)
            };
        }

        public async Task AssignRoleAsync(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId)
                ?? throw new BusinessException(BusinessErrorCode.UserNotFound, userId);

            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

            await userManager.AddToRoleAsync(user, role);
        }

        public async Task RemoveRoleAsync(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId)
                ?? throw new BusinessException(BusinessErrorCode.UserNotFound, userId);

            await userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task ToggleLockoutAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId)
                ?? throw new BusinessException(BusinessErrorCode.UserNotFound, userId);

            if (await userManager.IsLockedOutAsync(user))
                await userManager.SetLockoutEndDateAsync(user, null);
            else
                await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id)
                ?? throw new BusinessException(BusinessErrorCode.UserNotFound, id);

            await userManager.DeleteAsync(user);
        }
    }
}