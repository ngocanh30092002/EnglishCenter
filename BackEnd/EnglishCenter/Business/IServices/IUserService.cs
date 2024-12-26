using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IUserService
    {
        public Task<Response> GetAllAsync(string userId);
        public Task<Response> GetUsersWithRolesAsync();
        public Task<Response> GetUserBackgroundInfoAsync(string userId);
        public Task<Response> GetUserInfoAsync(string userId);
        public Task<Response> GetUserFullInfoAsync(string userId);
        public Task<Response> ChangeUserInfoAsync(string userId, UserInfoDto model);
        public Task<Response> ChangeUserBackgroundAsync(string userId, UserBackgroundDto stuModel);
        public Task<Response> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        public Task<Response> ChangePasswordAsync(string userId, string newPassword);
        public Task<Response> ChangeUserBackgroundImageAsync(string userId, IFormFile file);
        public Task<Response> ChangeUserImageAsync(string userId, IFormFile file);
        public Task<Response> UpdateAsync(UserDto model);
        public Task<Response> DeleteAsync(string userId);
    }
}
