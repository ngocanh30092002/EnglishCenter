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
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _lessonService.GetAllAsync();
            return await response.ChangeActionAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var response = await _lessonService.GetAsync(id);
            return await response.ChangeActionAsync();
        }

        [HttpPost]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> CreateAsync([FromForm] LessonDto model)
        {
            var response = await _lessonService.CreateAsync(model);
            return await response.ChangeActionAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id, [FromForm] LessonDto model)
        {
            var response = await _lessonService.UpdateAsync(id, model);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/class")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeClassAsync([FromRoute] long id, [FromQuery] string classId)
        {
            var response = await _lessonService.ChangeClassAsync(id, classId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/topic")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeTopicAsync([FromRoute] long id, [FromBody] string topic)
        {
            var response = await _lessonService.ChangeTopicAsync(id, topic);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/date")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeDateAsync([FromRoute] long id, [FromBody] string dateStr)
        {
            var response = await _lessonService.ChangeDateAsync(id, dateStr);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/class-room")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeClassRoomAsync([FromRoute] long id, [FromQuery] long classRoomId)
        {
            var response = await _lessonService.ChangeClassRoomAsync(id, classRoomId);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/start")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeStartPeriodAsync([FromRoute] long id, [FromQuery] int start)
        {
            var response = await _lessonService.ChangeStartPeriodAsync(id, start);
            return await response.ChangeActionAsync();
        }

        [HttpPatch("{id}/end")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> ChangeEndPeriodAsync([FromRoute] long id, [FromQuery] int end)
        {
            var response = await _lessonService.ChangeEndPeriodAsync(id, end);
            return await response.ChangeActionAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = GlobalVariable.ADMIN_TEACHER)]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _lessonService.DeleteAsync(id);
            return await response.ChangeActionAsync();
        }
    }
}
