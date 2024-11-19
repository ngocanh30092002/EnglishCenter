using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.CoursePage
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClassesAsync()
        {
            var response = await _classService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetClassesWithCourseAsync([FromRoute] string courseId)
        {
            var response = await _classService.GetClassesWithCourseAsync(courseId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("{classId}")]
        public async Task<IActionResult> GetClassAsync([FromRoute] string classId)
        {
            var response = await _classService.GetAsync(classId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("teacher")]
        public async Task<IActionResult> GetClassesWithTeacherAsync([FromQuery] string teacherId)
        {
            var response = await _classService.GetClassesWithTeacherAsync(teacherId);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{classId}")]
        public async Task<IActionResult> UpdateClassAsync([FromRoute] string classId, [FromForm] ClassDto model)
        {
            if (model.Image != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(model.Image);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            var response = await _classService.UpdateAsync(classId, model);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{classId}")]
        public async Task<IActionResult> DeleteClassAsync([FromRoute] string classId)
        {
            var response = await _classService.DeleteAsync(classId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateClassAsync([FromForm] ClassDto model)
        {
            if(model.Image != null)
            {
                var isImageFile = await UploadHelper.IsImageAsync(model.Image);
                if (!isImageFile)
                {
                    return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
                }
            }

            var response = await _classService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{classId}/start-time")]
        public async Task<IActionResult> ChangeStartTimeAsync([FromRoute] string classId, [FromQuery] DateOnly startTime)
        {
            var response = await _classService.ChangeStartTimeAsync(classId, startTime);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{classId}/end-time")]
        public async Task<IActionResult> ChangeEndTimeAsync([FromRoute] string classId, [FromQuery] DateOnly endTime)
        {
            var response = await _classService.ChangeEndTimeAsync(classId, endTime);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{classId}/image")]
        public async Task<IActionResult> ChangeImageAsync([FromRoute] string classId, IFormFile image)
        {
            var isImageFile = await UploadHelper.IsImageAsync(image);
            if (!isImageFile)
            {
                return BadRequest(new { message = "The image file is invalid. Only JPEG, PNG, GIF, and SVG are allowed.", success = false });
            }

            var response = await _classService.ChangeImageAsync(classId, image);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{classId}/max-num")]
        public async Task<IActionResult> ChangeMaxNumAsync([FromRoute] string classId, [FromQuery] int maxNum)
        {
            var response = await _classService.ChangeMaxNumAsync(classId, maxNum);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{classId}/course")]
        public async Task<IActionResult> ChangeCourseAsync([FromRoute] string classId, [FromQuery] string courseId)
        {
            var response = await _classService.ChangeCourseAsync(classId, courseId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{classId}/des")]
        public async Task<IActionResult> ChangeDescriptionAsync([FromRoute] string classId, [FromBody] string newDes)
        {
            var response = await _classService.ChangeDescriptionAsync(classId, newDes);
            return await response.ChangeActionAsync();
        }
    }
}
