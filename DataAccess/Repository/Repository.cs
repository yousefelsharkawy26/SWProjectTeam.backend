using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DataAccess.Context;

public class Repository<T> : IRepository<T> where T : class
{
    AppDbContext _db;
    DbSet<T> _Set;
    public Repository(AppDbContext db)
    {
        _db = db;
        _Set = db.Set<T>();
    }
    public async Task Add(T obj)
    {
        await _Set.AddAsync(obj);
    }

    public void Delete(T obj)
    {
        _Set.Remove(obj);
    }

    public async Task<T> Get(Expression<Func<T, bool>> filter, string includeProp = null, bool traced = false)
    {
        IQueryable<T> query = _Set;
        if (!traced)
            query = query.AsNoTracking();
        if (includeProp != null)
        {
            foreach (var prop in includeProp
                .Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(prop);
            }
        }

        return await query.FirstOrDefaultAsync(filter);
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, string includeProp = null)
    {
        IQueryable<T> query = _Set;

        if (filter != null)
            query = query.Where(filter);

        if (includeProp != null)
        {
            foreach(var prop in includeProp
                .Split(new char[] {','} 
                ,StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(prop);
            }
        }

        return query;
    }
}
