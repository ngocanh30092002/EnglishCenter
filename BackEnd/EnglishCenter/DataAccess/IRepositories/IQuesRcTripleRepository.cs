using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IQuesRcTripleRepository : IGenericRepository<QuesRcTriple>
    {
        public Task<bool> ChangeTimeAsync(QuesRcTriple model, TimeOnly time);
        public Task<bool> ChangeQuantityAsync(QuesRcTriple model, int quantity);
        public Task<bool> ChangeImage1Async(QuesRcTriple model, string imageUrl);
        public Task<bool> ChangeImage2Async(QuesRcTriple model, string imageUrl);
        public Task<bool> ChangeImage3Async(QuesRcTriple model, string imageUrl);
    }
}
