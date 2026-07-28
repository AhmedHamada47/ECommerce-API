using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Domain.Exceptions;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Dto.Auth;
using ServicesAbstractions;

namespace Services
{
    internal class AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IMapper mapper,
        IConfiguration configuration) : IAuthService
    {
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
                throw new BusinessException(BusinessErrorCode.EmailAlreadyRegistered);

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new BusinessException(BusinessErrorCode.BadRequest, string.Join("; ", errors));
            }

            await userManager.AddToRoleAsync(user, "User");

            var token = await GenerateTokenAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Token = token,
                Roles = roles
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                throw new BusinessException(BusinessErrorCode.InvalidCredentials);

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                throw new BusinessException(BusinessErrorCode.InvalidCredentials);

            var token = await GenerateTokenAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Token = token,
                Roles = roles
            };
        }

        public async Task<AuthResponseDto> GetCurrentUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId)
                ?? throw new BusinessException(BusinessErrorCode.UserNotFound, userId);

            var token = await GenerateTokenAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Token = token,
                Roles = roles
            };
        }

        private async Task<string> GenerateTokenAsync(AppUser user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.GivenName, user.DisplayName)
            };

            var roles = await userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}