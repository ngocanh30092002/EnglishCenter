﻿using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AuthenticationPage
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService, IRoleService roleService)
        {
            _roleService = roleService;
            _studentService = studentService;
        }

        [HttpPost("profile-image")]
        public async Task<IActionResult> ChangeUserImageAsync(IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var isImageFile = await UploadHelper.IsImageAsync(file);
            if (!isImageFile)
            {
                return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
            }

            var response = await _studentService.ChangeStudentImageAsync(file, userId);

            return await response.ChangeActionAsync();
        }

        [HttpPost("background-image")]
        public async Task<IActionResult> ChangeBackgroundImageAsync(IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var isImageFile = await UploadHelper.IsImageAsync(file);
            if (!isImageFile)
            {
                return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
            }

            var response = await _studentService.ChangeBackgroundImageAsync(file, userId);

            return await response.ChangeActionAsync();
        }

        [HttpPost("user-background")]
        public async Task<IActionResult> ChangeUserBackgroundAsync([FromForm] UserBackgroundDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var response = await _studentService.ChangeStudentBackgroundAsync(userId, model);

            return await response.ChangeActionAsync();
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetUserRolesAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (userId == "")
            {
                return BadRequest("UserId is required");
            }

            var result = await _roleService.GetUserRolesAsync(userId);

            return await result.ChangeActionAsync();
        }

        [HttpPost("password")]
        public async Task<IActionResult> ChangePasswordAsync([FromForm] string currentPassword, [FromForm] string newPassword)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var response = await _studentService.ChangePasswordAsync(userId, currentPassword, newPassword); ;

            return await response.ChangeActionAsync();
        }
    }
}
