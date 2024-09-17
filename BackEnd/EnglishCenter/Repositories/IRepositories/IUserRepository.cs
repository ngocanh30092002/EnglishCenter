
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IUserRepository
    {
        public Task<Response> GetUserInfo(string userId);
        public Task<Response> GetUserBackground(string userId);
        public Task<Response> ChangeUserInfoAsync(string userId, UserInfoDto stuModel);
        public Task<Response> ChangeUserBackgroundAsync(string userId, UserBackgroundDto stuModel);
        public Task<Response> ChangePasswordAsync(string userId,string currentPassword, string newPassword);
        public Task<Response> ChangeUserImageAsync(IFormFile file, string userId);
        public Task<Response> ChangeBackgroundImageAsync(IFormFile file, string userId);
    }
}
