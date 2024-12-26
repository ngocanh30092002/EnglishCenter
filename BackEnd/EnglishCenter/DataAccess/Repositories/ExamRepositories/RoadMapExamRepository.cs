using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class RoadMapExamRepository : GenericRepository<RoadMapExam>, IRoadMapExamRepository
    {
        public RoadMapExamRepository(EnglishCenterContext context) : base(context)
        {

        }

        public Task<bool> ChangeNameAsync(RoadMapExam model, string newName)
        {
            if (model == null) return Task.FromResult(false);

            model.Name = newName;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeRoadMapAsync(RoadMapExam model, long roadMapId)
        {
            if (model == null) return false;

            var roadMapModel = await context.RoadMaps.FindAsync(roadMapId);
            if (roadMapModel == null) return false;

            model.RoadMapId = roadMapId;

            return true;
        }

        public Task<bool> ChangeTimeAsync(RoadMapExam model, double timeMinute)
        {
            if (model == null) return Task.FromResult(false);

            if (timeMinute < 0) return Task.FromResult(false);

            model.TimeMinutes = timeMinute;

            return Task.FromResult(true);
        }
    }
}
