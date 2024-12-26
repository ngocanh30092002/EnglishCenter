using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.ClassPage
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClassSchedulesController : ControllerBase
    {
        private readonly IClassScheduleService _scheduleService;

        public ClassSchedulesController(IClassScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _scheduleService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var response = await _scheduleService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("classes/{classId}")]
        public async Task<IActionResult> GetByClassAsync([FromRoute] string classId)
        {
            var response = await _scheduleService.GetByClassAsync(classId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("day-of-week")]
        public async Task<IActionResult> GetDayOfWeekAsync()
        {
            var response = await _scheduleService.GetDayOfWeekAsync();
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAsync([FromForm] ClassScheduleDto model)
        {
            var response = await _scheduleService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("class/{classId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> HandleCreateLessonAsync([FromRoute] string classId)
        {
            var response = await _scheduleService.HandleCreateLessonAsync(classId);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] ClassScheduleDto model)
        {
            var response = await _scheduleService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/start")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeStartPeriodAsync([FromRoute] long id, [FromQuery] int start)
        {
            var response = await _scheduleService.ChangeStartPeriodAsync(id, start);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/end")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeEndPeriodAsync([FromRoute] long id, [FromQuery] int end)
        {
            var response = await _scheduleService.ChangeEndPeriodAsync(id, end);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/dayOfWeek")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeDayOfWeekAsync([FromRoute] long id, [FromQuery] int dayOfWeek)
        {
            var response = await _scheduleService.ChangeDayOfWeekAsync(id, dayOfWeek);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/class")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeClassAsync([FromRoute] long id, [FromQuery] string classId)
        {
            var response = await _scheduleService.ChangeClassAsync(id, classId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/class-room")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeClassRoomAsync([FromRoute] long id, [FromQuery] long classRoomId)
        {
            var response = await _scheduleService.ChangeClassRoomAsync(id, classRoomId);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _scheduleService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
