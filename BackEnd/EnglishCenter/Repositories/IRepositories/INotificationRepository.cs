using EnglishCenter.Models;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface INotificationRepository
    {
        public Task<Response> GetNotificationsAsync(Notification model);
        public Task<bool> SendNotificationAsync();
    }
}
