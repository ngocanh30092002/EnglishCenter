using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.HomeworkPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HwSubRecordsController : ControllerBase
    {
        private readonly IHwSubRecordService _subRecordService;

        public HwSubRecordsController(IHwSubRecordService subRecordService)
        {
            _subRecordService = subRecordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _subRecordService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _subRecordService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("submission/{hwSubId}")]
        public async Task<IActionResult> GetByHwSubmitAsync([FromRoute] long hwSubId)
        {
            var response = await _subRecordService.GetByHwSubmitAsync(hwSubId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] HwSubRecordDto model)
        {
            var response = await _subRecordService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] HwSubRecordDto model)
        {
            var response = await _subRecordService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-submission")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeSubmissionAsync([FromRoute] long id, [FromQuery] long submissionId)
        {
            var response = await _subRecordService.ChangeSubmissionAsync(id, submissionId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-home-que")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeHomeQuesAsync([FromRoute] long id, [FromQuery] long homeQueId)
        {
            var response = await _subRecordService.ChangeHomeQuesAsync(id, homeQueId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-sub")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeSubAsync([FromRoute] long id, [FromQuery] long? subId)
        {
            var response = await _subRecordService.ChangeSubAsync(id, subId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-selected-answer")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeSelectedAnswerAsync([FromRoute] long id, [FromQuery] string selectedAnswer)
        {
            var response = await _subRecordService.ChangeSelectedAnswerAsync(id, selectedAnswer);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _subRecordService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
