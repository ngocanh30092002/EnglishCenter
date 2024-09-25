using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        public Task<Response> UpdateAsync(long enrollmentId, EnrollmentDto model);
        public Task<List<Enrollment>> GetAsync(string userId);
        public Task<List<Enrollment>> GetAsync(string userId, string classId);
        public Task<List<Enrollment>> GetAsync(string classId, EnrollEnum status);
        public Task<List<Enrollment>> GetByTeacherAsync(string userId);
        public Task<List<Enrollment>> GetByTeacherAsync(string userId, string classId);
        public Task<int> GetHighestPreScoreAsync(string userId, string courseId);
        public Task<bool> ChangeClassAsync(Enrollment enroll, string classId);
        public Task<bool> ChangeStatusAsync(Enrollment enroll, EnrollEnum status);
        public Task<bool> HandleAcceptedAsync(string classId);
        public Task<bool> HandleAcceptedAsync(Enrollment enroll);
        public Task<bool> HandleStartClassAsync(string classId, Course preCourse);
        public Task<bool> HandleEndClassAsync(string classId);
        public Task<bool> HandleRejectByTeacherAsync(Enrollment enrollModel);
        public Task<bool> ChangeDateAsync(Enrollment enroll, DateOnly time);
        public Task<bool> ChangeStudentAsync(Enrollment enroll, string userId);
    }
}
