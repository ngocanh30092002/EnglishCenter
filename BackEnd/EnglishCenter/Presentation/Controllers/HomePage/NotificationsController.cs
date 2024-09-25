using System.Security.Claims;
using AutoMapper;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Hub;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EnglishCenter.Presentation.Controllers.HomePage
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly EnglishCenterContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;

        public NotificationsController(
            IHubContext<NotificationHub> hubContext,
            EnglishCenterContext context,
            IMapper mapper)
        {
            _context = context;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpPost("")]
        public async Task<IActionResult> SendNotification([FromBody] NotiDto model, [FromQuery] string groupName)
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
                                    .OrderByDescending(n => n.Time)
                                    .FirstOrDefault(n => n.Title == model.Title);
            foreach (var student in group.Students)
            {
                _context.NotiStudents.Add(new NotiStudent()
                {
                    IsRead = false,
                    Student = student,
                    Notification = newNotiModel
                });
            }

            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveNotification");
            return Ok();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userNoties = await _context.NotiStudents
                                        .Where(ns => ns.UserId == userId)
                                        .Include(ns => ns.Notification)
                                        .Select(ns => new NotiDto
                                        {
                                            NotiStuId = ns.NotiStuId,
                                            Title = ns.Notification.Title,
                                            Description = ns.Notification.Description,
                                            Image = ns.Notification.Image,
                                            IsRead = ns.IsRead,
                                            LinkUrl = ns.Notification.LinkUrl,
                                            Time = ns.Notification.Time
                                        })
                                        .ToListAsync();

            if (userNoties == null)
            {
                return NotFound();
            }

            return Ok(JsonConvert.SerializeObject(userNoties.OrderByDescending(un => un.Time)));

        }

        [HttpPatch("{notiStuId}/read")]
        public async Task<IActionResult> MarkReadNotification(long notiStuId)
        {
            var notiStudent = await _context.NotiStudents.FindAsync(notiStuId);

            if (notiStudent == null)
            {
                return NotFound();
            }

            notiStudent.IsRead = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("read-all")]
        public async Task<IActionResult> MarkReadAllNotification()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var notiStudents = await _context.NotiStudents
                                   .Where(ns => ns.UserId == userId)
                                   .ToListAsync();

            if (notiStudents == null || !notiStudents.Any())
            {
                return NotFound();
            }

            foreach (var noti in notiStudents)
            {
                noti.IsRead = true;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
