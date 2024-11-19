using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Presentation.Hub
{
    public interface IChatClient
    {
        Task Online(string userId);
        Task Offline(string userId);
        Task SendMessage(ChatMessageDto message);
        Task ReceiveMessage(ChatMessageResDto message);
        Task ReadMessage(string receiverId);
        Task ReadMessageForSender(string receiverId);
        Task ReadMessageForReceiver();
        Task RemoveMessage(ChatMessageResDto message);
        Task ReceiverError(string errorMessage);
    }
}
