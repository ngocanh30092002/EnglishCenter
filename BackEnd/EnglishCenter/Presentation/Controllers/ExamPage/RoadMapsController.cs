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
    public class RoadMapsController : ControllerBase
    {
        private readonly IRoadMapService _roadMapService;

        public RoadMapsController(IRoadMapService roadMapService)
        {
            _roadMapService = roadMapService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _roadMapService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _roadMapService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("course")]
        public async Task<IActionResult> GetCourseIdsAsync()
        {
            var response = await _roadMapService.GetCourseIdsAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("courses/{courseId}")]
        public async Task<IActionResult> GetByCourseAsync([FromRoute] string courseId)
        {
            var response = await _roadMapService.GetByCourseAsync(courseId);
            return await response.ChangeActionAsync();
        }

        [HttpPost()]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> CreateAsync([FromForm] RoadMapDto model)
        {
            var response = await _roadMapService.CreateAsync(model);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] RoadMapDto model)
        {
            var response = await _roadMapService.UpdateAsync(id, model);

            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _roadMapService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
