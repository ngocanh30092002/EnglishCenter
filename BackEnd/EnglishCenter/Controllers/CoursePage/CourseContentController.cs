using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers.CoursePage
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseContentController : ControllerBase
    {
        private readonly ICourseContentRepository _contentRepo;

        public CourseContentController(ICourseContentRepository contentRepo) 
        {
            _contentRepo = contentRepo;
        }

        [HttpGet()]
        public async Task<IActionResult> GetCourseContentsAsync()
        {
            var response = await _contentRepo.GetContentsAsync();

            return await response.ChangeActionAsync();
        }

        [HttpGet("{contentId}")]
        public async Task<IActionResult> GetCourseContentAsync(long contentId)
        {
            var response = await _contentRepo.GetContentAsync(contentId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCourseContentsAsync([FromRoute] string courseId)
        {
            var response = await _contentRepo.GetContentsAsync(courseId);

            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContentAsync([FromForm] CourseContentDto model)
        {
            var response = await _contentRepo.CreateContentAsync(model);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{contentId}")]
        public async Task<IActionResult> UpdateContentAsync([FromRoute] long contentId, [FromForm] CourseContentDto model)
        {
            var response = await _contentRepo.UpdateContentAsync(contentId, model);
            
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{contentId}/{number}")]
        public async Task<IActionResult> ChangeNoNumAsync([FromRoute] long contentId, int number)
        {
            var response = await _contentRepo.ChangeNoNumAsync(contentId, number);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("{contentId}/content")]
        public async Task<IActionResult> ChangeContentAsync([FromRoute] long contentId, [FromBody] string content)
        {
            var response = await _contentRepo.ChangeContentAsync(contentId, content);

            return await response.ChangeActionAsync();
        }

        [HttpDelete("{contentId}")]
        public async Task<IActionResult> RemoveContentAsync(long contentId)
        {
            var response = await _contentRepo.RemoveContentAsync(contentId);

            return await response.ChangeActionAsync();
        }
    }
}
