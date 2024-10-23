using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAnswerRcSenMediaRepository : IGenericRepository<AnswerRcSentenceMedia>
    {
        Task<bool> ChangeAnswerAAsync(AnswerRcSentenceMedia model, string newAnswer);
        Task<bool> ChangeAnswerBAsync(AnswerRcSentenceMedia model, string newAnswer);
        Task<bool> ChangeAnswerCAsync(AnswerRcSentenceMedia model, string newAnswer);
        Task<bool> ChangeAnswerDAsync(AnswerRcSentenceMedia model, string newAnswer);
        Task<bool> ChangeExplanationAsync(AnswerRcSentenceMedia model, string newExplanation);
        Task<bool> ChangeQuestionAsync(AnswerRcSentenceMedia model, string newQuestion);
        Task<bool> ChangeCorrectAnswerAsync(AnswerRcSentenceMedia model, string newCorrectAnswer);
        Task<bool> UpdateAsync(long answerId, AnswerRcSenMediaDto model);
    }
}
