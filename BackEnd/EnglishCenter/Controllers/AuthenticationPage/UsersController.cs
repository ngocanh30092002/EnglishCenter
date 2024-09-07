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

        public UsersController(IUserRepository userRepo) 
        {
            _userRepo = userRepo;
        }

        [HttpPost("profile-image")]
        public async Task<IActionResult> ChangeUserImageAsync(IFormFile file)
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

            var response = await _userRepo.ChangeUserImageAsync(file, userId);

            return await response.ChangeActionAsync();
        }

        [HttpPost("user-info")]
        public async Task<IActionResult> ChangeUserInfoAsync([FromForm] UserInfoDtoModel model)
        {
            var userId = User.FindFirst("Id")?.Value ?? "";

            var response = await _userRepo.ChangeUserInfoAsync(userId, model);

            return await response.ChangeActionAsync();
        }

        [HttpPost("user-background")]
        public async Task<IActionResult> ChangeUserBackgroundAsync([FromForm] UserBackgroundDtoModel model)
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

        [HttpPost("password")]
        public async Task<IActionResult> ChangePasswordAsync([FromForm] string currentPassword, [FromForm] string newPassword)
        {
            var userId = User.FindFirst("Id")?.Value ?? "";

            var response = await _userRepo.ChangePasswordAsync(userId, currentPassword, newPassword); ;

            return await response.ChangeActionAsync();
        }
    }
}
