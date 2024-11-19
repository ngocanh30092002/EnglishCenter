using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class ToeicDirectionRepository : GenericRepository<ToeicDirection>, IToeicDirectionRepository
    {
        public ToeicDirectionRepository(EnglishCenterContext context) : base(context)
        {
        }

        public Task<bool> ChangeAudio1Async(ToeicDirection toeicDirection, string audioUrl)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Audio1 = audioUrl;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAudio2Async(ToeicDirection toeicDirection, string audioUrl)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Audio2 = audioUrl;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAudio3Async(ToeicDirection toeicDirection, string audioUrl)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Audio3 = audioUrl;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAudio4Async(ToeicDirection toeicDirection, string audioUrl)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Audio4 = audioUrl;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeImageAsync(ToeicDirection toeicDirection, string imageUrl)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Image = imageUrl;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeIntroduceListeningAsync(ToeicDirection toeicDirection, string introduce)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Introduce_Listening = introduce;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeIntroduceReadingAsync(ToeicDirection toeicDirection, string introduce)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Introduce_Reading = introduce;

            return Task.FromResult(true);
        }

        public Task<bool> ChangePart1Async(ToeicDirection toeicDirection, string value)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Part1 = value;

            return Task.FromResult(true);
        }

        public Task<bool> ChangePart2Async(ToeicDirection toeicDirection, string value)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Part2 = value;

            return Task.FromResult(true);
        }

        public Task<bool> ChangePart3Async(ToeicDirection toeicDirection, string value)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Part3 = value;

            return Task.FromResult(true);
        }

        public Task<bool> ChangePart4Async(ToeicDirection toeicDirection, string value)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Part4 = value;

            return Task.FromResult(true);
        }

        public Task<bool> ChangePart5Async(ToeicDirection toeicDirection, string value)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Part5 = value;

            return Task.FromResult(true);
        }

        public Task<bool> ChangePart6Async(ToeicDirection toeicDirection, string value)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Part6 = value;

            return Task.FromResult(true);
        }

        public Task<bool> ChangePart7Async(ToeicDirection toeicDirection, string value)
        {
            if (toeicDirection == null) return Task.FromResult(false);

            toeicDirection.Part7 = value;

            return Task.FromResult(true);
        }
    }
}
