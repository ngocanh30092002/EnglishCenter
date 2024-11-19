using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.ExamPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToeicRecordsController : ControllerBase
    {
        private readonly IToeicRecordService _toeicRecordService;

        public ToeicRecordsController(IToeicRecordService toeicRecordService)
        {
            _toeicRecordService = toeicRecordService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _toeicRecordService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _toeicRecordService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("processes/{processId}/result")]
        public async Task<IActionResult> GetResultAsync([FromRoute] long processId)
        {
            var response = await _toeicRecordService.GetResultAsync(processId);
            return await response.ChangeActionAsync();
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromForm] ToeicRecordDto model)
        {
            var response = await _toeicRecordService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] ToeicRecordDto model)
        {
            var response = await _toeicRecordService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpPatch("{id}/processes/{processId}")]
        public async Task<IActionResult> ChangeProcessAsync([FromRoute] long id, [FromRoute] long processId)
        {
            var response = await _toeicRecordService.ChangeProcessAsync(id, processId);
            return await response.ChangeActionAsync();
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpPatch("{id}/selected-answer/{answer}")]
        public async Task<IActionResult> ChangeProcessAsync([FromRoute] long id, [FromRoute] string answer)
        {
            var response = await _toeicRecordService.ChangeSelectedAnswerAsync(id, answer);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _toeicRecordService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
