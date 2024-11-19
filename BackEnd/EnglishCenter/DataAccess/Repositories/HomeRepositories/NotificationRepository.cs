using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Models;

namespace EnglishCenter.DataAccess.Repositories.HomeRepositories
{
    public class NotificationRepository : INotificationRepository
    {
        public Task<Response> GetNotificationsAsync(Notification model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendNotificationAsync()
        {
            throw new NotImplementedException();
        }
    }
}
