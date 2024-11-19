using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class SubRcDoubleRepository : GenericRepository<SubRcDouble>, ISubRcDoubleRepository
    {
        public SubRcDoubleRepository(EnglishCenterContext context) : base(context)
        {

        }

        public Task<bool> ChangeAnswerAAsync(SubRcDouble model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerA = newAnswer;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeAnswerAsync(SubRcDouble model, long answerId)
        {
            if (model == null) return false;

            var answerModel = await context.AnswerRcDoubles
                        .Include(a => a.SubRcDouble)
                        .FirstOrDefaultAsync(a => a.AnswerId == answerId);

            if (answerModel == null) return false;
            if (answerModel.SubRcDouble != null) return false;

            model.AnswerId = answerId;
            return true;
        }

        public Task<bool> ChangeAnswerBAsync(SubRcDouble model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerB = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerCAsync(SubRcDouble model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerC = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerDAsync(SubRcDouble model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerD = newAnswer;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeNoNumAsync(SubRcDouble model, int noNum)
        {
            if (model == null) return false;
            if (noNum <= 0) return false;

            var subModels = context.SubRcDoubles
                                .Where(s => s.PreQuesId == model.PreQuesId)
                                .OrderBy(s => s.NoNum);

            if (!subModels.Any()) return false;

            var maxNoNum = await subModels.MaxAsync(s => s.NoNum);
            if (noNum > maxNoNum) return false;

            var subModelList = await subModels.ToListAsync();
            var index = subModelList.FindIndex(s => s.SubId == model.SubId);
            var itemMove = subModelList.ElementAt(index);

            subModelList.RemoveAt(index);
            subModelList.Insert(noNum - 1, itemMove);

            for (int i = 0; i < maxNoNum; i++)
            {
                subModelList[i].NoNum = i + 1;
            }

            return true;
        }

        public async Task<bool> ChangePreQuesAsync(SubRcDouble model, long preId)
        {
            if (model == null) return false;

            var preQuesModel = await context.QuesRcDoubles
                                            .Include(q => q.SubRcDoubles)
                                            .FirstOrDefaultAsync(q => q.QuesId == preId);

            if (preQuesModel == null) return false;
            if (preQuesModel.SubRcDoubles.Count >= preQuesModel.Quantity) return false;


            var previousSubModels = await context.SubRcDoubles
                                                .Where(s => s.SubId != model.SubId && s.PreQuesId == model.PreQuesId)
                                                .OrderBy(s => s.NoNum)
                                                .ToListAsync();

            int i = 1;
            foreach (var item in previousSubModels)
            {
                item.NoNum = i++;
            }

            var maxCurrentSubModel = await context.SubRcDoubles
                                                .Where(s => s.PreQuesId == preId)
                                                .Select(s => (int?) s.NoNum)
                                                .MaxAsync();


            model.PreQuesId = preId;
            model.NoNum = maxCurrentSubModel.HasValue ? maxCurrentSubModel.Value + 1 : 1;

            return true;
        }

        public Task<bool> ChangeQuestionAsync(SubRcDouble model, string newQues)
        {
            if (model == null) return Task.FromResult(false);

            model.Question = newQues;

            return Task.FromResult(true);
        }
    }
}
