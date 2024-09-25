using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IStudentService
    {

        public Task<Response> GetStudentInfoAsync(string userId);
        public Task<Response> GetStudentBackgroundAsync(string userId);
        public Task<Response> ChangeStudentBackgroundAsync(string userId, StudentBackgroundDto stuModel);
        public Task<Response> ChangeStudentInfoAsync(string userId, StudentInfoDto model);
        public Task<Response> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        public Task<Response> ChangeBackgroundImageAsync(IFormFile file, string userId);
        public Task<Response> ChangeStudentImageAsync(IFormFile file, string userId);
    }
}
