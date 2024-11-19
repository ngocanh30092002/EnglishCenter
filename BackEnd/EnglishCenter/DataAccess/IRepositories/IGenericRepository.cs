using System.Linq.Expressions;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        public T GetById(int id);
        public T GetById(long id);
        public T GetById(string id);
        public IEnumerable<T> GetAll();
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        public void Add(T model);
        public void AddRange(IEnumerable<T> models);
        public void Remove(T model);
        public void RemoveRange(IEnumerable<T> models);
        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
        public bool IsExist(Expression<Func<T,bool>> expression);
    }
}
