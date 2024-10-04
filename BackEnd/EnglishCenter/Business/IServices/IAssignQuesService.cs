using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IAssignQuesService
    {
        public Task<Response> GetAsync(long id);
        public Task<Response> GetAllAsync();
        public Task<Response> GetByAssignmentAsync(long assignmentId);
        public Task<Response> ChangeAssignmentIdAsync(long id, long assignmentId);
        public Task<Response> ChangeQuesAsync(long id, int type, long quesId);
        public Task<Response> CreateAsync(AssignQuesDto model);
        public Task<Response> UpdateAsync(long id, AssignQuesDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
