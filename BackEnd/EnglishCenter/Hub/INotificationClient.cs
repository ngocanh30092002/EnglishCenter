using EnglishCenter.Models;
using EnglishCenter.Models.RequestModel;

namespace EnglishCenter
{
    public interface INotificationClient
    {
        Task JoinGroup(string groupName);
        Task LeaveGroup(string groupName);
        Task ReceiveMessage(string groupName, string messsage);
        Task ReceiveNotification(Notification model);
        Task ReceiveError(string errorMessage);
    }
}
