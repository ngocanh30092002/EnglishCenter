using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class RandomQueToeicRepository : GenericRepository<RandomQuesToeic>, IRandomQueToeicRepository
    {
        public RandomQueToeicRepository(EnglishCenterContext context) : base(context)
        {

        }

        public async Task<bool> ChangeHomeworkAsync(RandomQuesToeic randomQuesToeic, long id)
        {
            if (randomQuesToeic == null) return false;

            var homeworkModel = await context.Homework.FindAsync(id);
            if (homeworkModel == null) return false;
            if (homeworkModel.Type == 1) return false;

            randomQuesToeic.HomeworkId = id;
            randomQuesToeic.RoadMapExamId = null;

            return true;
        }

        public async Task<bool> ChangeQuesToeicAsync(RandomQuesToeic randomQuesToeic, long id)
        {
            if (randomQuesToeic == null) return false;

            var queModel = await context.QuesToeic.FindAsync(id);
            if (queModel == null) return false;

            randomQuesToeic.QuesToeicId = id;

            return true;
        }

        public async Task<bool> ChangeRoadMapExamAsync(RandomQuesToeic randomQuesToeic, long id)
        {
            if (randomQuesToeic == null) return false;

            var roadMapExam = await context.RoadMapExams.FindAsync(id);
            if (roadMapExam == null) return false;

            randomQuesToeic.HomeworkId = null;
            randomQuesToeic.RoadMapExamId = id;

            return true;
        }

        public Task<int> GetNumberByPartHwAsync(long homeworkId, int part)
        {
            var ranQues = context.RandomQues
                                .Include(r => r.QuesToeic)
                                .ThenInclude(q => q.SubToeicList)
                                .Where(r => r.HomeworkId == homeworkId && r.QuesToeic.Part == part)
                                .SelectMany(r => r.QuesToeic.SubToeicList)
                                .Count();

            return Task.FromResult(ranQues);
        }

        public Task<int> GetNumberByPartRmAsync(long roadMapExamId, int part)
        {
            var ranQues = context.RandomQues
                               .Include(r => r.QuesToeic)
                               .ThenInclude(q => q.SubToeicList)
                               .Where(r => r.RoadMapExamId == roadMapExamId && r.QuesToeic.Part == part)
                               .SelectMany(r => r.QuesToeic.SubToeicList)
                               .Count();

            return Task.FromResult(ranQues);
        }

        public async Task<int> GetTotalNumberQuesAsync(long roadMapExamId, bool isListening = true)
        {

            if (isListening)
            {
                var numberListening = await context.RandomQues
                                        .Include(r => r.QuesToeic)
                                        .ThenInclude(r => r.SubToeicList)
                                        .Where(r => r.RoadMapExamId == roadMapExamId && r.QuesToeic.Part <= (int)PartEnum.Part4)
                                        .SelectMany(r => r.QuesToeic.SubToeicList)
                                        .CountAsync();
                return numberListening;
            }
            else
            {
                var numberReading = await context.RandomQues
                                        .Include(r => r.QuesToeic)
                                        .ThenInclude(r => r.SubToeicList)
                                        .Where(r => r.RoadMapExamId == roadMapExamId && r.QuesToeic.Part >= (int)PartEnum.Part4)
                                        .SelectMany(r => r.QuesToeic.SubToeicList)
                                        .CountAsync();
                return numberReading;
            }
        }
    }
}
