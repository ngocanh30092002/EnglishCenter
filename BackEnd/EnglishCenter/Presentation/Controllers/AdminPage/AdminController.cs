using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AdminPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AppRole.ADMIN)]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var response = await _userService.GetAllAsync(userId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("users/{userId}/password")]
        public async Task<IActionResult> ChangePasswordAsync([FromRoute] string userId, [FromBody] string newPassword)
        {
            var response = await _userService.ChangePasswordAsync(userId, newPassword);
            return await response.ChangeActionAsync();
        }

        [HttpGet("users/roles")]
        public async Task<IActionResult> GetUsersWithRolesAsync()
        {
            var response = await _userService.GetUsersWithRolesAsync();
            return await response.ChangeActionAsync();
        }

        [HttpPut("users")]
        public async Task<IActionResult> UpdateUserAsync(UserDto model)
        {
            var response = await _userService.UpdateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string userId)
        {
            var response = await _userService.DeleteAsync(userId);
            return await response.ChangeActionAsync();
        }

    }
}
