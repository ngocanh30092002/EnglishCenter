using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class AnswerRcSentenceRepository : GenericRepository<AnswerRcSentence>, IAnswerRcSentenceRepository
    {
        public AnswerRcSentenceRepository(EnglishCenterContext context) : base(context)
        {

        }

        public Task<bool> ChangeAnswerAAsync(AnswerRcSentence model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerA = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerBAsync(AnswerRcSentence model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerB = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerCAsync(AnswerRcSentence model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerC = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerDAsync(AnswerRcSentence model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerD = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeCorrectAnswerAsync(AnswerRcSentence model, string newCorrectAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.CorrectAnswer = newCorrectAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeExplanationAsync(AnswerRcSentence model, string newExplanation)
        {
            if (model == null) return Task.FromResult(false);

            model.Explanation = newExplanation;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeQuestionAsync(AnswerRcSentence model, string newQuestion)
        {
            if (model == null) return Task.FromResult(false);

            model.Question = newQuestion;

            return Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(long answerId, AnswerRcSentenceDto model)
        {
            var answerModel = await context.AnswerRcSentences.FindAsync(answerId);

            if (answerModel == null)
            {
                return false;
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

            if (answerModel.AnswerD != model.AnswerD)
            {
                var isSuccess = await ChangeAnswerDAsync(answerModel, model.AnswerD);
                if (!isSuccess) return false;
            }

            if (answerModel.CorrectAnswer != model.CorrectAnswer)
            {
                var isSuccess = await ChangeCorrectAnswerAsync(answerModel, model.CorrectAnswer);
                if (!isSuccess) return false;
            }

            if (answerModel.Explanation != model.Explanation)
            {
                var isSuccess = await ChangeExplanationAsync(answerModel, model.Explanation ?? "");
                if (!isSuccess) return false;
            }

            if (answerModel.Question != model.Question)
            {
                var isSuccess = await ChangeQuestionAsync(answerModel, model.Question ?? "");
                if (!isSuccess) return false;
            }

            return true;
        }
    }
}
