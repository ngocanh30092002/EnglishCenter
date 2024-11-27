using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.ClassPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionTasksController : ControllerBase
    {
        private readonly ISubmissionTaskService _taskService;

        public SubmissionTasksController(ISubmissionTaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _taskService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _taskService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("classes/{classId}")]
        public async Task<IActionResult> GetCurrentByClassAsync([FromRoute] string classId)
        {
            var response = await _taskService.GetCurrentByClassAsync(classId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAsync([FromForm] SubmissionTaskDto model)
        {
            var response = await _taskService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] SubmissionTaskDto model)
        {
            var response = await _taskService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/title")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeTitleAsync([FromRoute] long id, [FromBody] string title)
        {
            var response = await _taskService.ChangeTitleAsync(id, title);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/description")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeDescriptionAsync([FromRoute] long id, [FromBody] string description)
        {
            var response = await _taskService.ChangeDescriptionAsync(id, description);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/start-time")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeStartTimeAsync([FromRoute] long id, [FromBody] string startTime)
        {
            var response = await _taskService.ChangeStartTimeAsync(id, startTime);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/end-time")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeEndTimeAsync([FromRoute] long id, [FromBody] string endTime)
        {
            var response = await _taskService.ChangeEndTimeAsync(id, endTime);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/lesson")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeLessonAsync([FromRoute] long id, [FromQuery] long lessonId)
        {
            var response = await _taskService.ChangeLessonAsync(id, lessonId);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _taskService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }

    }
}
