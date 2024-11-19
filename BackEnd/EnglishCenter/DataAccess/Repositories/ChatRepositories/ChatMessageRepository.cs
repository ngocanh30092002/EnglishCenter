using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.ChatRepositories
{
    public class ChatMessageRepository : GenericRepository<ChatMessage>, IChatMessageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ChatMessageRepository(EnglishCenterContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<ChatMessage>?> GetFullMessageAsync(string senderId, string receiverId)
        {
            var sender = await context.Users.FindAsync(senderId);
            if (sender == null) return null;

            var receiver = await context.Users.FindAsync(receiverId);
            if (receiver == null) return null;

            var senderMessages = context.ChatMessages
                                        .Include(m => m.ChatFile)
                                        .Where(m => m.SenderId == senderId && m.ReceiverId == receiverId)
                                        .ToList();

            var receiveMessages = context.ChatMessages
                                        .Include(m => m.ChatFile)
                                        .Where(m => m.ReceiverId == senderId && m.SenderId == receiverId)
                                        .ToList();

            var result = new List<ChatMessage>();

            result.AddRange(senderMessages);
            result.AddRange(receiveMessages);

            return result.OrderBy(r => r.SendAt).ToList();
        }

        public async Task<bool> ReadMessageAsync(string senderId, string receiverId)
        {
            var receiver = await context.Users.FindAsync(receiverId);
            if (receiver == null) return false;

            var messageModels = context.ChatMessages.Where(m => m.ReceiverId == receiverId && m.SenderId == senderId);

            foreach (var item in messageModels)
            {
                item.IsRead = true;
            }

            return true;
        }

        public async Task<bool> RemoveMessageAsync(long messageId)
        {
            var messageModel = await context.ChatMessages.FindAsync(messageId);
            if (messageModel == null) return false;

            messageModel.IsDelete = true;

            if (messageModel.FileId.HasValue)
            {
                var chatFile = await context.ChatFiles.FindAsync(messageModel.FileId.Value);

                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, chatFile!.FilePath);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                messageModel.FileId = null;

                context.ChatFiles.Remove(chatFile);
            }

            return true;
        }

        public async Task<long> SendMessageAsync(ChatMessageDto chatModel)
        {
            if (chatModel == null) return -1;

            var sender = await context.Users.FindAsync(chatModel.SenderId);
            if (sender == null) return -1;

            var receiver = await context.Users.FindAsync(chatModel.ReceiverId);
            if (receiver == null) return -1;

            var chatMessage = new ChatMessage();
            chatMessage.Sender = sender;
            chatMessage.Receiver = receiver;
            chatMessage.Message = chatModel.Message;
            chatMessage.SendAt = DateTime.Now;
            chatMessage.IsRead = false;
            chatMessage.IsDelete = false;


            if (chatModel.file != null)
            {
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", sender.Id);

                var chatFile = new ChatFile();
                chatFile.FileName = chatModel.file.FileName;
                chatFile.FileType = Path.GetExtension(chatModel.file.FileName).Remove(0, 1);
                chatFile.FilePath = Path.Combine("uploads", sender.Id, chatModel.file.FileName);

                var result = await UploadHelper.UploadFileAsync(chatModel.file, folderPath, chatFile.FileName);
                if (!string.IsNullOrEmpty(result)) return -1;

                context.ChatFiles.Add(chatFile);

                chatMessage.ChatFile = chatFile;
            }

            if (chatModel.FileId.HasValue)
            {
                var fileModel = await context.ChatFiles.FindAsync(chatModel.FileId.Value);
                if (fileModel == null) return -1;
                chatMessage.ChatFile = fileModel;
            }

            context.ChatMessages.Add(chatMessage);
            await context.SaveChangesAsync();

            return chatMessage.MessageId;
        }
    }
}
