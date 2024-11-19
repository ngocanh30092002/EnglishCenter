using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;

namespace EnglishCenter.Business.Services.Chats
{
    public class ChatFileService : IChatFileService
    {
        private readonly IUnitOfWork _unit;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ChatFileService(IUnitOfWork unit, IWebHostEnvironment webHostEnvironment)
        {
            _unit = unit;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<Response> CreateAsync(string senderId, IFormFile file)
        {
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", senderId);

            var chatFile = new ChatFile();
            chatFile.FileName = file.FileName;
            chatFile.FileType = Path.GetExtension(file.FileName).Remove(0, 1);
            chatFile.FilePath = Path.Combine("uploads", senderId, file.FileName);

            var result = await UploadHelper.UploadFileAsync(file, folderPath, chatFile.FileName);
            if (!string.IsNullOrEmpty(result))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = result,
                    Success = false
                };
            }

            _unit.ChatFiles.Add(chatFile);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = chatFile.FileId,
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long id)
        {
            var chatFile = _unit.ChatFiles.GetById(id);
            if (chatFile == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any files",
                    Success = false
                };
            }

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, chatFile.FilePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _unit.ChatFiles.Remove(chatFile);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }
    }
}
