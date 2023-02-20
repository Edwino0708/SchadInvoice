using BusinessLogic.Entity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly TestInvoiceContext _dbContext;
        private Microsoft.EntityFrameworkCore.DbSet<T> _set = null;

        public GenericRepository(TestInvoiceContext dbContext)
        {
            _dbContext = dbContext;
            _set = _dbContext.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _set.ToList();
        }

        public IQueryable<T> GetIQueryableAll()
        {
            return _set.AsQueryable();
        }

        public T GetById(int id)
        {
            return _set.Find(id);
        }

        public void Remove(T entity)
        {
            _set.Remove(entity);
        }
        public void Add(T entity)
        {
            _set.Add(entity);
        }

        public void Update(T entity)
        {
            _set.Update(entity);
        }
    }
}
