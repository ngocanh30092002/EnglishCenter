using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.AssignmentRepositories
{
    public class SubLcConRepository : GenericRepository<SubLcConversation>, ISubLcConRepository
    {
        public SubLcConRepository(EnglishCenterContext context) : base(context)
        {
        }

        public Task<bool> ChangeAnswerAAsync(SubLcConversation model, string newAnswer)
        {
            if(model == null) return Task.FromResult(false);

            model.AnswerA = newAnswer;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeAnswerAsync(SubLcConversation model, long answerId)
        {
            if (model == null) return false;

            var answerModel = await context.AnswerLcConversations
                        .Include(a => a.SubLcConversation)
                        .FirstOrDefaultAsync(a => a.AnswerId == answerId);

            if (answerModel == null) return false;
            if (answerModel.SubLcConversation != null) return false;

            model.AnswerId = answerId;
            return true;
        }

        public Task<bool> ChangeAnswerBAsync(SubLcConversation model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerB = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerCAsync(SubLcConversation model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerC = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerDAsync(SubLcConversation model, string newAnswer)
        {
            if (model == null) return Task.FromResult(false);

            model.AnswerD = newAnswer;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeNoNumAsync(SubLcConversation model, int noNum)
        {
            if (noNum <= 0) return false;

            var subModels = context.SubLcConversations
                                .Where(s => s.PreQuesId == model.PreQuesId)
                                .OrderBy(s => s.NoNum);

            var maxNoNum = await subModels.MaxAsync(s => s.NoNum);
            if (noNum > maxNoNum) return false;

            var subModelList = await subModels.ToListAsync();
            var index = subModelList.FindIndex(s => s.SubId == model.SubId);
            var itemMove = subModelList.ElementAt(index);
            
            subModelList.RemoveAt(index);
            subModelList.Insert(noNum - 1, itemMove);

            for(int i = 0; i < maxNoNum; i++)
            {
                subModelList[i].NoNum = i + 1;
            }

            return true;
        }

        public async Task<bool> ChangePreQuesAsync(SubLcConversation model, long preId)
        {
            var preQuesModel = await context.QuesLcConversations
                                            .Include(q => q.SubLcConversations)
                                            .FirstOrDefaultAsync(q => q.QuesId == preId);

            if (preQuesModel == null) return false;
            if(preQuesModel.SubLcConversations.Count >= preQuesModel.Quantity ) return false;


            var previousSubModels = await context.SubLcConversations
                                                .Where(s => s.SubId != model.SubId && s.PreQuesId == model.PreQuesId)
                                                .OrderBy(s => s.NoNum)
                                                .ToListAsync();

            int i = 1;
            foreach(var item in previousSubModels)
            {
                item.NoNum = i++;
            }

            var maxCurrentSubModel = await context.SubLcConversations
                                                .Where(s => s.PreQuesId == preId)
                                                .Select(s => (int?) s.NoNum)
                                                .MaxAsync();


            model.PreQuesId = preId;
            model.NoNum = maxCurrentSubModel.HasValue ? maxCurrentSubModel.Value + 1 : 1;

            return true;
        }

        public Task<bool> ChangeQuestionAsync(SubLcConversation model, string newQues)
        {
            if (model == null) return Task.FromResult(false);

            model.Question = newQues;

            return Task.FromResult(true);
        }
    }
}
