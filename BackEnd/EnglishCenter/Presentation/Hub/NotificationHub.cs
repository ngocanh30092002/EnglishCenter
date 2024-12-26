using System.Security.Claims;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Global.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Presentation.Hub
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
                var userID = Context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var _student = _context.Students
                                       .Include(g => g.Groups)
                                       .FirstOrDefault(s => s.UserId == userID);

                var classIds = _context.Enrollments
                                       .Where(e => e.UserId == userID && e.StatusId == (int)EnrollEnum.Ongoing)
                                       .Select(e => e.ClassId)
                                       .ToList();




                var systemGroup = await _context.Groups.FirstOrDefaultAsync(g => g.Name == GlobalVariable.SYSTEM);
                if (systemGroup == null)
                {
                    systemGroup = new Group { Name = GlobalVariable.SYSTEM };
                    _context.Groups.Add(systemGroup);
                    await _context.SaveChangesAsync();
                }

                if (_student?.Groups == null || !_student.Groups.Any())
                {
                    _student.Groups = new List<Group>();
                }

                if (_student.Groups.All(g => g.Name != GlobalVariable.SYSTEM))
                {
                    _student.Groups.Add(systemGroup);
                    await _context.SaveChangesAsync();
                }

                foreach (var group in _student.Groups)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
                }

                foreach (var classId in classIds)
                {
                    var groupModel = _context.Groups.FirstOrDefault(g => g.Name == classId);
                    if (groupModel != null)
                    {
                        var isExist = _student.Groups.Any(s => s.Name == groupModel.Name);
                        if (!isExist)
                        {
                            _student.Groups.Add(groupModel);
                        }
                    }

                    await Groups.AddToGroupAsync(Context.ConnectionId, classId);
                }

                await _context.SaveChangesAsync();
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

        public async Task SendNotiToUserAsync()
        {
            var userID = Context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            await Clients.User(userID).ReceiveNotification();
        }

        public async Task JoinGroup(string groupName)
        {
            try
            {
                var userID = Context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
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
                                        .Include(g => g.Students)
                                        .FirstOrDefault(s => s.Name == groupName);

                    if (group != null && _student != null)
                    {
                        group.Students.Add(_student);
                        await _context.SaveChangesAsync();
                        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                    }
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.ReceiveError(ex.Message);
            }
        }

        public async Task LeaveGroup(string groupName)
        {
            try
            {
                var userID = Context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
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
