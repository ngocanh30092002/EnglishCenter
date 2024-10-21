using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] HwSubmissionDto model)
        {
            var response = await _submitService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id,[FromForm] HwSubmissionDto model)
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

        [HttpPatch("{id}/change-homework")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeHomeworkAsync([FromRoute] long id, [FromQuery] long homeworkId)
        {
            var response = await _submitService.ChangeHomeworkAsync(id, homeworkId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-enroll")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeEnrollAsync([FromRoute] long id, [FromQuery] long enrollId)
        {
            var response = await _submitService.ChangeEnrollAsync(id, enrollId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-date")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeDateAsync([FromRoute] long id, [FromBody] string dateTime)
        {
            var response = await _submitService.ChangeDateAsync(id, dateTime);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-pass")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeIsPassAsync([FromRoute] long id, [FromQuery] bool isPass)
        {
            var response = await _submitService.ChangeIsPassAsync(id, isPass);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-feedback")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeFeedbackAsync([FromRoute] long id, [FromBody] string feedback)
        {
            var response = await _submitService.ChangeFeedbackAsync(id, feedback);
            return await response.ChangeActionAsync();
        }
    }
}
