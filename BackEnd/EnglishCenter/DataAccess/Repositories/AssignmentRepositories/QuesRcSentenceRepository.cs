using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class QuesRcSentenceRepository : GenericRepository<QuesRcSentence>, IQuesRcSentenceRepository
    {
        public QuesRcSentenceRepository(EnglishCenterContext context) : base(context)
        {

        }

        public Task<bool> ChangeTimeAsync(QuesRcSentence model, TimeOnly time)
        {
            if (model == null) return Task.FromResult(false);

            model.Time = time;

            return Task.FromResult(true);
        }
        public Task<bool> ChangeAnswerAAsync(QuesRcSentence model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerA = newAnswer;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeAnswerAsync(QuesRcSentence model, long answerId)
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

        public Task<bool> ChangeAnswerBAsync(QuesRcSentence model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerB = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerCAsync(QuesRcSentence model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerC = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerDAsync(QuesRcSentence model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerD = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeQuestionAsync(QuesRcSentence model, string newQues)
        {
            if (model == null) return Task.FromResult(false);

            model.Question = newQues;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeLevelAsync(QuesRcSentence model, int level)
        {
            if (model == null) return Task.FromResult(false);

            if (level <= 0 || level > 4) return Task.FromResult(false);

            model.Level = level;

            return Task.FromResult(true);
        }
    }
}
