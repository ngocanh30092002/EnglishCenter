using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.CoursePage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToeicConversionController : ControllerBase
    {
        private readonly IToeicConversionService _toeicService;

        public ToeicConversionController(IToeicConversionService toeicService)
        {
            _toeicService = toeicService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _toeicService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute]int id)
        {
            var response = await _toeicService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("section/{section}")]
        public async Task<IActionResult> GetBySectionAsync([FromRoute] string section)
        {
            var response = await _toeicService.GetBySectionAsync(section);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> CreateAsync([FromForm] ToeicConversionDto model)
        {
            var response = await _toeicService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var response = await _toeicService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
