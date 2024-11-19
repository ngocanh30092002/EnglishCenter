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
    public class ToeicExamsController : ControllerBase
    {
        private readonly IToeicExamService _toeicService;

        public ToeicExamsController(IToeicExamService toeicService)
        {
            _toeicService = toeicService;
        }

        [HttpGet()]
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

        [HttpGet("{id}/direction")]
        public async Task<IActionResult> GetDirectionAsync([FromRoute] long id)
        {
            var response = await _toeicService.GetToeicDirectionAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> CreateAsync([FromForm] ToeicExamDto model)
        {
            var response = await _toeicService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] ToeicExamDto model)
        {
            var response = await _toeicService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _toeicService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/name")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeNameAsync([FromRoute] long id, [FromBody] string newName)
        {
            var response = await _toeicService.ChangeNameAsync(id, newName);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/code")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeCodeAsync([FromRoute] long id, [FromQuery] int code)
        {
            var response = await _toeicService.ChangeCodeAsync(id, code);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/year")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeYearAsync([FromRoute] long id, [FromQuery] int year)
        {
            var response = await _toeicService.ChangeYearAsync(id, year);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/completed-num")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeCompleteNumAsync([FromRoute] long id, [FromQuery] int num)
        {
            var response = await _toeicService.ChangeCompleteNumAsync(id, num);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/point")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangePointAsync([FromRoute] long id, [FromQuery] int point)
        {
            var response = await _toeicService.ChangePointAsync(id, point);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/minutes")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeMinutesAsync([FromRoute] long id, [FromQuery] int minutes)
        {
            var response = await _toeicService.ChangeMinutesAsync(id, minutes);
            return await response.ChangeActionAsync();
        }
    }
}
