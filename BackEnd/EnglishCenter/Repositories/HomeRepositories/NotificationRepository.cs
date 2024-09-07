using EnglishCenter.Models;
using EnglishCenter.Repositories.IRepositories;

namespace EnglishCenter.Repositories.HomeRepositories
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
