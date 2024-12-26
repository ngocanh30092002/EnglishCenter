using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class QuesToeicRepository : GenericRepository<QuesToeic>, IQuesToeicRepository
    {
        public QuesToeicRepository(EnglishCenterContext context) : base(context)
        {

        }

        public async Task<bool> LoadSubQuesAsync(QuesToeic queModel)
        {
            if (queModel == null) return false;

            await context.Entry(queModel)
                .Collection(q => q.SubToeicList)
                .LoadAsync();

            return true;
        }

        public async Task<bool> LoadSubQuesWithAnswer(QuesToeic queModel)
        {
            if (queModel == null) return false;

            await context.Entry(queModel)
                .Collection(q => q.SubToeicList)
                .Query()
                .Include(s => s.Answer)
                .LoadAsync();

            return true;
        }

        public Task<bool> ChangeAudioAsync(QuesToeic queModel, string audioPath)
        {
            if (queModel == null) return Task.FromResult(false);

            queModel.Audio = audioPath;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeGroupAsync(QuesToeic queModel, bool isGroup)
        {
            if (queModel == null) return Task.FromResult(false);

            var numRecords = context.SubToeic.Where(s => s.QuesId == queModel.QuesId).Count();
            if (numRecords > 1 && !isGroup) return Task.FromResult(false);
            if (numRecords <= 1 && isGroup) return Task.FromResult(false);

            queModel.IsGroup = isGroup;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeImage1Async(QuesToeic queModel, string imagePath)
        {
            if (queModel == null) return Task.FromResult(false);

            queModel.Image_1 = string.IsNullOrEmpty(imagePath) ? null : imagePath;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeImage2Async(QuesToeic queModel, string imagePath)
        {
            if (queModel == null) return Task.FromResult(false);

            queModel.Image_2 = string.IsNullOrEmpty(imagePath) ? null : imagePath;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeImage3Async(QuesToeic queModel, string imagePath)
        {
            if (queModel == null) return Task.FromResult(false);

            queModel.Image_3 = string.IsNullOrEmpty(imagePath) ? null : imagePath;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeLevelAsync(QuesToeic queModel, int level)
        {
            if (queModel == null) return Task.FromResult(false);
            if (level <= 0 || level > 4) return Task.FromResult(false);

            queModel.Level = level;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeNoNumAsync(QuesToeic queModel, int noNum)
        {
            if (queModel == null) return false;

            var noNumQueModel = context.QuesToeic
                                       .FirstOrDefault(q => q.ToeicId == queModel.ToeicId &&
                                                            q.NoNum == noNum &&
                                                            q.Part == queModel.Part);

            if (noNumQueModel == null) return false;
            if (queModel.QuesId == noNumQueModel.QuesId) return true;

            var sameQueModels = await context.QuesToeic
                                             .Include(q => q.SubToeicList)
                                             .Where(q => q.ToeicId == queModel.ToeicId && q.Part == queModel.Part)
                                             .OrderBy(q => q.NoNum)
                                             .ToListAsync();

            int minQues = sameQueModels.Min(q => (int?)q.NoNum) ?? 1;
            var indexRemove = sameQueModels.FindIndex(q => q.QuesId == queModel.QuesId);
            var indexDes = sameQueModels.FindIndex(q => q.QuesId == noNumQueModel.QuesId);
            var removedModel = sameQueModels.ElementAt(indexRemove);

            sameQueModels.RemoveAt(indexRemove);
            sameQueModels.Insert(indexDes, removedModel);

            var minQueNo = await context.SubToeic
                                        .Include(q => q.QuesToeic)
                                        .Where(q => q.QuesToeic.ToeicId == queModel.ToeicId && q.QuesToeic.Part == queModel.Part)
                                        .Select(q => (int?)q.QuesNo)
                                        .MinAsync();

            switch (queModel.Part)
            {
                case (int)PartEnum.Part1:
                    minQueNo = minQueNo ?? 1;
                    break;
                case (int)PartEnum.Part2:
                    minQueNo = minQueNo ?? 7;
                    break;
                case (int)PartEnum.Part3:
                    minQueNo = minQueNo ?? 32;
                    break;
                case (int)PartEnum.Part4:
                    minQueNo = minQueNo ?? 71;
                    break;
                case (int)PartEnum.Part5:
                    minQueNo = minQueNo ?? 101;
                    break;
                case (int)PartEnum.Part6:
                    minQueNo = minQueNo ?? 131;
                    break;
                case (int)PartEnum.Part7:
                    minQueNo = minQueNo ?? 147;
                    break;
            }

            foreach (var que in sameQueModels)
            {
                que.NoNum = minQues;
                minQues++;

                foreach (var subQue in que.SubToeicList)
                {
                    subQue.QuesNo = minQueNo.Value;
                    minQueNo++;
                }
            }

            return true;
        }
    }
}
