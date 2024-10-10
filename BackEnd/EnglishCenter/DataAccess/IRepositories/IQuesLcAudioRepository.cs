using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IQuesLcAudioRepository : IGenericRepository<QuesLcAudio>
    {
        Task<bool> ChangeTimeAsync(QuesLcAudio queModel, TimeOnly time);
        Task<bool> ChangeAudioAsync(QuesLcAudio queModel, string audioPath);
        Task<bool> ChangeAnswerAsync(QuesLcAudio queModel, long answerId);
        Task<bool> ChangeQuestionAsync(QuesLcAudio queModel, string newQues);
        Task<bool> ChangeAnswerAAsync(QuesLcAudio queModel, string newAnswer);
        Task<bool> ChangeAnswerBAsync(QuesLcAudio queModel, string newAnswer);
        Task<bool> ChangeAnswerCAsync(QuesLcAudio queModel, string newAnswer);
    }
}
