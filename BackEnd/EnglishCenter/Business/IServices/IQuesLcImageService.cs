using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IQuesLcImageService
    {
        public Task<Response> GetAsync(long quesId);
        public Task<Response> GetOtherQuestionByAssignmentAsync(long assignmentId);
        public Task<Response> GetOtherQuestionByHomeworkAsync(long homeworkId);
        public Task<Response> GetAllAsync();
        public Task<Response> ChangeAnswerAsync(long quesId, long answerId);
        public Task<Response> ChangeAudioAsync(long quesId, IFormFile audioFile);
        public Task<Response> ChangeImageAsync(long quesId, IFormFile imageFile);
        public Task<Response> ChangeLevelAsync(long quesId, int level);
        public Task<Response> CreateAsync(QuesLcImageDto queModel);
        public Task<Response> UpdateAsync(long quesId, QuesLcImageDto queModel);
        public Task<Response> DeleteAsync(long quesId);
    }
}
