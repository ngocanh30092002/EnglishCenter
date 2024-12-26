using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        public Task<bool> CreateGroupAsync(string groupName);
        public Task<bool> SendNotificationToGroup(string groupName, NotiDto model);
        public Task<bool> SendNotificationToUser(string userId, string groupName, NotiDto model);
    }
}
