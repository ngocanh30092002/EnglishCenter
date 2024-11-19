using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class AnswerLcAudioRepository : GenericRepository<AnswerLcAudio>, IAnswerLcAudioRepository
    {
        public AnswerLcAudioRepository(EnglishCenterContext context) : base(context)
        {
        }

        public Task<bool> ChangeAnswerAAsync(AnswerLcAudio model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerA = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerBAsync(AnswerLcAudio model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerB = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerCAsync(AnswerLcAudio model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerC = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeCorrectAnswerAsync(AnswerLcAudio model, string newCorrectAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.CorrectAnswer = newCorrectAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeQuestionAsync(AnswerLcAudio model, string newQues)
        {
            if (model == null) return Task.FromResult(false);

            model.Question = newQues;

            return Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(long answerId, AnswerLcAudioDto model)
        {
            var answerModel = await context.AnswerLcAudios.FindAsync(answerId);

            if (answerModel == null)
            {
                return false;
            }

            if(answerModel.Question != model.Question)
            {
                var isSuccess = await ChangeQuestionAsync(answerModel, model.Question);
                if (!isSuccess) return false;
            }

            if (answerModel.AnswerA != model.AnswerA)
            {
                var isSuccess = await ChangeAnswerAAsync(answerModel, model.AnswerA);
                if (!isSuccess) return false;
            }

            if (answerModel.AnswerB != model.AnswerB)
            {
                var isSuccess = await ChangeAnswerBAsync(answerModel, model.AnswerB);
                if (!isSuccess) return false;
            }

            if (answerModel.AnswerC != model.AnswerC)
            {
                var isSuccess = await ChangeAnswerCAsync(answerModel, model.AnswerC);
                if (!isSuccess) return false;
            }

            if (answerModel.CorrectAnswer != model.CorrectAnswer)
            {
                var isSuccess = await ChangeCorrectAnswerAsync(answerModel, model.CorrectAnswer);
                if (!isSuccess) return false;
            }

            return true;
        }
    }
}
