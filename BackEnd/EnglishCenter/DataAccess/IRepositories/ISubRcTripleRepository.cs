using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ISubRcTripleRepository : IGenericRepository<SubRcTriple>
    {
        public Task<bool> ChangePreQuesAsync(SubRcTriple model, long preId);
        public Task<bool> ChangeQuestionAsync(SubRcTriple model, string newQues);
        public Task<bool> ChangeAnswerAsync(SubRcTriple model, long answerId);
        public Task<bool> ChangeAnswerAAsync(SubRcTriple model, string newAnswer);
        public Task<bool> ChangeAnswerBAsync(SubRcTriple model, string newAnswer);
        public Task<bool> ChangeAnswerCAsync(SubRcTriple model, string newAnswer);
        public Task<bool> ChangeAnswerDAsync(SubRcTriple model, string newAnswer);
        public Task<bool> ChangeNoNumAsync(SubRcTriple model, int noNum);
    }
}
