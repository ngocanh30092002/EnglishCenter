using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.CoursePage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreHistoryController : ControllerBase
    {
        private readonly IScoreHistoryService _scoreHisService;

        public ScoreHistoryController(IScoreHistoryService scoreHisService) 
        {
            _scoreHisService = scoreHisService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _scoreHisService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{scoreId}")]
        public async Task<IActionResult> GetAsync([FromRoute] long scoreId)
        {
            var response = await _scoreHisService.GetAsync(scoreId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] ScoreHistoryDto model)
        {
            var response = await _scoreHisService.CreateAsync(model);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{scoreId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] long scoreId, [FromForm] ScoreHistoryDto model)
        {
            var response = await _scoreHisService.UpdateAsync(scoreId, model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{scoreId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long scoreId)
        {
            var response = await _scoreHisService.DeleteAsync(scoreId);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("{scoreId}/entrance-point")]
        public async Task<IActionResult> ChangeEntrancePointAsync([FromRoute] long scoreId, [FromBody] int score)
        {
            var response = await _scoreHisService.ChangeEntrancePointAsync(scoreId, score);
            return await response.ChangeActionAsync();  
        }

        [HttpPatch("{scoreId}/final-point")]
        public async Task<IActionResult> ChangeFinalPointAsync([FromRoute] long scoreId, [FromBody] int score)
        {
            var response = await _scoreHisService.ChangeFinalPointAsync(scoreId, score);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{scoreId}/midterm-point")]
        public async Task<IActionResult> ChangeMidtermPointAsync([FromRoute] long scoreId, [FromBody] int score)
        {
            var response = await _scoreHisService.ChangeMidtermPointAsync(scoreId, score);
            return await response.ChangeActionAsync();
        }
    }
}
