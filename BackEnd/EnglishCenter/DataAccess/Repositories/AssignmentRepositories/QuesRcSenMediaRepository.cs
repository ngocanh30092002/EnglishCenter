using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class QuesRcSenMediaRepository : GenericRepository<QuesRcSentenceMedia>, IQuesRcSenMediaRepository
    {
        public QuesRcSenMediaRepository(EnglishCenterContext context) : base(context)
        {
        }

        public Task<bool> ChangeTimeAsync(QuesRcSentenceMedia model, TimeOnly time)
        {
            if (model == null) return Task.FromResult(false);

            model.Time = time;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerAAsync(QuesRcSentenceMedia model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerA = newAnswer;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeAnswerAsync(QuesRcSentenceMedia model, long answerId)
        {
            if (model == null) return false;

            var answerModel = await context.AnswerRcSentences
                                    .Include(a => a.QuesRcSentence)
                                    .FirstOrDefaultAsync(a => a.AnswerId == answerId);

            if (answerModel == null) return false;
            if (answerModel.QuesRcSentence != null) return false;

            model.AnswerId = answerId;
            return true;
        }

        public Task<bool> ChangeAnswerBAsync(QuesRcSentenceMedia model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerB = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerCAsync(QuesRcSentenceMedia model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerC = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerDAsync(QuesRcSentenceMedia model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerD = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeQuestionAsync(QuesRcSentenceMedia model, string newQues)
        {
            if (model == null) return Task.FromResult(false);

            model.Question = newQues;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeImageAsync(QuesRcSentenceMedia model, string imageUrl)
        {
            if (model == null) return Task.FromResult(false);

            model.Image = imageUrl ?? "";

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAudioAsync(QuesRcSentenceMedia model, string? audioUrl)
        {
            if (model == null) return Task.FromResult(false);

            model.Audio = audioUrl;

            return Task.FromResult(true);
        }
    }
}
