using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAnswerRcSingleRepository : IGenericRepository<AnswerRcSingle>
    {
        Task<bool> ChangeAnswerAAsync(AnswerRcSingle model, string newAnswer);
        Task<bool> ChangeAnswerBAsync(AnswerRcSingle model, string newAnswer);
        Task<bool> ChangeAnswerCAsync(AnswerRcSingle model, string newAnswer);
        Task<bool> ChangeAnswerDAsync(AnswerRcSingle model, string newAnswer);
        Task<bool> ChangeCorrectAnswerAsync(AnswerRcSingle model, string newCorrectAnswer);
        Task<bool> ChangeQuestionAsync(AnswerRcSingle model, string newQuestion);
        Task<bool> UpdateAsync(long answerId, AnswerRcSingleDto model);
    }
}
