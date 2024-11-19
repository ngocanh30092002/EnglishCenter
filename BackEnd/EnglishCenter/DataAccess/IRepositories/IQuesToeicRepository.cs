using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IQuesToeicRepository : IGenericRepository<QuesToeic>
    {
        public Task<bool> LoadSubQuesWithAnswer(QuesToeic queModel);
        public Task<bool> LoadSubQuesAsync(QuesToeic queModel);
        public Task<bool> ChangeNoNumAsync(QuesToeic queModel, int noNum);
        public Task<bool> ChangeAudioAsync(QuesToeic queModel, string audioPath);
        public Task<bool> ChangeImage1Async(QuesToeic queModel, string imagePath);
        public Task<bool> ChangeImage2Async(QuesToeic queModel, string imagePath);
        public Task<bool> ChangeImage3Async(QuesToeic queModel, string imagePath);
        public Task<bool> ChangeGroupAsync(QuesToeic queModel, bool isGroup);
    }
}
