using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers.AdminPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepo;

        public RolesController(IRoleRepository roleRepo)  
        {
            _roleRepo = roleRepo;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("exist/{roleName}")]
        public async Task<bool> IsExistRoleAsync(string roleName)
        {
            return await _roleRepo.IsExistRoleAsync(roleName);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public async Task<IActionResult> GetRolesAsync()
        {
            var response = await _roleRepo.GetRolesAsync();

            return await response.ChangeActionAsync();
        }

        [Authorize]
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserRolesAsync([FromRoute] string userId)
        {
            var response = await _roleRepo.GetUserRolesAsync(userId);

            return await response.ChangeActionAsync();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public async Task<bool> CreateRoleAsync([FromBody] string roleName)
        {
            return await _roleRepo.CreateRoleAsync(roleName);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("users/{userId}")]
        public async Task<bool> AddUserRoleAsync([FromRoute] string userId, [FromBody] string roleName)
        {
            return await _roleRepo.AddUserRoleAsync(userId, roleName);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{roleName}")]
        public async Task<bool>  DeleteRoleAsync([FromRoute] string roleName)
        {
            return await _roleRepo.DeleteRoleAsync(roleName);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{userId}")]
        public async Task<bool> DeleteUserRolesAsync([FromRoute] string userId,[FromBody] string roleName)
        {
            return await _roleRepo.DeleteUserRolesAsync(userId, roleName);
        }
    }
}
