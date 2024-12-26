using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IHomeQuesRepository : IGenericRepository<HomeQue>
    {
        public Task<bool> LoadQuestionAsync(HomeQue model);
        public Task<bool> LoadQuestionWithAnswerAsync(HomeQue model);
        public Task<bool> IsExistQuesIdAsync(QuesTypeEnum type, long quesId);
        public Task<bool> IsCorrectAnswerAsync(HomeQue model, string selectedAnswer, long? subId);
        public Task<bool> IsSameHomeQuesAsync(QuesTypeEnum type, long homeworkId, long quesId);
        public Task<bool> ChangeQuesAsync(HomeQue model, QuesTypeEnum type, long quesId);
        public Task<bool> ChangeHomeworkIdAsync(HomeQue model, long homeworkId);
        public Task<bool> ChangeNoNumAsync(HomeQue model, int noNum);
        public Task<int> GetNumberByHomeworkAsync(long homeworkId);
        public Task<long> GetQuesIdAsync(HomeQue model);
        public Task<TimeOnly> GetTimeQuesAsync(HomeQue model);
        public Task<object> GetAnswerInfoAsync(long homeQuesId, long? subId);
        public Task<List<HomeQue>?> GetByHomeworkAsync(long homeworkId);
        public Task<bool> UpdateAsync(long id, HomeQueDto model);
    }
}
