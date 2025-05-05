using Models;
using DataAccess.Repository.IRepository;
using DataAccess.Context;

namespace DataAccess.Repository;

class AllergicRepository: Repository<Allergic>, IAllergicRepository
{
    private readonly AppDbContext _db;
    public AllergicRepository(AppDbContext db) : base(db)
    {
        _db = db;
    }
    public void Update(Allergic obj)
    {
        _db.Allergies.Update(obj);
    }
}
