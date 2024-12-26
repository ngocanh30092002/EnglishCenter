using System.Globalization;
using System.Security.Claims;
using EnglishCenter.Business.IServices;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Presentation.Controllers.HomePage
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetScheduleEventsAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var response = await _eventService.GetEventsAsync(userId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("{scheduleEventId}")]
        public async Task<IActionResult> GetScheduleEventAsync([FromRoute] long scheduleEventId)
        {
            var response = await _eventService.GetEventAsync(scheduleEventId);

            return await response.ChangeActionAsync();
        }

        [HttpPut("{scheduleEventId}/send-noti")]
        public async Task<IActionResult> SendTaskInfoToNotiAsync([FromRoute] long scheduleEventId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            if (userId == "")
            {
                BadRequest();
            }
            var response = await _eventService.SendTaskInfoToNotiAsync(userId, scheduleEventId);
            return await response.ChangeActionAsync();
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetScheduleEventsWithDateAsync([FromRoute] string date)
        {
            string formatDate = "yyyy-MM-dd";

            bool isValid = DateOnly.TryParseExact(
                            date,
                            formatDate,
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateOnly parsedDate);

            if (!isValid)
            {
                return BadRequest("Invalid date format. Please use yyyy-MM-dd.");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var response = await _eventService.GetEventsWithDateAsync(userId, parsedDate);

            return await response.ChangeActionAsync();
        }

        [HttpGet("date/{startTime}/{endTime}")]
        public async Task<IActionResult> GetNotificationsEventsInRange([FromRoute] string startTime, [FromRoute] string endTime)
        {
            string formatDate = "yyyy-MM-dd";

            bool isValidStartTime = DateOnly.TryParseExact(
                            startTime,
                            formatDate,
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateOnly startDate);

            bool isValidEndTime = DateOnly.TryParseExact(
                            endTime,
                            formatDate,
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateOnly endDate);

            if (!isValidStartTime || !isValidEndTime)
            {
                return BadRequest("Invalid date format. Please use yyyy-MM-dd.");
            }

            var response = await _eventService.GetEventsInRangeAsync(startDate, endDate);

            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateScheduleEventAsync([FromForm] EventDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var response = await _eventService.CreateAsync(userId, model);

            return await response.ChangeActionAsync();

        }

        [HttpPut("{eventId}")]
        public async Task<IActionResult> UpdateScheduleEventAsync([FromRoute] long eventId, [FromForm] EventDto model)
        {
            var response = await _eventService.UpdateAsync(eventId, model);

            return await response.ChangeActionAsync();
        }

        [HttpDelete("{eventId}")]
        public async Task<IActionResult> RemoveScheduleEventAsync([FromRoute] long eventId)
        {
            var response = await _eventService.DeleteAsync(eventId);

            return await response.ChangeActionAsync();
        }
    }
}
