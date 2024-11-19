using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAnswerLcImageRepository : IGenericRepository<AnswerLcImage>
    {
        Task<bool> ChangeAnswerAAsync(AnswerLcImage model, string newAnswer);
        Task<bool> ChangeAnswerBAsync(AnswerLcImage model, string newAnswer);
        Task<bool> ChangeAnswerCAsync(AnswerLcImage model, string newAnswer);
        Task<bool> ChangeAnswerDAsync(AnswerLcImage model, string newAnswer);
        Task<bool> ChangeCorrectAnswerAsync(AnswerLcImage model, string newCorrectAnswer);
        Task<bool> UpdateAsync(long answerId, AnswerLcImageDto model);
    }
}
