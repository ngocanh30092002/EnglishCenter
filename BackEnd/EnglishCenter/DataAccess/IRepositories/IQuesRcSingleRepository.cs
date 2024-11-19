using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IQuesRcSingleRepository : IGenericRepository<QuesRcSingle>
    {
        public Task<bool> ChangeTimeAsync(QuesRcSingle model, TimeOnly time);
        public Task<bool> ChangeQuantityAsync(QuesRcSingle model, int quantity);
        public Task<bool> ChangeImageAsync(QuesRcSingle model, string imageUrl);
    }
}
