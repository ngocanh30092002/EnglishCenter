using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IEnrollmentService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long enrollmentId);
        public Task<Response> GetAsync(string userId);
        public Task<Response> GetAsync(string classId, int statusId);
        public Task<Response> GetAsync(string userId, string classId);
        public Task<Response> GetByTeacherAsync(string userId);
        public Task<Response> GetByTeacherAsync(string userId, string classId);
        public Task<Response> HandleStartClassAsync(string classId);
        public Task<Response> HandleEndClassAsync(string classId);
        public Task<Response> HandleAcceptedAsync(long enrollmentId);
        public Task<Response> HandleAcceptedAsync(string classId);
        public Task<Response> HandleRejectAsync(long enrollmentId);
        public Task<Response> HandleRejectByTeacherAsync(long enrollmentId);
        public Task<Response> HandleChangeClassAsync(long enrollmentId, string classId);
        public Task<Response> CreateAsync(EnrollmentDto model);
        public Task<Response> UpdateAsync(long enrollmentId, EnrollmentDto model);
        public Task<Response> DeleteAsync(long enrollmentId);
        public Task<bool> IsEnrollmentOfStudentAsync(string userId, long enrollmentId);

    }
}
