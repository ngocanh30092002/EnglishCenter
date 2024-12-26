using System.Security.Claims;
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
    public class ToeicPracticeController : ControllerBase
    {
        private readonly IAttemptRecordService _toeicService;

        public ToeicPracticeController(IAttemptRecordService toeicService)
        {
            _toeicService = toeicService;
        }

        [HttpGet]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _toeicService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _toeicService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("attempt/{attemptId}")]
        public async Task<IActionResult> GetByRoadMapQuestionAsync([FromRoute] long attemptId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var response = await _toeicService.GetByRoadMapQuestionAsync(attemptId, userId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("attempt/{attemptId}/result-answer")]
        public async Task<IActionResult> GetResultAsync([FromRoute] long attemptId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var response = await _toeicService.GetResultAsync(attemptId, userId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("attempt/{attemptId}/score")]
        public async Task<IActionResult> GetResultScoreAsync([FromRoute] long attemptId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var response = await _toeicService.GetResultScoreAsync(attemptId, userId);
            return await response.ChangeActionAsync();
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync([FromForm] AttemptRecordDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            model.UserId = userId;

            var response = await _toeicService.CreateAsync(model);

            return await response.ChangeActionAsync();
        }

        [HttpPost("admin")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> CreateAsync([FromForm] AttemptRecordDto model, [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            model.UserId = userId;

            var response = await _toeicService.CreateAsync(model);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] AttemptRecordDto model)
        {
            var response = await _toeicService.UpdateAsync(id, model);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/selected-answer")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeSelectedAnswerAsync([FromRoute] long id, [FromBody] string? answer)
        {
            var response = await _toeicService.ChangeSelectedAnswerAsync(id, answer);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _toeicService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
