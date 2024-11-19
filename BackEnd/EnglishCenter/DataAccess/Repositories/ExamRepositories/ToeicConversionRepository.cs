using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class ToeicConversionRepository : GenericRepository<ToeicConversion>, IToeicConversionRepository
    {
        public ToeicConversionRepository(EnglishCenterContext context) : base(context)
        {

        }

        public Task<ToeicConversion?> GetByNumberCorrectAsync(int numberCorrect, ToeicEnum toeicEnum)
        {
            var record = context.ToeicConversion
                                .Where(t => t.NumberCorrect == numberCorrect && t.Section == toeicEnum.ToString())
                                .FirstOrDefault();

            return Task.FromResult(record);
        }
    }
}
