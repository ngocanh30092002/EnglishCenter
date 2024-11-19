using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IChatMessageRepository : IGenericRepository<ChatMessage>
    {
        public Task<long> SendMessageAsync(ChatMessageDto chatModel);
        public Task<bool> ReadMessageAsync(string senderId, string receiverId);
        public Task<bool> RemoveMessageAsync(long messageId);
        public Task<List<ChatMessage>?> GetFullMessageAsync(string senderId, string receiverId);
    }
}
