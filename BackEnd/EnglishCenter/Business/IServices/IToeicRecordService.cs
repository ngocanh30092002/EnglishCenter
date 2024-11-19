using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IToeicRecordService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetResultAsync(long processId);
        public Task<Response> ChangeProcessAsync(long id, long processId);
        public Task<Response> ChangeSelectedAnswerAsync(long id, string? selectedAnswer);
        public Task<Response> CreateAsync(ToeicRecordDto model);
        public Task<Response> UpdateAsync(long id, ToeicRecordDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
