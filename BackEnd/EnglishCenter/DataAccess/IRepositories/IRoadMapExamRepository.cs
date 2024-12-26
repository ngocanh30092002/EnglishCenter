using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IRoadMapExamRepository : IGenericRepository<RoadMapExam>
    {
        public Task<bool> ChangeNameAsync(RoadMapExam model, string newName);
        public Task<bool> ChangeRoadMapAsync(RoadMapExam model, long roadMapId);
        public Task<bool> ChangeTimeAsync(RoadMapExam model, double timeMinute);
    }
}
