using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IRoadMapService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetCourseIdsAsync();
        public Task<Response> GetByCourseAsync(string courseId);
        public Task<Response> CreateAsync(RoadMapDto model);
        public Task<Response> UpdateAsync(long id, RoadMapDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
