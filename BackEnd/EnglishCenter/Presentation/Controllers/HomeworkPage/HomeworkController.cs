using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.HomeworkPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeworkController : ControllerBase
    {
        private readonly IHomeworkService _homeService;
        private readonly ILessonService _lessonService;

        public HomeworkController(IHomeworkService homeService, ILessonService lessonService)
        {
            _homeService = homeService;
            _lessonService = lessonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _homeService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var response = await _homeService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("lessons/{lessonId}")]
        public async Task<IActionResult> GetByLessonAsync([FromRoute] long lessonId)
        {
            var response = await _homeService.GetByLessonAsync(lessonId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("classes/{classId}")]
        public async Task<IActionResult> GetCurrentByClassAsync([FromRoute] string classId)
        {
            var res = await _homeService.GetCurrentByClassAsync(classId);
            return await res.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAsync([FromForm] HomeworkDto model)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var res = await _lessonService.IsInChargeOfClassAsync(model.LessonId, userId);
                if (res.Success == false || Convert.ToBoolean(res.Message) == false)
                {
                    return Forbid("You aren't in charge of this class so you can't access it.");
                }
            }

            if (model.Image != null)
            {
                var isImage = await UploadHelper.IsImageAsync(model.Image);
                if (!isImage)
                {
                    return BadRequest("Image is not correct format");
                }
            }

            var response = await _homeService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] HomeworkDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var isTeacher = User.IsInRole(AppRole.TEACHER);

            if (isTeacher)
            {
                var res = await _lessonService.IsInChargeOfClassAsync(model.LessonId, userId);
                if (res.Success == false || Convert.ToBoolean(res.Message) == false)
                {
                    return Forbid("You aren't in charge of this class so you can't access it.");
                }
            }

            if (model.Image != null)
            {
                var isImage = await UploadHelper.IsImageAsync(model.Image);
                if (!isImage)
                {
                    return BadRequest("Image is not correct format");
                }
            }

            var response = await _homeService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-start-time")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeStartTimeAsync([FromRoute] long id, [FromBody] string startTime)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var isInCharge = await _homeService.IsInChargeClass(userId, id);
                if (!isInCharge)
                {
                    return Forbid("You aren't in charge of this class so you can't access it.");
                }
            }

            var response = await _homeService.ChangeStartTimeAsync(id, startTime);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-image")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeImageAsync([FromRoute] long id, IFormFile file)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var isInCharge = await _homeService.IsInChargeClass(userId, id);
                if (!isInCharge)
                {
                    return Forbid("You aren't in charge of this class so you can't access it.");
                }
            }

            var isImage = await UploadHelper.IsImageAsync(file);
            if (!isImage)
            {
                return BadRequest("Image is not correct format");
            }

            var response = await _homeService.ChangeImageAsync(id, file);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-end-time")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeEndTimeAsync([FromRoute] long id, [FromBody] string endTime)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var isInCharge = await _homeService.IsInChargeClass(userId, id);
                if (!isInCharge)
                {
                    return Forbid("You aren't in charge of this class so you can't access it.");
                }
            }

            var response = await _homeService.ChangeEndTimeAsync(id, endTime);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/lesson")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeLessonAsync([FromRoute] long id, [FromQuery] long lessonId)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var isInCharge = await _homeService.IsInChargeClass(userId, id);
                if (!isInCharge)
                {
                    return Forbid("You aren't in charge of this class so you can't access it.");
                }
            }

            var response = await _homeService.ChangeLessonAsync(id, lessonId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-late-days")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeLateSubmitDaysAsync([FromRoute] long id, [FromQuery] int days)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var isInCharge = await _homeService.IsInChargeClass(userId, id);
                if (!isInCharge)
                {
                    return Forbid("You aren't in charge of this class so you can't access it.");
                }
            }

            var response = await _homeService.ChangeLateSubmitDaysAsync(id, days);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-percentage")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangePercentageAsync([FromRoute] long id, [FromQuery] int percentage)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var isInCharge = await _homeService.IsInChargeClass(userId, id);
                if (!isInCharge)
                {
                    return Forbid("You aren't in charge of this class so you can't access it.");
                }
            }

            var response = await _homeService.ChangePercentageAsync(id, percentage);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-time")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeTimeAsync([FromRoute] long id, [FromBody] string time)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var isInCharge = await _homeService.IsInChargeClass(userId, id);
                if (!isInCharge)
                {
                    return Forbid("You aren't in charge of this class so you can't access it.");
                }
            }

            var response = await _homeService.ChangeTimeAsync(id, time);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/change-title")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeTitleAsync([FromRoute] long id, [FromBody] string title)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var isInCharge = await _homeService.IsInChargeClass(userId, id);
                if (!isInCharge)
                {
                    return Forbid("You aren't in charge of this class so you can't access it.");
                }
            }

            var response = await _homeService.ChangeTimeAsync(id, title);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var isTeacher = User.IsInRole(AppRole.TEACHER);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (isTeacher)
            {
                var isInCharge = await _homeService.IsInChargeClass(userId, id);
                if (!isInCharge)
                {
                    return Forbid("You aren't in charge of this class so you can't access it.");
                }
            }

            var response = await _homeService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
