using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IQuesLcAudioRepository : IGenericRepository<QuesLcAudio>
    {
        Task<bool> ChangeAudioAsync(QuesLcAudio queModel, string audioPath);
        Task<bool> ChangeAnswerAsync(QuesLcAudio queModel, long answerId);
    }
}
