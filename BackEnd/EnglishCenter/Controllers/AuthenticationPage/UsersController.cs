using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers.AuthenticationPage
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;

        public UsersController(IUserRepository userRepo, IRoleRepository roleRepo) 
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
        }

        [HttpPost("profile-image")]
        public async Task<IActionResult> ChangeUserImageAsync([FromForm] IFormFile file)
        {
            var userId = User.FindFirst("Id")?.Value ?? "";

            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/svg+xml" };
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".svg" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedMimeTypes.Contains(file.ContentType.ToLower()) || !allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { message = "Invalid file type or extension. Only JPEG, PNG, GIF, and SVG are allowed." });
            }

            var response = await _userRepo.ChangeUserImageAsync(file, userId);

            return await response.ChangeActionAsync();
        }

        [HttpPost("background-image")]
        public async Task<IActionResult> ChangeBackgroundImageAsync(IFormFile file)
        {
            var userId = User.FindFirst("Id")?.Value ?? "";

            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/svg+xml" };
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".svg" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedMimeTypes.Contains(file.ContentType.ToLower()) || !allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { message = "Invalid file type or extension. Only JPEG, PNG, GIF, and SVG are allowed." });
            }

            var response = await _userRepo.ChangeBackgroundImageAsync(file, userId);

            return await response.ChangeActionAsync();
        }

        [HttpPost("user-info")]
        public async Task<IActionResult> ChangeUserInfoAsync([FromForm] UserInfoDto model)
        {
            var userId = User.FindFirst("Id")?.Value ?? "";

            var response = await _userRepo.ChangeUserInfoAsync(userId, model);

            return await response.ChangeActionAsync();
        }

        [HttpPost("user-background")]
        public async Task<IActionResult> ChangeUserBackgroundAsync([FromForm] UserBackgroundDto model)
        {
            var userId = User.FindFirst("Id")?.Value ?? "";

            var response = await _userRepo.ChangeUserBackgroundAsync(userId, model);

            return await response.ChangeActionAsync();
        }

        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInforAsync()
        {
            var userId = User.FindFirst("Id")?.Value ?? "";

            var response = await _userRepo.GetUserInfo(userId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("user-background-info")]
        public async Task<IActionResult> GetUserBackgroundInfoAsync()
        {
            var userId = User.FindFirst("Id")?.Value ?? "";

            var response = await _userRepo.GetUserBackground(userId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetUserRolesAsync()
        {
            var userId = User.FindFirst("Id")?.Value ?? "";
            
            if(userId == "")
            {
                return BadRequest("UserId is required");
            }

            var result = await _roleRepo.GetUserRolesAsync(userId);

            return await result.ChangeActionAsync();
        }

        [HttpPost("password")]
        public async Task<IActionResult> ChangePasswordAsync([FromForm] string currentPassword, [FromForm] string newPassword)
        {
            var userId = User.FindFirst("Id")?.Value ?? "";

            var response = await _userRepo.ChangePasswordAsync(userId, currentPassword, newPassword); ;

            return await response.ChangeActionAsync();
        }
    }
}
