using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.CoursePage
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CourseContentController : ControllerBase
    {
        private readonly ICourseContentService _contentService;

        public CourseContentController(ICourseContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetCourseContentsAsync()
        {
            var response = await _contentService.GetAllAsync();

            return await response.ChangeActionAsync();
        }

        [HttpGet("{contentId}")]
        public async Task<IActionResult> GetCourseContentAsync(long contentId)
        {
            var response = await _contentService.GetAsync(contentId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCourseContentsAsync([FromRoute] string courseId)
        {
            var response = await _contentService.GetByCourseAsync(courseId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("course/{courseId}/enroll/{enrollId}")]
        public async Task<IActionResult> GetHisCourseContentAsync([FromRoute] string courseId, [FromRoute] long enrollId)
        {
            var response = await _contentService.GetHisCourseContentAsync(courseId, enrollId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("course/{courseId}/total-time")]
        public async Task<IActionResult> GetTotalTimeByCourseAsync([FromRoute] string courseId)
        {
            var response = await _contentService.GetTotalTimeByCourseAsync(courseId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("course/{courseId}/total-num")]
        public async Task<IActionResult> GetNumLessonAsync([FromRoute] string courseId)
        {
            var response = await _contentService.GetNumLessonAsync(courseId);

            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateContentAsync([FromForm] CourseContentDto model)
        {
            var response = await _contentService.CreateAsync(model);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{contentId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateContentAsync([FromRoute] long contentId, [FromForm] CourseContentDto model)
        {
            var response = await _contentService.UpdateAsync(contentId, model);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("{contentId}/{number}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeNoNumAsync([FromRoute] long contentId, int number)
        {
            var response = await _contentService.ChangeNoNumAsync(contentId, number);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("{contentId}/content")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeContentAsync([FromRoute] long contentId, [FromBody] string content)
        {
            var response = await _contentService.ChangeContentAsync(contentId, content);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("{contentId}/type")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeTypeAsync([FromRoute] long contentId, [FromQuery] int type)
        {
            var response = await _contentService.ChangeTypeAsync(contentId, type);

            return await response.ChangeActionAsync();
        }

        [HttpDelete("{contentId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> RemoveContentAsync(long contentId)
        {
            var response = await _contentService.DeleteAsync(contentId);

            return await response.ChangeActionAsync();
        }
    }
}
