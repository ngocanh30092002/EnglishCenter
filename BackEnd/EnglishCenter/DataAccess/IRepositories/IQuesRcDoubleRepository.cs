using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IQuesRcDoubleRepository : IGenericRepository<QuesRcDouble>
    {
        public Task<bool> ChangeTimeAsync(QuesRcDouble model, TimeOnly time);
        public Task<bool> ChangeQuantityAsync(QuesRcDouble model, int quantity);
        public Task<bool> ChangeImage1Async(QuesRcDouble model, string imageUrl);
        public Task<bool> ChangeImage2Async(QuesRcDouble model, string imageUrl);
    }
}
