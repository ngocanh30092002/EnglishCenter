﻿using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AdminPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpGet("exist/{roleName}")]
        public async Task<bool> IsExistRoleAsync(string roleName)
        {
            return await _roleService.IsExistRoleAsync(roleName);
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpGet()]
        public async Task<IActionResult> GetRolesAsync()
        {
            var response = await _roleService.GetRolesAsync();

            return await response.ChangeActionAsync();
        }

        [Authorize]
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserRolesAsync([FromRoute] string userId)
        {
            var response = await _roleService.GetUserRolesAsync(userId);

            return await response.ChangeActionAsync();
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserRoleAsync()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (userId == null) return BadRequest();

            var response = await _roleService.GetUserRolesAsync(userId);
            return await response.ChangeActionAsync();
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpPost()]
        public async Task<bool> CreateRoleAsync([FromBody] string roleName)
        {
            return await _roleService.CreateRoleAsync(roleName);
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpPost("users/{userId}")]
        public async Task<IActionResult> AddUserRoleAsync([FromRoute] string userId, [FromBody] string roleName)
        {
            var response = await _roleService.AddUserRoleAsync(userId, roleName);
            return await response.ChangeActionAsync();
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpDelete("{roleName}")]
        public async Task<bool> DeleteRoleAsync([FromRoute] string roleName)
        {
            return await _roleService.DeleteRoleAsync(roleName);
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUserRolesAsync([FromRoute] string userId, [FromQuery] string roleName)
        {
            var response = await _roleService.DeleteUserRolesAsync(userId, roleName);
            return await response.ChangeActionAsync();
        }
    }
}
