using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAnswerRcTripleRepository : IGenericRepository<AnswerRcTriple>
    {
        Task<bool> ChangeAnswerAAsync(AnswerRcTriple model, string newAnswer);
        Task<bool> ChangeAnswerBAsync(AnswerRcTriple model, string newAnswer);
        Task<bool> ChangeAnswerCAsync(AnswerRcTriple model, string newAnswer);
        Task<bool> ChangeAnswerDAsync(AnswerRcTriple model, string newAnswer);
        Task<bool> ChangeCorrectAnswerAsync(AnswerRcTriple model, string newCorrectAnswer);
        Task<bool> ChangeQuestionAsync(AnswerRcTriple model, string newQuestion);
        Task<bool> UpdateAsync(long answerId, AnswerRcTripleDto model);
    }
}
