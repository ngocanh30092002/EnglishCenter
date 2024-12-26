using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IRandomQueToeicRepository : IGenericRepository<RandomQuesToeic>
    {
        public Task<bool> ChangeRoadMapExamAsync(RandomQuesToeic randomQuesToeic, long id);
        public Task<bool> ChangeHomeworkAsync(RandomQuesToeic randomQuesToeic, long id);
        public Task<bool> ChangeQuesToeicAsync(RandomQuesToeic randomQuesToeic, long id);
        public Task<int> GetTotalNumberQuesAsync(long roadMapExamId, bool isListening = true);
        public Task<int> GetNumberByPartHwAsync(long homeworkId, int part);
        public Task<int> GetNumberByPartRmAsync(long roadMapExamId, int part);
    }
}
