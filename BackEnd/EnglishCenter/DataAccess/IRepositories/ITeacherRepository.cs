using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        public Task<bool> ChangeTeacherImageAsync(IFormFile file, Teacher teacher);
        public Task<bool> ChangeBackgroundImageAsync(IFormFile file, Teacher teacher);
        public Task<Response> ChangeTeacherInfoASync(Teacher teacher, UserInfoDto model);
        public Task<Response> ChangePasswordAsync(Teacher teacher, string currentPassword, string newPassword);
        public Task<bool> ChangeTeacherBackgroundAsync(Teacher teacher, UserBackgroundDto stuModel);
        public Task<string> GetFullNameAsync(string userId);
        public string GetFullName(Teacher teacher);
    }
}
