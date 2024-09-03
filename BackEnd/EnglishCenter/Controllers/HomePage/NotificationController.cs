using AutoMapper;
using EnglishCenter.Database;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Models.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace EnglishCenter.Controllers.HomePage
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly EnglishCenterContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;

        public NotificationController(
            IHubContext<NotificationHub> hubContext,
            EnglishCenterContext context,
            IMapper mapper) 
        {
            _context = context;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("send-noti")]
        public async Task<IActionResult> SendNotification([FromBody] NotiModel model, [FromQuery] string groupName)
        {
            var group = _context.Groups
                                .Include(g => g.Students)
                                .FirstOrDefault(g => g.Name == groupName);

            if (group == null)
            {
                return NotFound(new
                {
                    Message = $"Can't find any group {groupName}"
                });
            }

            // Save notification in database
            var notiModel = _mapper.Map<Notification>(model);
            _context.Notifications.Add(notiModel);
            await _context.SaveChangesAsync();

            var newNotiModel = _context.Notifications
                                       .Include(g => g.Students)
                                       .OrderByDescending(n => n.Time)
                                       .FirstOrDefault(n => n.Title == model.Title);

            foreach (var student in group.Students)
            {
                newNotiModel.Students.Add(student);
            }

            await _context.SaveChangesAsync();
            var notiDto = _mapper.Map<NotiDtoModel>(newNotiModel);

            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveNotification", JsonConvert.SerializeObject(notiDto));
            return Ok();
        }
        
        [Authorize]
        [HttpGet("get-all-notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirst("Id")?.Value;

            var userNoties = await _context.Students
                                        .Include(s => s.Notifications)
                                        .FirstOrDefaultAsync(s => s.UserId == userId);

            if(userNoties == null)
            {
                return NotFound();
            }

            return Ok(JsonConvert.SerializeObject(_mapper.Map<List<NotiDtoModel>>(userNoties.Notifications.OrderByDescending(n => n.Time))));
        }

        [HttpPatch("mark-read/{notiId}")]
        public async Task<IActionResult> MarkReadNotification(long notiId)
        {
            var notification = await _context.Notifications.FindAsync(notiId);

            if (notification == null)
            {
                return NotFound();
            }

            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("mark-read-all")]
        public async Task<IActionResult> MarkReadAllNotification()
        {
            var userId = User.FindFirst("Id")?.Value;

            var userInfo = await _context.Students
                                   .Include(s => s.Notifications)
                                   .FirstOrDefaultAsync(s => s.UserId == userId);

            if(userInfo == null)
            {
                return NotFound();
            }

            foreach(var noti in userInfo.Notifications)
            {
                noti.IsRead = true;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
