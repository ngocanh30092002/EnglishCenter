using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IToeicExamService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetToeicDirectionAsync(long id);
        public Task<Response> ChangeNameAsync(long id, string newName);
        public Task<Response> ChangeCodeAsync(long id, int code);
        public Task<Response> ChangeYearAsync(long id, int year);
        public Task<Response> ChangeCompleteNumAsync(long id, int num);
        public Task<Response> ChangePointAsync(long id, int point);
        public Task<Response> ChangeMinutesAsync(long id, int minutes);
        public Task<Response> CreateAsync(ToeicExamDto model);
        public Task<Response> UpdateAsync(long id, ToeicExamDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
