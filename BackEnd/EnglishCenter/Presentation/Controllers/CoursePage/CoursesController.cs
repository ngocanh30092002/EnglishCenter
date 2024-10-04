using System.Security.Claims;
using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.Business.Services.Courses;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.CoursePage
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCoursesAsync()
        {
            var response = await _courseService.GetAllAsync();

            return await response.ChangeActionAsync();
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseAsync([FromRoute] string courseId)
        {
            var response = await _courseService.GetAsync(courseId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("{courseId}/student/is-qualified")]
        [Authorize]
        public async Task<IActionResult> CheckIsQualifiedAsync([FromRoute] string courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var response = await _courseService.CheckIsQualifiedAsync(userId, courseId);
            return await response.ChangeActionAsync();
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateCourseAsync([FromForm] CourseDto model)
        {
            var response = await _courseService.CreateAsync(model);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> UpdateCourseAsync([FromRoute] string courseId, [FromForm] CourseDto model)
        {
            var response = await _courseService.UpdateAsync(courseId, model);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("image/{courseId}")]
        public async Task<IActionResult> UploadCourseImageAsync([FromRoute] string courseId, IFormFile file)
        {
            var response = await _courseService.UploadImageAsync(courseId, file);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("image-thumbnail/{courseId}")]
        public async Task<IActionResult> UploadCourseImageThumbnailAsync([FromRoute] string courseId, IFormFile file)
        {
            var response = await _courseService.UploadImageThumbnailAsync(courseId, file);

            return await response.ChangeActionAsync();
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourseAsync([FromRoute] string courseId)
        {
            var response = await _courseService.DeleteAsync(courseId);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("{courseId}/priority/{priority}")]
        public async Task<IActionResult> ChangePriorityAsync([FromRoute] string courseId, [FromRoute] int priority)
        {
            var response = await _courseService.ChangePriorityAsync(courseId, priority);

            return await response.ChangeActionAsync();
        }
    }
}
