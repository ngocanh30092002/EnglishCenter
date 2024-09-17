using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers.CoursePage
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollStatusController : ControllerBase
    {
        private readonly IEnrollStatusRepository _enrollRepo;

        public EnrollStatusController(IEnrollStatusRepository enrollRepo)
        {
            _enrollRepo = enrollRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEnrollStatusAsync()
        {
            var response = await _enrollRepo.GetAllEnrollStatusAsync();

            return await response.ChangeActionAsync();
        }

        [HttpGet("{enrollStatusId}")]
        public async Task<IActionResult> GetEnrollStatusAsync([FromRoute] int enrollStatusId  )
        {
            var response = await _enrollRepo.GetEnrollStatusAsync(enrollStatusId);
            
            return await response.ChangeActionAsync();  
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrollStatusAsync([FromForm] EnrollStatusDto model)
        {
            var response = _enrollRepo.CreateEnrollStatus(model);

            return await response.ChangeActionAsync();
        }

        [HttpDelete("{enrollStatusId}")]
        public async Task<IActionResult> DeleteEnrollStatusAsync([FromRoute] int enrollStatusId)
        {
            var response = await _enrollRepo.DeleteEnrollStatusAsync(enrollStatusId);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{enrollStatusId}")]
        public async Task<IActionResult> UpdateEnrollStatusAsync([FromRoute] int enrollStatusId, [FromForm] EnrollStatusDto model)
        {
            var response = await _enrollRepo.UpdateEnrollStatusAsync(enrollStatusId, model);

            return await response.ChangeActionAsync();
        }
    }
}
