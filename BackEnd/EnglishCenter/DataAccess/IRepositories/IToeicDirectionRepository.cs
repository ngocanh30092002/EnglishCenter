using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IToeicDirectionRepository : IGenericRepository<ToeicDirection>
    {
        public Task<bool> ChangeIntroduceListeningAsync(ToeicDirection toeicDirection, string introduce);
        public Task<bool> ChangeIntroduceReadingAsync(ToeicDirection toeicDirection, string introduce);
        public Task<bool> ChangeImageAsync(ToeicDirection toeicDirection, string imageUrl);
        public Task<bool> ChangePart1Async(ToeicDirection toeicDirection, string value);
        public Task<bool> ChangeAudio1Async(ToeicDirection toeicDirection, string audioUrl);
        public Task<bool> ChangePart2Async(ToeicDirection toeicDirection, string value);
        public Task<bool> ChangeAudio2Async(ToeicDirection toeicDirection, string audioUrl);
        public Task<bool> ChangePart3Async(ToeicDirection toeicDirection, string value);
        public Task<bool> ChangeAudio3Async(ToeicDirection toeicDirection, string audioUrl);
        public Task<bool> ChangePart4Async(ToeicDirection toeicDirection, string value);
        public Task<bool> ChangeAudio4Async(ToeicDirection toeicDirection, string audioUrl);
        public Task<bool> ChangePart5Async(ToeicDirection toeicDirection, string value);
        public Task<bool> ChangePart6Async(ToeicDirection toeicDirection, string value);
        public Task<bool> ChangePart7Async(ToeicDirection toeicDirection, string value);
    }
}
