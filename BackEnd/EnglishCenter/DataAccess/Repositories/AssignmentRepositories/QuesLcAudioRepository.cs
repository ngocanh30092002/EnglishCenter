using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class QuesLcAudioRepository : GenericRepository<QuesLcAudio>, IQuesLcAudioRepository
    {

        public QuesLcAudioRepository(EnglishCenterContext context) : base(context)
        {
        }

        public Task<bool> ChangeAnswerAAsync(QuesLcAudio queModel, string newAnswer)
        {
            if (queModel == null) return Task.FromResult(false);

            queModel.AnswerA = newAnswer;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeAnswerAsync(QuesLcAudio queModel, long answerId)
        {
            if (queModel == null) return false;

            var answerModel = await context.AnswerLcAudios
                                    .Include(a => a.QuesLcAudio)
                                    .FirstOrDefaultAsync(a => a.AnswerId == answerId);
            if (answerModel == null) return false;
            if (answerModel.QuesLcAudio != null) return false;

            return true;
        }

        public Task<bool> ChangeAnswerBAsync(QuesLcAudio queModel, string newAnswer)
        {
            if (queModel == null) return Task.FromResult(false);

            queModel.AnswerB = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerCAsync(QuesLcAudio queModel, string newAnswer)
        {
            if (queModel == null) return Task.FromResult(false);

            queModel.AnswerC = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAudioAsync(QuesLcAudio queModel, string audioPath)
        {
            if (queModel == null) return Task.FromResult(false);

            queModel.Audio = audioPath ?? "";

            return Task.FromResult(true);
        }

        public Task<bool> ChangeLevelAsync(QuesLcAudio queModel, int level)
        {
            if (queModel == null) return Task.FromResult(false);

            if (level <= 0 || level > 4) return Task.FromResult(false);

            queModel.Level = level;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeQuestionAsync(QuesLcAudio queModel, string newQues)
        {
            if (queModel == null) return Task.FromResult(false);

            queModel.Question = newQues;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTimeAsync(QuesLcAudio queModel, TimeOnly time)
        {
            if (queModel == null) return Task.FromResult(false);

            queModel.Time = time;

            return Task.FromResult(true);
        }
    }
}
