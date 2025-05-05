using System.Linq.Expressions;

namespace DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Add(T obj);
        void Delete(T obj);
        Task<T> Get(Expression<Func<T, bool>> filter, string includeProp = null, bool traced = false);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, string includeProp = null);
    }
}
