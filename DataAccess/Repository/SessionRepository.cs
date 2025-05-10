using DataAccess.Context;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository;
public class SessionRepository : Repository<PlanSession>, ISessionRepository
{
    private readonly AppDbContext _db;
    public SessionRepository(AppDbContext db) 
        : base(db)
    {
        _db = db;
    }

    public void Update(PlanSession obj)
    {
        _db.Sessions.Update(obj);
    }
}
