using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IQuesLcImageRepository : IGenericRepository<QuesLcImage>
    {
        Task<bool> ChangeTimeAsync(QuesLcImage queModel, TimeOnly time);
        Task<bool> ChangeImageAsync(QuesLcImage queModel, string imagePath);
        Task<bool> ChangeAudioAsync(QuesLcImage queModel, string audioPath);
        Task<bool> ChangeAnswerAsync(QuesLcImage queModel, long answerId);
        Task<bool> ChangeLevelAsync(QuesLcImage model, int level);

    }
}
