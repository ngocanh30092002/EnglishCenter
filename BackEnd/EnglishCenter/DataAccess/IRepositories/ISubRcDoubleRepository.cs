using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ISubRcDoubleRepository : IGenericRepository<SubRcDouble>
    {
        public Task<bool> ChangePreQuesAsync(SubRcDouble model, long preId);
        public Task<bool> ChangeQuestionAsync(SubRcDouble model, string newQues);
        public Task<bool> ChangeAnswerAsync(SubRcDouble model, long answerId);
        public Task<bool> ChangeAnswerAAsync(SubRcDouble model, string newAnswer);
        public Task<bool> ChangeAnswerBAsync(SubRcDouble model, string newAnswer);
        public Task<bool> ChangeAnswerCAsync(SubRcDouble model, string newAnswer);
        public Task<bool> ChangeAnswerDAsync(SubRcDouble model, string newAnswer);
        public Task<bool> ChangeNoNumAsync(SubRcDouble model, int noNum);
    }
}
