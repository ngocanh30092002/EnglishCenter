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
    public class ToeicDirectionController : ControllerBase
    {
        private readonly IToeicDirectionService _toeicService;

        public ToeicDirectionController(IToeicDirectionService toeicService)
        {
            _toeicService = toeicService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _toeicService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPost("")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> CreateAsync([FromForm] ToeicDirectionDto model)
        {
            var response = await _toeicService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] ToeicDirectionDto model)
        {
            var response = await _toeicService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [Authorize(Roles = AppRole.ADMIN)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _toeicService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
