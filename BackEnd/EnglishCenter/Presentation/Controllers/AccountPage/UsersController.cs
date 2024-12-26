using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AccountPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("bg-info")]
        public async Task<IActionResult> GetBackgroundInforAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? ""; ;
            if (userId == "")
            {
                return BadRequest();
            }

            var response = await _userService.GetUserBackgroundInfoAsync(userId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? ""; ;
            if (userId == "")
            {
                return BadRequest();
            }

            var response = await _userService.GetUserInfoAsync(userId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("full-info")]
        public async Task<IActionResult> GetUserFullInfoAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? ""; ;
            if (userId == "")
            {
                return BadRequest();
            }

            var response = await _userService.GetUserFullInfoAsync(userId);
            return await response.ChangeActionAsync();
        }

        [HttpPost("user-info")]
        public async Task<IActionResult> ChangeUserInfoAsync([FromForm] UserInfoDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var response = await _userService.ChangeUserInfoAsync(userId, model);

            return await response.ChangeActionAsync();
        }

        [HttpPost("user-background")]
        public async Task<IActionResult> ChangeUserBackgroundAsync([FromForm] UserBackgroundDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var response = await _userService.ChangeUserBackgroundAsync(userId, model);

            return await response.ChangeActionAsync();
        }

        [HttpPost("password")]
        public async Task<IActionResult> ChangeUserPasswordAsync([FromForm] string currentPassword, [FromForm] string newPassword)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var response = await _userService.ChangePasswordAsync(userId, currentPassword, newPassword); ;

            return await response.ChangeActionAsync();
        }

        [HttpPatch("profile-image")]
        public async Task<IActionResult> ChangeUserImageAsync(IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var isImageFile = await UploadHelper.IsImageAsync(file);
            if (!isImageFile)
            {
                return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
            }

            var response = await _userService.ChangeUserImageAsync(userId, file);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("background-image")]
        public async Task<IActionResult> ChangeUserBackgroundImageAsync(IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var isImageFile = await UploadHelper.IsImageAsync(file);
            if (!isImageFile)
            {
                return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
            }

            var response = await _userService.ChangeUserBackgroundImageAsync(userId, file);

            return await response.ChangeActionAsync();
        }
    }
}
