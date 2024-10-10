using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAnswerLcConRepository : IGenericRepository<AnswerLcConversation>
    {
        Task<bool> ChangeAnswerAAsync(AnswerLcConversation model, string newAnswer);
        Task<bool> ChangeAnswerBAsync(AnswerLcConversation model, string newAnswer);
        Task<bool> ChangeAnswerCAsync(AnswerLcConversation model, string newAnswer);
        Task<bool> ChangeAnswerDAsync(AnswerLcConversation model, string newAnswer);
        Task<bool> ChangeCorrectAnswerAsync(AnswerLcConversation model, string newCorrectAnswer);
        Task<bool> ChangeQuestionAsync(AnswerLcConversation model, string newQuestion);
        Task<bool> UpdateAsync(long answerId, AnswerLcConDto model);
    }
}
