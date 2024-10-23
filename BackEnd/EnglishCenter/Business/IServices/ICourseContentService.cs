using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface ICourseContentService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long contentId);
        public Task<Response> GetByCourseAsync(string courseId);
        public Task<Response> CreateAsync(CourseContentDto courseContentDto);
        public Task<Response> UpdateAsync(long contentId, CourseContentDto courseContentDto);
        public Task<Response> ChangeNoNumAsync(long contentId, int number);
        public Task<Response> ChangeContentAsync(long contentId, string content);
        public Task<Response> ChangeTypeAsync(long contentId, int type);
        public Task<Response> DeleteAsync(long contentId);
    }
}
