using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IRoadMapExamService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetByRoadMapAsync(long roadMapId);
        public Task<Response> CreateAsync(RoadMapExamDto model);
        public Task<Response> UpdateAsync(long id, RoadMapExamDto model);
        public Task<Response> DeleteAsync(long id);
        public Task<Response> ChangeNameAsync(long id, string newName);
        public Task<Response> ChangeTimeAsync(long id, double timeMinute);
        public Task<Response> ChangeRoadMapAsync(long id, long roadMapId);
    }
}
