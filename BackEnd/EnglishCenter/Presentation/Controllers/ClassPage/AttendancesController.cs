using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.ClassPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _attendanceService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var response = await _attendanceService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpGet("classes/{classId}")]
        public async Task<IActionResult> GetByClassAsync([FromRoute] string classId)
        {
            var response = await _attendanceService.GetByClassAsync(classId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("lessons/{lessonId}")]
        public async Task<IActionResult> GetByLessonAsync([FromRoute] long lessonId)
        {
            var response = await _attendanceService.GetByLessonAsync(lessonId);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAsync([FromForm] AttendanceDto model)
        {
            var response = await _attendanceService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPost("lessons/{lessonId}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> HandleCreateByLessonAsync([FromRoute] long lessonId)
        {
            var response = await _attendanceService.HandleCreateByLessonAsync(lessonId);
            return await response.ChangeActionAsync();
        }

        [HttpPut("lessons/{lessonId}/attended")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> HandleAttendedAllAsync([FromRoute] long lessonId)
        {
            var response = await _attendanceService.HandleAttendedAllAsync(lessonId);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] AttendanceDto model)
        {
            var response = await _attendanceService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/attended")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeIsAttendedAsync([FromRoute] long id, [FromBody] bool isAttended)
        {
            var response = await _attendanceService.ChangeIsAttendedAsync(id, isAttended);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/late")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeIsLateAsync([FromRoute] long id, [FromBody] bool isLate)
        {
            var response = await _attendanceService.ChangeIsLateAsync(id, isLate);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/permitted")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeIsPermittedAsync([FromRoute] long id, [FromBody] bool isPermitted)
        {
            var response = await _attendanceService.ChangeIsPermittedAsync(id, isPermitted);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/leaved")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeIsLeavedAsync([FromRoute] long id, [FromBody] bool isLeaved)
        {
            var response = await _attendanceService.ChangeIsLeavedAsync(id, isLeaved);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _attendanceService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
