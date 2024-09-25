using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.CoursePage
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollStatusController : ControllerBase
    {
        private readonly IEnrollStatusService _enrollService;

        public EnrollStatusController(IEnrollStatusService enrollService)
        {
            _enrollService = enrollService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEnrollStatusAsync()
        {
            var response = await _enrollService.GetAllAsync();

            return await response.ChangeActionAsync();
        }

        [HttpGet("{enrollStatusId}")]
        public async Task<IActionResult> GetEnrollStatusAsync([FromRoute] int enrollStatusId)
        {
            var response = await _enrollService.GetAsync(enrollStatusId);

            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrollStatusAsync([FromForm] EnrollStatusDto model)
        {
            var response = await _enrollService.CreateAsync(model);

            return await response.ChangeActionAsync();
        }

        [HttpDelete("{enrollStatusId}")]
        public async Task<IActionResult> DeleteEnrollStatusAsync([FromRoute] int enrollStatusId)
        {
            var response = await _enrollService.DeleteAsync(enrollStatusId);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{enrollStatusId}")]
        public async Task<IActionResult> UpdateEnrollStatusAsync([FromRoute] int enrollStatusId, [FromForm] EnrollStatusDto model)
        {
            var response = await _enrollService.UpdateAsync(enrollStatusId, model);

            return await response.ChangeActionAsync();
        }
    }
}
