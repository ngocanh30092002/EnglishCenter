using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IExaminationService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetByCouseContentAsync(long courseContentId);
        public Task<Response> ChangeCourseContentAsync(long id, long contentId);
        public Task<Response> ChangeToeicAsync(long id, long toeicId);
        public Task<Response> ChangeTitleAsync(long id, string title);
        public Task<Response> ChangeTimeAsync(long id, string timeStr);
        public Task<Response> ChangeDescriptionAsync(long id, string description);
        public Task<Response> CreateAsync(ExaminationDto model);
        public Task<Response> UpdateAsync(long id, ExaminationDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
