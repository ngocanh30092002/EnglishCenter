using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class RoadMapRepository : GenericRepository<RoadMap>, IRoadMapRepository
    {
        public RoadMapRepository(EnglishCenterContext context) : base(context)
        {
        }
    }
}
