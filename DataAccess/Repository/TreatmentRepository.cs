using Models;
using DataAccess.Context;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository;
public class TreatmentRepository : Repository<TreatmentPlan>, ITreatmentRepository
{
    private readonly AppDbContext _db;
    public TreatmentRepository(AppDbContext db) 
        : base(db)
    {
        _db = db;
    }

    public void Update(TreatmentPlan obj)
    {
        _db.Treatments.Update(obj);
    }
}
