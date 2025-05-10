using DataAccess.Context;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository;
public class MedicalScanRepository : Repository<MedicalScan>, IMedicalScanRepository
{
    private readonly AppDbContext _db;
    public MedicalScanRepository(AppDbContext db) 
        : base(db)
    {
        _db = db;
    }

    public void Update(MedicalScan obj)
    {
        _db.MedicalScans.Update(obj);
    }
}
