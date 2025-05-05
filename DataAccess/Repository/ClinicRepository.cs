using DataAccess.Context;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository;
public class ClinicRepository : Repository<Clinic>, IClinicRepository
{
    private readonly AppDbContext _db;
    public ClinicRepository(AppDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Clinic obj)
    {
        _db.Clinics.Update(obj);
    }
}
