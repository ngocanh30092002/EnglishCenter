using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.CourseRepositories
{
    public class ToeicConversionRepository : GenericRepository<ToeicConversion>, IToeicConversionRepository
    {
        public ToeicConversionRepository(EnglishCenterContext context) : base(context)
        {

        }
    }
}
