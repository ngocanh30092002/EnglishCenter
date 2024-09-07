using EnglishCenter.Database;
using EnglishCenter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter
{
    [Authorize]
    public class NotificationHub : Hub<INotificationClient>
    {
        private readonly EnglishCenterContext _context;

        public NotificationHub(EnglishCenterContext context) 
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var userID = Context.User.Claims.First(c => c.Type == "Id").Value;
                var _student = _context.Students
                                       .Include(g => g.Groups)     
                                       .FirstOrDefault(s => s.UserId == userID);

                if (_student?.Groups != null)
                {
                    foreach (var group in _student.Groups)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
                    }
                }

                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                await Clients.Caller.ReceiveError(ex.Message);
            }
        }

        public async Task SendMessage(string groupName, string message)
        {
            await Clients.Group(groupName).ReceiveMessage(groupName, message);
        }

        public async Task SendNotiToGroup(string groupName)
        {
            await Clients.Group(groupName).ReceiveNotification();
        }

        public async Task JoinGroup(string groupName)
        {
            try
            {
                var userID = Context.User.Claims.First(c => c.Type == "Id").Value;
                var _student = _context.Students
                                       .Include(s => s.Groups)                   
                                       .FirstOrDefault(s => s.UserId == userID);

                if (_student == null)
                {
                    await Clients.Caller.ReceiveError("Can't find any user");
                    return;
                }

                bool isExistGroup = _context.Groups.Any(g => g.Name == groupName);

                if (!_student.Groups.Any())
                {
                    if (!isExistGroup)
                    {
                        var newGroup = new Group()
                        {
                            Name = groupName,
                        };
                        _context.Groups.Add(newGroup);
                        await _context.SaveChangesAsync();
                    }

                    var group = _context.Groups
                                        .Include(g=>g.Students)
                                        .FirstOrDefault(s => s.Name == groupName);
                    
                    if (group != null && _student != null)
                    {
                        group.Students.Add(_student);
                        await _context.SaveChangesAsync();
                        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                    }
                }
            }
            catch(Exception ex)
            {
                await Clients.Caller.ReceiveError(ex.Message);
            }
        }

        public async Task LeaveGroup(string groupName)
        {
            try
            {
                var userID = Context.User.Claims.First(c => c.Type == "Id").Value;
                var _student = _context.Students
                                       .Include(s => s.Groups)
                                       .FirstOrDefault(s => s.UserId == userID);

                if (_student == null)
                {
                    await Clients.Caller.ReceiveError("Can't find any user");
                    return;
                }

                bool isExistGroup = _student.Groups.Any((s) => s.Name == groupName);
                if (!_student.Groups.Any() || !isExistGroup) return;


                var group = _context.Groups
                                    .Include(g => g.Students)
                                    .FirstOrDefault(s => s.Name == groupName);

                group.Students.Remove(_student);
                await _context.SaveChangesAsync();

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }
            catch (Exception ex)
            {
                await Clients.Caller.ReceiveError(ex.Message);
            }
            
        }
    }
}
