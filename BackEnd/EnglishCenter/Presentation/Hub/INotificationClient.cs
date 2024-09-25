namespace EnglishCenter.Presentation.Hub
{
    public interface INotificationClient
    {
        Task JoinGroup(string groupName);
        Task LeaveGroup(string groupName);
        Task ReceiveMessage(string groupName, string messsage);
        Task ReceiveNotification();
        Task ReceiveError(string errorMessage);
    }
}
