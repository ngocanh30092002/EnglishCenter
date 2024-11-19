using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class SubToeicRepository : GenericRepository<SubToeic>, ISubToeicRepository
    {
        public SubToeicRepository(EnglishCenterContext context) : base(context)
        {
        }

        public override IEnumerable<SubToeic> GetAll()
        {
            return base.Include(s => s.Answer).ToList();
        }

        public override SubToeic GetById(long id)
        {
            return base.Include(s => s.Answer).FirstOrDefault(s => s.SubId == id);
        }

        public Task<bool> ChangeAnswerAAsync(SubToeic subModel, string newAnswer)
        {
            if (subModel == null) return Task.FromResult(false);

            subModel.AnswerA = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerAsync(SubToeic subModel, long answerId)
        {
            if (subModel == null) return Task.FromResult(false);

            var isExistAnswer = context.AnswerToeic.Any(a => a.AnswerId == answerId);
            if (!isExistAnswer) return Task.FromResult(false);

            subModel.AnswerId = answerId;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerBAsync(SubToeic subModel, string newAnswer)
        {
            if (subModel == null) return Task.FromResult(false);

            subModel.AnswerB = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerCAsync(SubToeic subModel, string newAnswer)
        {
            if (subModel == null) return Task.FromResult(false);

            subModel.AnswerC = newAnswer;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeAnswerDAsync(SubToeic subModel, string newAnswer)
        {
            if (subModel == null) return Task.FromResult(false);

            subModel.AnswerD = newAnswer;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeQuesNoAsync(SubToeic subModel, int queNo)
        {
            if (subModel == null) return false;
            if (subModel.QuesNo == queNo) return true;

            if (subModel.QuesToeic.Part == (int)PartEnum.Part1 && (queNo < 1 || queNo > 6))
            {
                return false;
            }
            if (subModel.QuesToeic.Part == (int)PartEnum.Part2 && (queNo < 7 || queNo > 31))
            {
                return false;
            }
            if (subModel.QuesToeic.Part == (int)PartEnum.Part3 && (queNo < 32 || queNo > 70))
            {
                return false;
            }
            if (subModel.QuesToeic.Part == (int)PartEnum.Part4 && (queNo < 32 || queNo > 70))
            {
                return false;
            }
            if (subModel.QuesToeic.Part == (int)PartEnum.Part5 && (queNo < 101 || queNo > 130))
            {
                return false;
            }
            if (subModel.QuesToeic.Part == (int)PartEnum.Part6 && (queNo < 131 || queNo > 146))
            {
                return false;
            }
            if (subModel.QuesToeic.Part == (int)PartEnum.Part7 && (queNo < 147 || queNo > 200))
            {
                return false;
            }

            var noNumSubModel = new SubToeic();
            var minQuesNo = 1;
            var sameSubQues = new List<SubToeic>();

            switch (subModel.QuesToeic.Part)
            {
                case (int)PartEnum.Part1:
                case (int)PartEnum.Part2:
                case (int)PartEnum.Part5:
                    sameSubQues = await context.SubToeic.Include(s => s.QuesToeic)
                                                   .Where(s => s.QuesToeic.ToeicId == subModel.QuesToeic.ToeicId && s.QuesToeic.Part == subModel.QuesToeic.Part)
                                                   .ToListAsync();
                    noNumSubModel = sameSubQues.FirstOrDefault(s => s.QuesNo == queNo);
                    minQuesNo = sameSubQues.Min(s => s.QuesNo);
                    break;
                default:
                    sameSubQues = await context.SubToeic.Where(s => s.QuesId == subModel.QuesId).ToListAsync();
                    noNumSubModel = sameSubQues.FirstOrDefault(s => s.QuesNo == queNo);
                    minQuesNo = sameSubQues.Min(s => s.QuesNo);
                    break;
            }

            if (noNumSubModel == null) return false;

            int indexRemove = sameSubQues.FindIndex(s => s.SubId == subModel.SubId);
            int indexDes = sameSubQues.FindIndex(s => s.SubId == noNumSubModel.SubId);
            var removedModel = sameSubQues.ElementAt(indexRemove);
            sameSubQues.RemoveAt(indexRemove);
            sameSubQues.Insert(indexDes, removedModel);

            foreach (var sub in sameSubQues)
            {
                sub.QuesNo = minQuesNo;
                minQuesNo++;
            }

            return true;
        }

        public Task<bool> ChangeQuestionAsync(SubToeic subModel, string question)
        {
            if (subModel == null) return Task.FromResult(false);

            subModel.Question = question;

            return Task.FromResult(true);
        }
    }
}
