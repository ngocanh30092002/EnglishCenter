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
    public class ToeicAttemptsController : ControllerBase
    {
        private readonly IToeicAttemptService _toeicService;

        public ToeicAttemptsController(IToeicAttemptService toeicService)
        {
            _toeicService = toeicService;
        }

        [HttpGet("admin")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _toeicService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _toeicService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetByUserAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var response = await _toeicService.GetByUserAsync(userId);
            return await response.ChangeActionAsync();
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync([FromForm] ToeicAttemptDto model)
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

        [HttpPut("{id}/submit")]
        public async Task<IActionResult> HandleSubmitAsync([FromRoute] long id, [FromForm] ToeicAttemptDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            model.UserId = userId;

            var response = await _toeicService.HandleSubmitToeicAsync(id, model);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] ToeicAttemptDto model)
        {
            var response = await _toeicService.UpdateAsync(id, model);

            return await response.ChangeActionAsync();
        }


        [HttpPatch("{id}/user")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeUserAsync([FromRoute] long id, [FromBody] string userId)
        {
            var response = await _toeicService.ChangeUserAsync(id, userId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/toeic")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeToeicAsync([FromRoute] long id, [FromQuery] long toeicId)
        {
            var response = await _toeicService.ChangeToeicAsync(id, toeicId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/listening-score")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeListeningScoreAsync([FromRoute] long id, [FromQuery] int score)
        {
            var response = await _toeicService.ChangeListeningScoreAsync(id, score);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/reading-score")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeReadingScoreAsync([FromRoute] long id, [FromQuery] int score)
        {
            var response = await _toeicService.ChangeReadingScoreAsync(id, score);
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
