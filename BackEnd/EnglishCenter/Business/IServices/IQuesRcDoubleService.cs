using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IQuesRcDoubleService
    {
        public Task<Response> GetAsync(long quesId);
        public Task<Response> GetOtherQuestionByAssignmentAsync(long assignmentId);
        public Task<Response> GetOtherQuestionByHomeworkAsync(long homeworkId);
        public Task<Response> GetAllAsync();
        public Task<Response> ChangeQuantityAsync(long quesId, int quantity);
        public Task<Response> ChangeTimeAsync(long quesId, TimeOnly timeOnly);
        public Task<Response> ChangeImage1Async(long quesId, IFormFile imageFile);
        public Task<Response> ChangeImage2Async(long quesId, IFormFile imageFile);
        public Task<Response> ChangeLevelAsync(long quesId, int level);
        public Task<Response> CreateAsync(QuesRcDoubleDto queModel);
        public Task<Response> UpdateAsync(long quesId, QuesRcDoubleDto queModel);
        public Task<Response> DeleteAsync(long quesId);
    }
}
