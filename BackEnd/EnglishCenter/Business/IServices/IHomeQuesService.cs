using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IHomeQuesService
    {
        public Task<Response> GetAsync(long id);
        public Task<Response> GetAllAsync();
        public Task<Response> GetByHomeworkAsync(long homeworkId);
        public Task<Response> GetByHwSubmissionAsync(long hwSubId);
        public Task<Response> GetNumberByHomeworkAsync(long homeworkId);
        public Task<Response> GetHomeQuesByNoNumAsync(long homeworkId, int noNum);
        public Task<Response> GetNumQuesWithTypeAsync();
        public Task<Response> GetTypeQuesAsync();
        public Task<Response> GetPartAsync();
        public Task<Response> ChangeHomeworkIdAsync(long id, long homeworkId);
        public Task<Response> ChangeNoNumAsync(long id, int noNum);
        public Task<Response> ChangeQuesAsync(long id, int type, long quesId);
        public Task<Response> CreateAsync(HomeQueDto model);
        public Task<Response> HandleCreateWithHwAsync(long homeworkId, List<TypeQuestionDto> listModel);
        public Task<Response> UpdateAsync(long id, HomeQueDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
