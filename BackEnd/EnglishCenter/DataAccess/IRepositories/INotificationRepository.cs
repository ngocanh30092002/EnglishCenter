using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface INotificationRepository
    {
        public Task<Response> GetNotificationsAsync(Notification model);
        public Task<bool> SendNotificationAsync();
    }
}
