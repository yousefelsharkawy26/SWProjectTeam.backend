using DataAccess.Context;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository;
public class PrescriptionRepository : Repository<Prescription>, IPrescriptionRepository
{
    private readonly AppDbContext _db;
    public PrescriptionRepository(AppDbContext db) 
        : base(db)
    {
        _db = db;
    }

    public void Update(Prescription obj)
    {
        _db.Prescriptions.Update(obj);
    }
}
