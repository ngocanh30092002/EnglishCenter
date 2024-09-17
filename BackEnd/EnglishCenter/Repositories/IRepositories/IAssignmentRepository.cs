using EnglishCenter.Models;
using EnglishCenter.Models.DTO;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IAssignmentRepository
    {
        public Task<Response> CreateAssignmentAsync(AssignmentDto model);
        public Task<Response> UpdateAssignmentAsync(long assignmentId, AssignmentDto model);
        public Task<Response> RemoveAssignmentAsync(long assignmentId);
        public Task<Response> GetAssignmentsAsync();
        public Task<Response> GetAssignmentsAsync(long contentId);
        public Task<Response> GetAssignmentAsync(long assignmentId);
        public Task<Response> GetNumberAssignmentsAsync(string courseId);
        public Task<Response> GetTotalTimeAssignmentsAsync(string courseId);
        public Task<Response> ChangeNoNumberAsync(long assignmentId, int number);
        public Task<Response> ChangeCourseContentAsync(long assignmentId, long contentId);
        public Task<Response> ChangeCourseContentTitleAsync(long assignmentId,  string title);
        public Task<Response> ChangeTimeAsync(long assignmentId, string time);
    }
}
