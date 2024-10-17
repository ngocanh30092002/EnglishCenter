using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAssignmentRepository : IGenericRepository<Assignment>
    {
        public Task<Response> UpdateAsync(long assignmentId, AssignmentDto model);
        public Task<List<Assignment>> GetByCourseAsync(string courseId);
        public Task<List<Assignment>> GetByCourseContentAsync(long contentId);
        public Task<int> GetNumberByCourseAsync(string courseId);
        public Task<string> GetTotalTimeByCourseAsync(string courseId);
        public Task<Assignment?> GetPreviousAssignmentAsync(long id);
        public Task<bool> ChangeNoNumberAsync(Assignment assignmentModel, int number);
        public Task<bool> ChangeCourseContentAsync(Assignment assignmentModel, long contentId);
        public Task<bool> ChangeTitleAsync(Assignment assignmentModel, string title);
        public Task<bool> ChangeTimeAsync(Assignment assignmentModel, TimeOnly time);
    }
}
