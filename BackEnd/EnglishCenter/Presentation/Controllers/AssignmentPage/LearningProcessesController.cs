using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AssignmentPage
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LearningProcessesController : ControllerBase
    {
        private readonly ILearningProcessService _processService;

        public LearningProcessesController(ILearningProcessService processService)
        {
            _processService = processService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _processService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _processService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("ongoing/enrollments/{enrollId}")]
        public async Task<IActionResult> GetOngoingAsync([FromRoute] long enrollId, [FromQuery] long? assignmentId, [FromQuery] long? examId)
        {
            var response = await _processService.GetOngoingAsync(enrollId, assignmentId, examId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("his/enrollments/{enrollId}")]
        public async Task<IActionResult> GetHisProcesses([FromRoute] long enrollId, [FromQuery] long? assignmentId, [FromQuery] long? examId)
        {
            var response = await _processService.GetHisProcessesAsync(enrollId, assignmentId, examId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}/score")]
        public async Task<IActionResult> GetScoreByProcessAsync([FromRoute] long id)
        {
            var response = await _processService.GetScoreByProcessAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("enrollments/{enrollId}/number-attempted")]
        public async Task<IActionResult> GetNumberAttemptedAsync([FromRoute] long enrollId, [FromQuery] long assignmentId)
        {
            var response = await _processService.GetNumberAttemptedAsync(enrollId, assignmentId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("enrollments/{enrollId}/status-exam")]
        public async Task<IActionResult> GetStatusExamAsync([FromRoute] long enrollId, [FromQuery] long examId)
        {
            var response = await _processService.GetStatusExamAsync(enrollId, examId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("enrollments/{enrollId}/status")]
        public async Task<IActionResult> GetStatusLessonAsync([FromRoute] long enrollId, [FromQuery] long? assignmentId, [FromQuery] long? examId)
        {
            var response = await _processService.GetStatusLessonAsync(enrollId, assignmentId, examId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}/is-submitted")]
        public async Task<IActionResult> IsSubmittedAsync([FromRoute] long id)
        {
            var response = await _processService.IsSubmittedAsync(id);
            return await response.ChangeActionAsync();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] LearningProcessDto model)
        {
            var response = await _processService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}/submit")]
        public async Task<IActionResult> HandleSubmitAsync([FromRoute] long id, [FromForm] LearningProcessDto model)
        {
            var response = await _processService.HandleSubmitProcessAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] LearningProcessDto model)
        {
            var response = await _processService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeStatusAsync([FromRoute] long id, [FromQuery] int status)
        {
            var response = await _processService.ChangeStatusAsync(id, status);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/start-time")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeStartTimeAsync([FromRoute] long id, [FromBody] string dateTime)
        {
            var response = await _processService.ChangeStartTimeAsync(id, dateTime);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/end-time")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeEndTimeAsync([FromRoute] long id, [FromBody] string dateTime)
        {
            var response = await _processService.ChangeEndTimeAsync(id, dateTime);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _processService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
