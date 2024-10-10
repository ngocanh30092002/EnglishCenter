using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ISubLcConRepository : IGenericRepository<SubLcConversation>
    {
        public Task<bool> ChangePreQuesAsync(SubLcConversation model, long preId);
        public Task<bool> ChangeQuestionAsync(SubLcConversation model, string newQues);
        public Task<bool> ChangeAnswerAsync(SubLcConversation model, long answerId);
        public Task<bool> ChangeAnswerAAsync(SubLcConversation model, string newAnswer);
        public Task<bool> ChangeAnswerBAsync(SubLcConversation model, string newAnswer);
        public Task<bool> ChangeAnswerCAsync(SubLcConversation model, string newAnswer);
        public Task<bool> ChangeAnswerDAsync(SubLcConversation model, string newAnswer);
        public Task<bool> ChangeNoNumAsync(SubLcConversation model, int noNum);
    }
}
