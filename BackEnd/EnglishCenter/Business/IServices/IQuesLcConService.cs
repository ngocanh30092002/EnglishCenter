using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IQuesLcConService
    {
        public Task<Response> GetAsync(long quesId);
        public Task<Response> GetAllAsync();
        public Task<Response> GetOtherQuestionByAssignmentAsync(long assignmentId);
        public Task<Response> GetOtherQuestionByHomeworkAsync(long homeworkId);
        public Task<Response> ChangeQuantityAsync(long quesId, int quantity);
        public Task<Response> ChangeAudioAsync(long quesId, IFormFile audioFile);
        public Task<Response> ChangeImageAsync(long quesId, IFormFile imageFile);
        public Task<Response> ChangeLevelAsync(long quesId, int level);
        public Task<Response> CreateAsync(QuesLcConDto queModel);
        public Task<Response> UpdateAsync(long quesId, QuesLcConDto queModel);
        public Task<Response> DeleteAsync(long quesId);
    }
}
