using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IQuesRcSenMediaRepository : IGenericRepository<QuesRcSentenceMedia>
    {
        public Task<bool> ChangeTimeAsync(QuesRcSentenceMedia model, TimeOnly time);
        public Task<bool> ChangeQuestionAsync(QuesRcSentenceMedia model, string newQues);
        public Task<bool> ChangeAnswerAAsync(QuesRcSentenceMedia model, string newAnswer);
        public Task<bool> ChangeAnswerBAsync(QuesRcSentenceMedia model, string newAnswer);
        public Task<bool> ChangeAnswerCAsync(QuesRcSentenceMedia model, string newAnswer);
        public Task<bool> ChangeAnswerDAsync(QuesRcSentenceMedia model, string newAnswer);
        public Task<bool> ChangeAnswerAsync(QuesRcSentenceMedia model, long answerId);
        public Task<bool> ChangeImageAsync(QuesRcSentenceMedia model, string imageUrl);
        public Task<bool> ChangeAudioAsync(QuesRcSentenceMedia model, string? audioUrl);
    }
}
