using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        public Task<bool> ChangeStudentImageAsync(IFormFile file, Student student);
        public Task<bool> ChangeBackgroundImageAsync(IFormFile file, Student student);
        public Task<Response> ChangePasswordAsync(Student student, string currentPassword, string newPassword);
        public Task<Response> ChangeStudentInfoAsync(Student student, UserInfoDto model);
        public Task<bool> ChangeStudentBackgroundAsync(Student student, UserBackgroundDto stuModel);
    }
}
