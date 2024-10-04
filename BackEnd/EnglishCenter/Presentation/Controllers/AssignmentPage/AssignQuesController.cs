using EnglishCenter.Business.IServices;
using EnglishCenter.Business.Services.Assignments;
using Microsoft.AspNetCore.Http;
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
    }
}
