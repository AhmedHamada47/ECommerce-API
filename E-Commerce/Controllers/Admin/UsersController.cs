using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;

namespace E_Commerce.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shared.Dto.Auth.UserDto>>> GetAllUsers()
        {
            var users = await serviceManager.UserService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shared.Dto.Auth.UserDto>> GetUser(string id)
        {
            var user = await serviceManager.UserService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPost("{id}/roles")]
        public async Task<ActionResult> AssignRole(string id, [FromBody] string role)
        {
            await serviceManager.UserService.AssignRoleAsync(id, role);
            return NoContent();
        }

        [HttpDelete("{id}/roles")]
        public async Task<ActionResult> RemoveRole(string id, [FromBody] string role)
        {
            await serviceManager.UserService.RemoveRoleAsync(id, role);
            return NoContent();
        }

        [HttpPost("{id}/lockout")]
        public async Task<ActionResult> ToggleLockout(string id)
        {
            await serviceManager.UserService.ToggleLockoutAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            await serviceManager.UserService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}