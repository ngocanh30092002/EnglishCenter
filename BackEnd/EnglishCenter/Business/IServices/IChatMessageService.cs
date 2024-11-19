using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IChatMessageService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long messageId);
        public Task<Response> GetMessagesAsync(string senderId, string receiverId);
        public Task<Response> GetMembersInClassAsync(string senderId, string classId);
        public Task<Response> GetTeacherInClassAsync(string senderId, string classId);
        public Task<Response> CreateAsync(ChatMessageDto chatModel);
        public Task<Response> DeleteAsync(long messageId);
    }
}
