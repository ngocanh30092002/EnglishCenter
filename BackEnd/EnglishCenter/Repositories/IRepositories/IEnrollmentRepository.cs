using EnglishCenter.Models;
using EnglishCenter.Models.DTO;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IEnrollmentRepository
    {
        public Task<Response> EnrollInClassAsync(EnrollmentDto model);
        public Task<Response> DeleteEnrollmentAsync(long enrollmentId);
        public Task<Response> UpdateEnrollmentAsync(long enrollmentId, EnrollmentDto model);
        public Task<Response> GetEnrollmentsAsync();
        public Task<Response> GetEnrollmentsAsync(string userId, string classId);
        public Task<Response> GetEnrollmentsWithStatusAsync(string classId, Global.Enum.EnrollStatus status);
        public Task<Response> GetEnrollmentAsync(long enrollmentId);
        public Task<Response> ChangeStatusAsync(long enrollmentId, int statusId);
        public Task<Response> ChangeClassAsync(long enrollmentId, string classId);
        public Task<Response> ChangeStatusWithClassAsync(string classId);

    }
}
