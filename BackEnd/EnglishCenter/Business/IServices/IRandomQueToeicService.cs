using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IRandomQueToeicService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetByHomeworkAsync(long homeworkId);
        public Task<Response> GetByDefaultHwAsync(long homeworkId);
        public Task<Response> GetByRoadMapExamAsync(long examId);
        public Task<Response> GetByDefaultRmAsync(long examId);
        public Task<Response> GetNumberQuesByHwAsync(long homeworkId);
        public Task<Response> GetNumberByHwAsync(long homeworkId);
        public Task<Response> GetNumberQuesByRmAsync(long roadMapExamId);
        public Task<Response> GetNumberByRmAsync(long roadMapExamId);
        public Task<Response> HandleCreateHwWithLevelAsync(RandomPartDto model);
        public Task<Response> HandleCreateHwWithLevelAsync(long homeworkId, List<RandomPartWithLevelDto> models);
        public Task<Response> HandleCreateRmWithLevelAsync(RandomPartDto model);
        public Task<Response> HandleCreateRmWithLevelAsync(long examId, List<RandomPartWithLevelDto> models);
        public Task<Response> CreateAsync(RandomQuesToeicDto model, bool isAddTime = true);
        public Task<Response> UpdateAsync(long id, RandomQuesToeicDto model);
        public Task<Response> DeleteAsync(long id);
        public Task<Response> DeleteHwAsync(long homeworkId, long quesId);
        public Task<Response> DeleteRmAsync(long roadMapId, long quesId);
        public Task<Response> ChangeRoadMapAsync(long id, long examId);
        public Task<Response> ChangeHomeworkAsync(long id, long homeworkId);
        public Task<Response> ChangeQuesToeicAsync(long id, long quesId);
        public Task<Response> HandleCreateRmByListAsync(List<RandomQuesToeicDto> models);
        public Task<Response> HandleCreateHwByListAsync(List<RandomQuesToeicDto> models);
    }
}
