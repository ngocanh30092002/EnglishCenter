using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ISubToeicRepository : IGenericRepository<SubToeic>
    {
        public Task<bool> ChangeQuesNoAsync(SubToeic subModel, int queNo);
        public Task<bool> ChangeQuestionAsync(SubToeic subModel, string question);
        public Task<bool> ChangeAnswerAAsync(SubToeic subModel, string newAnswer);
        public Task<bool> ChangeAnswerBAsync(SubToeic subModel, string newAnswer);
        public Task<bool> ChangeAnswerCAsync(SubToeic subModel, string newAnswer);
        public Task<bool> ChangeAnswerDAsync(SubToeic subModel, string newAnswer);
        public Task<bool> ChangeAnswerAsync(SubToeic subModel, long answerId);
    }
}
