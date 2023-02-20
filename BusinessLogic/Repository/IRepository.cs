
namespace BusinessLogic.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        
        IQueryable<T> GetIQueryableAll();
       
        T GetById(int id);

        void Add(T entity);
        
        void Update(T entity);
        
        void Remove(T entity);
    }
}
