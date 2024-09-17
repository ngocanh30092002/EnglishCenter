using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers.CoursePage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassRepository _classRepo;

        public ClassesController(IClassRepository classRepo) 
        {
            _classRepo = classRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetClassesAsync()
        {
            var response = await _classRepo.GetClassesAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{classId}")]
        public async Task<IActionResult> GetClassAsync([FromRoute] string classId)
        {
            var response = await _classRepo.GetClassAsync(classId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("teacher")]
        public async Task<IActionResult> GetClassesWithTeacherAsync([FromQuery] string teacherId)
        {
            var response = await _classRepo.GetClassesWithTeacherAsync(teacherId);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{classId}")]
        public async Task<IActionResult> UpdateClassAsync([FromRoute] string classId,[FromForm] ClassDto model)
        {
            var response = await _classRepo.UpdateClassAsync(classId, model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{classId}")]
        public async Task<IActionResult> DeleteClassAsync([FromRoute] string classId)
        {
            var response = await _classRepo.DeleteClassAsync(classId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateClassAsync([FromForm] ClassDto model)
        {
            var response = await _classRepo.CreateClassAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{classId}/start-time")]
        public async Task<IActionResult> ChangeStartTimeAsync([FromRoute] string classId, [FromQuery] DateOnly startTime)
        {
            var response = await _classRepo.ChangeStartTimeAsync(classId, startTime);
            return await response.ChangeActionAsync();
        }
        
        [HttpPatch("{classId}/end-time")]
        public async Task<IActionResult> ChangeEndTimeAsync([FromRoute] string classId, [FromQuery] DateOnly endTime)
        {
            var response = await _classRepo.ChangeEndTimeAsync(classId, endTime);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{classId}/image")]
        public async Task<IActionResult> ChangeImageAsync([FromRoute] string classId, IFormFile image)
        {
            var response = await _classRepo.ChangeImageAsync(classId, image);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{classId}/max-num")]
        public async Task<IActionResult> ChangeMaxNumAsync([FromRoute] string classId, [FromQuery] int maxNum)
        {
            var response = await _classRepo.ChangeMaxNumAsync(classId, maxNum);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{classId}/course")]
        public async Task<IActionResult> ChangeCourseAsync([FromRoute] string classId, [FromQuery] string courseId)
        {
            var response = await _classRepo.ChangeCourseAsync(classId, courseId);
            return await response.ChangeActionAsync();
        }
    }
}
