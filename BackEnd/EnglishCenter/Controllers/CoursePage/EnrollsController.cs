using EnglishCenter.Global.Enum;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers.CoursePage
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollsController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollRepo;

        public EnrollsController(IEnrollmentRepository enrollRepo)
        {
            _enrollRepo = enrollRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetEnrollmentsAsync()
        {
            var response = await _enrollRepo.GetEnrollmentsAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("users/{userId}/classes/{classId}")]
        public async Task<IActionResult> GetEnrollmentsAsync([FromRoute] string userId, [FromRoute] string classId)
        {
            var response = await _enrollRepo.GetEnrollmentsAsync(userId, classId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("classes/{classId}/{statusId}")]
        public async Task<IActionResult> GetEnrollmentsWithStatusAsync([FromRoute] string classId, [FromRoute] int statusId)
        {
            if(Enum.IsDefined(typeof(EnrollStatus), statusId)){
                var response = await _enrollRepo.GetEnrollmentsWithStatusAsync(classId, (EnrollStatus)statusId);

                return await response.ChangeActionAsync();
            }
            else
            {
                return BadRequest("StatusId isn't valid");
            }
        }

        [HttpGet("{enrollmentId}")]
        public async Task<IActionResult> GetEnrollmentAsync([FromRoute] long enrollmentId)
        {
            var response = await _enrollRepo.GetEnrollmentAsync(enrollmentId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EnrollInClassAsync([FromForm] EnrollmentDto model)
        {
            var userId = User.FindFirst("Id")?.Value ?? "";
            model.UserId = userId;

            var response = await _enrollRepo.EnrollInClassAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{enrollmentId}")]
        public async Task<IActionResult> UpdateEnrollmentAsync([FromRoute] long enrollmentId,[FromForm] EnrollmentDto model)
        {
            var response = await _enrollRepo.UpdateEnrollmentAsync(enrollmentId, model);

            return await response.ChangeActionAsync();
        }

        [HttpDelete("{enrollmentId}")]
        public async Task<IActionResult> DeleteEnrollmentAsync([FromRoute] long enrollmentId)
        {
            var response = await _enrollRepo.DeleteEnrollmentAsync(enrollmentId);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("{enrollmentId}/class/{classId}")]
        public async Task<IActionResult> ChangeClassAsync([FromRoute] long enrollmentId, string classId)
        {
            var response = await _enrollRepo.ChangeClassAsync(enrollmentId, classId);

            return await response.ChangeActionAsync();
        }

        [HttpPatch("{enrollmentId}/status/{statusId}")]
        public async Task<IActionResult> ChangeClassAsync([FromRoute] long enrollmentId, int statusId)
        {
            var response = await _enrollRepo.ChangeStatusAsync(enrollmentId, statusId);

            return await response.ChangeActionAsync();
        }
    }
}
