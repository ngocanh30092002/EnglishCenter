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
        public Task<Response> ChangeStudentInfoAsync(Student student, StudentInfoDto model);
        public bool ChangeStudentBackground(Student student, StudentBackgroundDto stuModel);
        public Task<StudentInfoDto?> GetStudentInfoAsync(Student student);
        public Task<StudentBackgroundDto?> GetStudentBackgroundAsync(Student student);
    }
}
