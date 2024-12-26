using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.ClassRepositories
{
    public class PeriodRepository : GenericRepository<Period>, IPeriodRepository
    {
        public PeriodRepository(EnglishCenterContext context) : base(context)
        {
        }
    }
}
