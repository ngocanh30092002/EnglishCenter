using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.HomeworkPage
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class HwSubmissionController : ControllerBase
    {
        private readonly IHwSubmissionService _submitService;

        public HwSubmissionController(IHwSubmissionService submitService)
        {
            _submitService = submitService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _submitService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _submitService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("homework/{homeworkId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> GetByHomeworkAsync([FromRoute] long homeworkId)
        {
            var response = await _submitService.GetByHomeworkAsync(homeworkId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}/score")]
        public async Task<IActionResult> GetScoreAsync([FromRoute] long id)
        {
            var response = await _submitService.GetScoreAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("enrolls/{enrollId}")]
        public async Task<IActionResult> GetByEnrollAsync([FromRoute] long enrollId, [FromQuery] long homeworkId)
        {
            var response = await _submitService.GetByEnrollAsync(enrollId, homeworkId);
            return await response.ChangeActionAsync();
        }


        [HttpGet("enrolls/{enrollId}/his")]
        public async Task<IActionResult> GetByEnrollHistoryAsync([FromRoute] long enrollId)
        {
            var response = await _submitService.GetByEnrollHistoryAsync(enrollId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("enrolls/{enrollId}/ongoing")]
        public async Task<IActionResult> GetOngoingAsync([FromRoute] long enrollId, [FromQuery] long homeworkId)
        {
            var response = await _submitService.GetOngoingAsync(enrollId, homeworkId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("enrolls/{enrollId}/number-attempt")]
        public async Task<IActionResult> GetNumberAttemptAsync([FromRoute] long enrollId, [FromQuery] long homeworkId)
        {
            var response = await _submitService.GetNumberAttemptAsync(enrollId, homeworkId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}/is-submitted")]
        public async Task<IActionResult> IsSubmittedAsync([FromRoute] long id)
        {
            var response = await _submitService.IsSubmittedAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] HwSubmissionDto model)
        {
            var response = await _submitService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}/submit")]
        public async Task<IActionResult> HandleSubmitHomework([FromRoute] long id, [FromForm] HwSubmissionDto model)
        {
            var response = await _submitService.HandleSubmitHomework(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] HwSubmissionDto model)
        {
            var response = await _submitService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _submitService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/homework")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeHomeworkAsync([FromRoute] long id, [FromQuery] long homeworkId)
        {
            var response = await _submitService.ChangeHomeworkAsync(id, homeworkId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/enroll")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeEnrollAsync([FromRoute] long id, [FromQuery] long enrollId)
        {
            var response = await _submitService.ChangeEnrollAsync(id, enrollId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/date")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeDateAsync([FromRoute] long id, [FromBody] string dateTime)
        {
            var response = await _submitService.ChangeDateAsync(id, dateTime);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/pass")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeIsPassAsync([FromRoute] long id, [FromQuery] bool isPass)
        {
            var response = await _submitService.ChangeIsPassAsync(id, isPass);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/feedback")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeFeedbackAsync([FromRoute] long id, [FromBody] string feedback)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var isValid = await _submitService.IsInChargeAsync(userId, id);
                if (!isValid)
                {
                    return Forbid("You can't send feedback on record which you aren't in charge of.");
                }
            }

            var response = await _submitService.ChangeFeedbackAsync(id, feedback);
            return await response.ChangeActionAsync();
        }
    }
}
