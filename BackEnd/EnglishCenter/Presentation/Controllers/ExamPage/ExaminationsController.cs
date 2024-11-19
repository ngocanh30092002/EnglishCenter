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
    public class ExaminationsController : ControllerBase
    {
        private readonly IExaminationService _examService;

        public ExaminationsController(IExaminationService examService)
        {
            _examService = examService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _examService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllAsync([FromRoute] long id)
        {
            var response = await _examService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> CreateAsync([FromForm] ExaminationDto model)
        {
            var response = await _examService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] ExaminationDto model)
        {
            var response = await _examService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _examService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/content")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeCourseContentAsync([FromRoute] long id, [FromQuery] long contentId)
        {
            var response = await _examService.ChangeCourseContentAsync(id, contentId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/toeic")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeToeicAsync([FromRoute] long id, [FromQuery] long toeicId)
        {
            var response = await _examService.ChangeToeicAsync(id, toeicId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/time")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeTimeAsync([FromRoute] long id, [FromBody] string timeStr)
        {
            var response = await _examService.ChangeTimeAsync(id, timeStr);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/title")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeTitleAsync([FromRoute] long id, [FromBody] string title)
        {
            var response = await _examService.ChangeTitleAsync(id, title);
            return await response.ChangeActionAsync();
        }
    }
}
