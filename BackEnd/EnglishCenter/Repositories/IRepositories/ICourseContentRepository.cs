using EnglishCenter.Models;
using EnglishCenter.Models.DTO;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface ICourseContentRepository
    {
        public Task<Response> CreateContentAsync(CourseContentDto model);
        public Task<Response> UpdateContentAsync(long contentId, CourseContentDto model);
        public Task<Response> RemoveContentAsync(long contentId);
        public Task<Response> ChangeNoNumAsync(long contentId, int number);
        public Task<Response> ChangeContentAsync(long contentId, string content);
        public Task<Response> GetContentsAsync();
        public Task<Response> GetContentsAsync(string courseId);
        public Task<Response> GetContentAsync(long contentId);
    }
}
