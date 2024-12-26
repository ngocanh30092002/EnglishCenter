using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.ExamPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoadMapExamsController : ControllerBase
    {
        private readonly IRoadMapExamService _roadMapExamService;

        public RoadMapExamsController(IRoadMapExamService roadMapExamService)
        {
            _roadMapExamService = roadMapExamService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _roadMapExamService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _roadMapExamService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("road-maps/{roadMapId}")]
        public async Task<IActionResult> GetByRoadMapAsync([FromRoute] long roadMapId)
        {
            var response = await _roadMapExamService.GetByRoadMapAsync(roadMapId);
            return await response.ChangeActionAsync();
        }

        [HttpPost()]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> CreateAsync([FromForm] RoadMapExamDto model)
        {
            var response = await _roadMapExamService.CreateAsync(model);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] RoadMapExamDto model)
        {
            var response = await _roadMapExamService.UpdateAsync(id, model);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/name")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeNameAsync([FromRoute] long id, [FromBody] string newName)
        {
            var response = await _roadMapExamService.ChangeNameAsync(id, newName);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/time")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeTimeAsync([FromRoute] long id, [FromQuery] double timeMinutes)
        {
            var response = await _roadMapExamService.ChangeTimeAsync(id, timeMinutes);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/roadMap")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeRoadMapAsync([FromRoute] long id, [FromQuery] long roadMapId)
        {
            var response = await _roadMapExamService.ChangeRoadMapAsync(id, roadMapId);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _roadMapExamService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
