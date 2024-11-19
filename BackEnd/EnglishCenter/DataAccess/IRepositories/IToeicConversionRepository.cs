using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IToeicConversionRepository : IGenericRepository<ToeicConversion>
    {
        public Task<ToeicConversion?> GetByNumberCorrectAsync(int numberCorrect, ToeicEnum toeicEnum);
    }
}
