using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Hub;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.HomeRepositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationRepository(EnglishCenterContext context, IHubContext<NotificationHub> hubContext) : base(context)
        {
            _hubContext = hubContext;
        }

        public async Task<bool> CreateGroupAsync(string groupName)
        {
            var groupModel = await context.Groups.FirstOrDefaultAsync(g => g.Name == groupName);
            if (groupModel != null) return true;

            var model = new Group() { Name = groupName };
            await context.Groups.AddAsync(model);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SendNotificationToGroup(string groupName, NotiDto model)
        {
            var group = await context.Groups
                                     .Include(g => g.Students)
                                     .FirstOrDefaultAsync(g => g.Name == groupName);
            if (group == null) return false;

            var notiModel = new Notification()
            {
                Title = model.Title,
                Description = model.Description,
                Time = model.Time,
                Image = model.Image,
                LinkUrl = model.LinkUrl,
            };

            context.Notifications.Add(notiModel);

            foreach (var student in group.Students)
            {
                context.NotiStudents.Add(new NotiStudent
                {
                    IsRead = false,
                    Notification = notiModel,
                    UserId = student.UserId
                });
            }

            await _hubContext.Clients.Group(group.Name).SendAsync("ReceiveNotification");

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SendNotificationToUser(string userId, string groupName, NotiDto model)
        {
            var group = await context.Groups
                                     .Include(g => g.Students)
                                     .FirstOrDefaultAsync(g => g.Name == groupName);
            if (group == null) return false;

            var notiModel = new Notification()
            {
                Title = model.Title,
                Description = model.Description,
                Time = model.Time,
                Image = model.Image,
                LinkUrl = model.LinkUrl,
            };

            context.Notifications.Add(notiModel);
            context.NotiStudents.Add(new NotiStudent
            {
                IsRead = false,
                Notification = notiModel,
                UserId = userId
            });

            await _hubContext.Clients.Users(userId).SendAsync("ReceiveNotification");

            await context.SaveChangesAsync();
            return true;
        }
    }
}
