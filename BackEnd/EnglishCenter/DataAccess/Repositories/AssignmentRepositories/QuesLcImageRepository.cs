using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class QuesLcImageRepository : GenericRepository<QuesLcImage>, IQuesLcImageRepository
    {

        public QuesLcImageRepository(EnglishCenterContext context) : base(context)
        {
        }

        public async Task<bool> ChangeAnswerAsync(QuesLcImage queModel, long answerId)
        {
            if (queModel == null) return false;

            var answerModel = await context.AnswerLcImages
                                    .Include(a => a.QuesLcImage)
                                    .FirstOrDefaultAsync(a => a.AnswerId == answerId);
            if (answerModel == null) return false;
            if (answerModel.QuesLcImage != null) return false;

            queModel.AnswerId = answerId;
            return true;
        }

        public Task<bool> ChangeAudioAsync(QuesLcImage queModel, string audioPath)
        {
            if (queModel == null) return Task.FromResult(false);

            queModel.Audio = audioPath ?? "";

            return Task.FromResult(true);
        }

        public Task<bool> ChangeImageAsync(QuesLcImage queModel, string imagePath)
        {
            if (queModel == null) return Task.FromResult(false);

            queModel.Image = imagePath ?? "";

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTimeAsync(QuesLcImage queModel, TimeOnly time)
        {
            if (queModel == null) return Task.FromResult(false);

            queModel.Time = time;

            return Task.FromResult(true);
        }
    }
}
