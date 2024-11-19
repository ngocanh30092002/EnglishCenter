using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAnswerLcAudioRepository : IGenericRepository<AnswerLcAudio>
    {
        Task<bool> ChangeQuestionAsync(AnswerLcAudio model, string newQues);
        Task<bool> ChangeAnswerAAsync(AnswerLcAudio model, string newAnswer);
        Task<bool> ChangeAnswerBAsync(AnswerLcAudio model, string newAnswer);
        Task<bool> ChangeAnswerCAsync(AnswerLcAudio model, string newAnswer);
        Task<bool> ChangeCorrectAnswerAsync(AnswerLcAudio model, string newCorrectAnswer);
        Task<bool> UpdateAsync(long answerId, AnswerLcAudioDto model);
    }
}
