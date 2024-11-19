using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAnswerRcDoubleRepository : IGenericRepository<AnswerRcDouble>
    {
        Task<bool> ChangeAnswerAAsync(AnswerRcDouble model, string newAnswer);
        Task<bool> ChangeAnswerBAsync(AnswerRcDouble model, string newAnswer);
        Task<bool> ChangeAnswerCAsync(AnswerRcDouble model, string newAnswer);
        Task<bool> ChangeAnswerDAsync(AnswerRcDouble model, string newAnswer);
        Task<bool> ChangeCorrectAnswerAsync(AnswerRcDouble model, string newCorrectAnswer);
        Task<bool> ChangeQuestionAsync(AnswerRcDouble model, string newQuestion);
        Task<bool> UpdateAsync(long answerId, AnswerRcDoubleDto model);
    }
}
