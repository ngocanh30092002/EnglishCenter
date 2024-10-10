using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IQuesRcSentenceRepository : IGenericRepository<QuesRcSentence>
    {
        public Task<bool> ChangeQuestionAsync(QuesRcSentence model, string newQues);
        public Task<bool> ChangeAnswerAAsync(QuesRcSentence model, string newAnswer);
        public Task<bool> ChangeAnswerBAsync(QuesRcSentence model, string newAnswer);
        public Task<bool> ChangeAnswerCAsync(QuesRcSentence model, string newAnswer);
        public Task<bool> ChangeAnswerDAsync(QuesRcSentence model, string newAnswer);
        public Task<bool> ChangeAnswerAsync(QuesRcSentence model, long answerId);
    }
}
