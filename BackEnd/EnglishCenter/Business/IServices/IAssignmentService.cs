using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IAssignmentService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long assignmentId);
        public Task<Response> GetByCourseContentAsync(long courseContentId);
        public Task<Response> GetNumberByCourseAsync(string courseId);
        public Task<Response> GetTotalTimeByCourseAsync(string courseId);
        public Task<Response> CreateAsync(AssignmentDto model);
        public Task<Response> UpdateAsync(long assignmentId, AssignmentDto model);
        public Task<Response> DeleteAsync(long assignmentId);
        public Task<Response> ChangeNoNumAsync(long assignmentId, int number);
        public Task<Response> ChangeCourseContentAsync(long assignmentId, long contentId);
        public Task<Response> ChangeTitleAsync(long assignmentId,  string title);
        public Task<Response> ChangeTimeAsync(long assignmentId,  string time);
        public Task<Response> ChangePercentageAsync(long assignmentId, int percentage);
    }
}
