using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dto.Auth;
using ServicesAbstractions;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            var response = await serviceManager.AuthService.RegisterAsync(registerDto);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var response = await serviceManager.AuthService.LoginAsync(loginDto);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<AuthResponseDto>> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var response = await serviceManager.AuthService.GetCurrentUserAsync(userId);
            return Ok(response);
        }
    }
}