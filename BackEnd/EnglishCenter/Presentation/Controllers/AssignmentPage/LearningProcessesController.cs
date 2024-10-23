using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AssignmentPage
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LearningProcessesController : ControllerBase
    {
        private readonly ILearningProcessService _processService;

        public LearningProcessesController(ILearningProcessService processService) 
        {
            _processService = processService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _processService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _processService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] LearningProcessDto model)
        {
            var response = await _processService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}/submit")]
        public async Task<IActionResult> HandleSubmitAsync([FromRoute] long id, [FromForm] LearningProcessDto model)
        {
            var response = await _processService.HandleSubmitProcessAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] LearningProcessDto model)
        {
            var response = await _processService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeStatusAsync([FromRoute] long id, [FromQuery] int status)
        {
            var response = await _processService.ChangeStatusAsync(id, status);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/start-time")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeStartTimeAsync([FromRoute] long id, [FromBody] string dateTime)
        {
            var response = await _processService.ChangeStartTimeAsync(id, dateTime);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/end-time")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> ChangeEndTimeAsync([FromRoute] long id, [FromBody] string dateTime)
        {
            var response = await _processService.ChangeEndTimeAsync(id, dateTime);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _processService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
