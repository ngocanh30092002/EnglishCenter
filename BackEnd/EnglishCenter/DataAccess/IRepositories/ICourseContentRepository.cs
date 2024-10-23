using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ICourseContentRepository : IGenericRepository<CourseContent>
    {
        public Task<Response> UpdateAsync(long contentId, CourseContentDto model);
        public Task<bool> ChangeNoNumAsync(CourseContent contentModel, int number);
        public Task<bool> ChangeContentAsync(CourseContent contentModel, string content);
        public Task<bool> ChangeTypeAsync(CourseContent contentModel, CourseContentTypeEnum type);
        public Task<List<CourseContent>?> GetByCourseAsync(string courseId);
    }
}
