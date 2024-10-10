using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ISubRcSingleRepository : IGenericRepository<SubRcSingle>
    {
        public Task<bool> ChangePreQuesAsync(SubRcSingle model, long preId);
        public Task<bool> ChangeQuestionAsync(SubRcSingle model, string newQues);
        public Task<bool> ChangeAnswerAsync(SubRcSingle model, long answerId);
        public Task<bool> ChangeAnswerAAsync(SubRcSingle model, string newAnswer);
        public Task<bool> ChangeAnswerBAsync(SubRcSingle model, string newAnswer);
        public Task<bool> ChangeAnswerCAsync(SubRcSingle model, string newAnswer);
        public Task<bool> ChangeAnswerDAsync(SubRcSingle model, string newAnswer);
        public Task<bool> ChangeNoNumAsync(SubRcSingle model, int noNum);
    }
}
