using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class QuesLcImageRepository : GenericRepository<QuesLcImage>, IQuesLcImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuesLcImageRepository(EnglishCenterContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> ChangeAnswerAsync(QuesLcImage queModel, long answerId)
        {
            if (queModel == null) return false;

            var isExist = await context.AnswerLcImages.AnyAsync(a => a.AnswerId == answerId);
            if(isExist == false) return false;

            queModel.AnswerId = answerId;
            return true;
        }

        public Task<bool> ChangeAudioAsync(QuesLcImage queModel, string audioPath)
        {
            if (queModel == null) return Task.FromResult(false);

            var path = Path.Combine(_webHostEnvironment.WebRootPath, audioPath ?? "");
            if (!File.Exists(path)) return Task.FromResult(false);

            queModel.Audio = audioPath ?? "";

            return Task.FromResult(true);
        }

        public Task<bool> ChangeImageAsync(QuesLcImage queModel, string imagePath)
        {
            if (queModel == null) return Task.FromResult(false);

            var path = Path.Combine(_webHostEnvironment.WebRootPath, imagePath ?? "");
            if (!File.Exists(path)) return Task.FromResult(false);

            queModel.Image = imagePath ?? "";

            return Task.FromResult(true);
        }
    }
}
