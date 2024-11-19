using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAnswerToeicRepository : IGenericRepository<AnswerToeic>
    {
        Task<bool> ChangeAnswerAAsync(AnswerToeic model, string? newAnswer);
        Task<bool> ChangeAnswerBAsync(AnswerToeic model, string? newAnswer);
        Task<bool> ChangeAnswerCAsync(AnswerToeic model, string? newAnswer);
        Task<bool> ChangeAnswerDAsync(AnswerToeic model, string? newAnswer);
        Task<bool> ChangeExplanationAsync(AnswerToeic model, string? newExplanation);
        Task<bool> ChangeQuestionAsync(AnswerToeic model, string? newQuestion);
        Task<bool> ChangeCorrectAnswerAsync(AnswerToeic model, string newCorrectAnswer);
        Task<bool> UpdateAsync(long answerId, AnswerToeicDto model);
    }
}
