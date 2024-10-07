using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories
{
    public class QuesLcAudioRepository : GenericRepository<QuesLcAudio>, IQuesLcAudioRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuesLcAudioRepository(EnglishCenterContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> ChangeAnswerAsync(QuesLcAudio queModel, long answerId)
        {
            if (queModel == null) return false;

            var isExist = await context.AnswerLcAudios.AnyAsync(a => a.AnswerId == answerId);
            if (isExist == false) return false;

            queModel.AnswerId = answerId;
            return true;
        }

        public Task<bool> ChangeAudioAsync(QuesLcAudio queModel, string audioPath)
        {
            if (queModel == null) return Task.FromResult(false);

            var path = Path.Combine(_webHostEnvironment.WebRootPath, audioPath ?? "");
            if (!File.Exists(path)) return Task.FromResult(false);

            queModel.Audio = audioPath ?? "";

            return Task.FromResult(true);
        }
    }
}
