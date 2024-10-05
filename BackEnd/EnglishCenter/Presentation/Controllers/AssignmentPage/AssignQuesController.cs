using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.AssignmentPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignQuesController : ControllerBase
    {
        private readonly IAssignQuesService _assignQuesService;

        public AssignQuesController(IAssignQuesService assignQuesService)
        {
            _assignQuesService = assignQuesService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _assignQuesService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var response = await _assignQuesService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("assignments/{assignId}")]
        public async Task<IActionResult> GetByAssignmentAsync(long assignId)
        {
            var response = await _assignQuesService.GetByAssignmentAsync(assignId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] AssignQueDto model)
        {
            var response = await _assignQuesService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] AssignQueDto model)
        {
            var response = await _assignQuesService.UpdateAsync(id,model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _assignQuesService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-assignment")]
        public async Task<IActionResult> ChangeAssignmentIdAsync([FromRoute]long id, [FromQuery] long assignmentId)
        {
            var response = await _assignQuesService.ChangeAssignmentIdAsync(id, assignmentId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-ques")]
        public async Task<IActionResult> ChangeQuesAsync([FromRoute] long id, [FromQuery]int type, [FromQuery] long quesId)
        {
            var response = await _assignQuesService.ChangeQuesAsync(id, type, quesId);
            return await response.ChangeActionAsync();
        }
    }
}
