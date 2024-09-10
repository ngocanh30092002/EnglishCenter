using System.Globalization;
using AutoMapper;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EnglishCenter.Controllers.HomePage
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepo;

        public EventsController(IEventRepository eventRepo) 
        {
            _eventRepo = eventRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetScheduleEventsAsync()
        {
            var userId = User.FindFirst("Id")?.Value ?? "";
            var response = await _eventRepo.GetScheduleEventsAsync(userId);

            return await response.ChangeActionAsync();
        }

        [HttpGet("{scheduleEventId}")]
        public async Task<IActionResult> GetScheduleEventAsync([FromRoute] long scheduleEventId)
        {
            var response = await _eventRepo.GetScheduleEventAsync( scheduleEventId);

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

            if(!isValid)
            {
                return BadRequest("Invalid date format. Please use yyyy-MM-dd.");
            }

            var userId = User.FindFirst("Id")?.Value ?? "";
            var response = await _eventRepo.GetScheduleEventsWithDateAsync(userId, parsedDate);

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

            var response = await _eventRepo.GetNotificationEventsInRange(startDate, endDate);

            return await response.ChangeActionAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateScheduleEventAsync([FromForm] EventDtoModel model)
        {
            var userId = User.FindFirst("Id")?.Value ?? "";
            var response = await _eventRepo.CreateScheduleEventAsync(userId, model);

            return await response.ChangeActionAsync();

        }

        [HttpPut("{eventId}")]
        public async Task<IActionResult> UpdateScheduleEventAsync([FromRoute] long eventId, [FromForm] EventDtoModel model)
        {
            model.ScheduleId = eventId;

            var response = await _eventRepo.UpdateScheduleEventAsync(model);

            return await response.ChangeActionAsync();
        }

        [HttpDelete("{eventId}")]
        public async Task<IActionResult> RemoveScheduleEventAsync([FromRoute] long eventId)
        {
            var response = await _eventRepo.RemoveScheduleEventAsync(eventId);

            return await response.ChangeActionAsync();
        }
    }
}
