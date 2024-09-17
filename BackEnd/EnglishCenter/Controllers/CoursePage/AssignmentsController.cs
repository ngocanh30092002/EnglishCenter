using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers.CoursePage
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentRepository _assignRepo;

        public AssignmentsController(IAssignmentRepository assignRepo)
        {
            _assignRepo = assignRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAssignmentsAsync()
        {
            var response = await _assignRepo.GetAssignmentsAsync();

            return await response.ChangeActionAsync();
        }

        [HttpGet("{assignmentId}")]
        public async Task<IActionResult> GetAssignmentAsync([FromRoute] long assignmentId) 
        {
            var response = await _assignRepo.GetAssignmentAsync(assignmentId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("content/{contentId}")]
        public async Task<IActionResult> GetAssignmentsAsync([FromRoute] long contentId)
        {
            var response = await _assignRepo.GetAssignmentsAsync(contentId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("course/{courseId}/number")]
        public async Task<IActionResult> GetNumberAssignmentWithCourse([FromRoute] string courseId)
        {
            var response = await _assignRepo.GetNumberAssignmentsAsync(courseId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("course/{courseId}/total-time")]
        public async Task<IActionResult> GetTotalTimeWithCourseAsync([FromRoute] string courseId)
        {
            var response = await _assignRepo.GetTotalTimeAssignmentsAsync(courseId);

            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssignmentAsync([FromForm] AssignmentDto model)
        {
            var response = await _assignRepo.CreateAssignmentAsync(model);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{assignmentId}")]
        public async Task<IActionResult> UpdateAssignmentAsync([FromRoute] long assignmentId, [FromForm] AssignmentDto model)
        {
            var response = await _assignRepo.UpdateAssignmentAsync(assignmentId, model);

            return await response.ChangeActionAsync();
        }

        [HttpDelete("{assignmentId}")]
        public async Task<IActionResult> RemoveAssignmentAsync([FromRoute] long assignmentId)
        {
            var response = await _assignRepo.RemoveAssignmentAsync(assignmentId);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("{assignmentId}/{number}")]
        public async Task<IActionResult> ChangeNoNumAsync([FromRoute] long assignmentId, [FromRoute] int number)
        {
            var response = await _assignRepo.ChangeNoNumberAsync(assignmentId, number);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{assignmentId}/content-id/{contentId}")]
        public async Task<IActionResult> ChangeCourseContentAsync([FromRoute] long assignmentId, [FromRoute] long contentId)
        {
            var response = await _assignRepo.ChangeCourseContentAsync(assignmentId, contentId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{assignmentId}/title")]
        public async Task<IActionResult> ChangeCourseContentTitleAsync([FromRoute] long assignmentId, [FromBody] string title)
        {
            var response = await _assignRepo.ChangeCourseContentTitleAsync(assignmentId, title);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{assignmentId}/time")]
        public async Task<IActionResult> ChangeTimeAsync([FromRoute] long assignmentId, [FromBody] string time)
        {
            var response = await _assignRepo.ChangeTimeAsync(assignmentId, time);
            return await response.ChangeActionAsync();
        }
    }
}
