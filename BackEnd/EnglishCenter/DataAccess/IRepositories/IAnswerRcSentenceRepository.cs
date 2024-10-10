using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAnswerRcSentenceRepository : IGenericRepository<AnswerRcSentence>
    {
        Task<bool> ChangeAnswerAAsync(AnswerRcSentence model, string newAnswer);
        Task<bool> ChangeAnswerBAsync(AnswerRcSentence model, string newAnswer);
        Task<bool> ChangeAnswerCAsync(AnswerRcSentence model, string newAnswer);
        Task<bool> ChangeAnswerDAsync(AnswerRcSentence model, string newAnswer);
        Task<bool> ChangeExplanationAsync(AnswerRcSentence model, string newExplanation);
        Task<bool> ChangeQuestionAsync(AnswerRcSentence model, string newQuestion);
        Task<bool> ChangeCorrectAnswerAsync(AnswerRcSentence model, string newCorrectAnswer);
        Task<bool> UpdateAsync(long answerId, AnswerRcSentenceDto model);
    }
}
