using System.Linq.Expressions;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly EnglishCenterContext context;

        public GenericRepository(EnglishCenterContext context) 
        {
            this.context = context;
        }
        public virtual void Add(T model)
        {
            context.Set<T>().Add(model);
        }

        public virtual void AddRange(IEnumerable<T> models)
        {
            context.Set<T>().AddRange(models);
        }

        public virtual void Remove(T model)
        {
            context.Set<T>().Remove(model);
        }

        public virtual void RemoveRange(IEnumerable<T> models)
        {
            context.Set<T>().RemoveRange(models);
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return context.Set<T>().Where(expression);
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();
            
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            return query;
        }

        public virtual T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }

        public virtual T GetById(string id)
        {
            return context.Set<T>().Find(id);
        }

        public virtual T GetById(long id)
        {
            return context.Set<T>().Find(id);
        }
        public virtual IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public virtual bool IsExist(Expression<Func<T,bool>> expression)
        {
            return context.Set<T>().Any(expression);
        }
    }
}
