using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AuthenticationPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet("{userId}/full-name")]
        public async Task<IActionResult> GetFullNameAsync([FromRoute] string userId)
        {
            var response = await _teacherService.GetFullNameAsync(userId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAsync([FromRoute] string userId)
        {
            var response = await _teacherService.GetAsync(userId);
            return await response.ChangeActionAsync();
        }

        [HttpPost("profile-image")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeTeacherImageAsync(IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var isImageFile = await UploadHelper.IsImageAsync(file);
            if (!isImageFile)
            {
                return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
            }

            var response = await _teacherService.ChangeTeacherImageAsync(file, userId);

            return await response.ChangeActionAsync();
        }

        [HttpPost("background-image")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeBackgroundImageAsync(IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var isImageFile = await UploadHelper.IsImageAsync(file);
            if (!isImageFile)
            {
                return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
            }

            var response = await _teacherService.ChangeBackgroundImageAsync(file, userId);

            return await response.ChangeActionAsync();
        }

        [HttpPost("admin/profile-image")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeTeacherImageAsync(IFormFile file, [FromQuery] string userId)
        {
            var isImageFile = await UploadHelper.IsImageAsync(file);
            if (!isImageFile)
            {
                return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
            }

            var response = await _teacherService.ChangeTeacherImageAsync(file, userId);

            return await response.ChangeActionAsync();
        }

        [HttpPost("admin/background-image")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeBackgroundImageAsync(IFormFile file, [FromQuery] string userId)
        {
            var isImageFile = await UploadHelper.IsImageAsync(file);
            if (!isImageFile)
            {
                return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
            }

            var response = await _teacherService.ChangeBackgroundImageAsync(file, userId);

            return await response.ChangeActionAsync();
        }
    }
}
