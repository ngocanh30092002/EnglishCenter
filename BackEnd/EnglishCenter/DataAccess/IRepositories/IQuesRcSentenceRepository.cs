using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IQuesRcSentenceRepository : IGenericRepository<QuesRcSentence>
    {
        public Task<bool> ChangeTimeAsync(QuesRcSentence model, TimeOnly time);
        public Task<bool> ChangeQuestionAsync(QuesRcSentence model, string newQues);
        public Task<bool> ChangeAnswerAAsync(QuesRcSentence model, string newAnswer);
        public Task<bool> ChangeAnswerBAsync(QuesRcSentence model, string newAnswer);
        public Task<bool> ChangeAnswerCAsync(QuesRcSentence model, string newAnswer);
        public Task<bool> ChangeAnswerDAsync(QuesRcSentence model, string newAnswer);
        public Task<bool> ChangeAnswerAsync(QuesRcSentence model, long answerId);
        public Task<bool> ChangeLevelAsync(QuesRcSentence model, int level);

    }
}
