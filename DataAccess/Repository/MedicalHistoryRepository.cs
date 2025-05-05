using Models;
using DataAccess.Context;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository;

class MedicalHistoryRepository : Repository<MedicalHistory>, IMedicalHistoryRepository
{
    private readonly AppDbContext _db;
    public MedicalHistoryRepository(AppDbContext db) : base(db)
    {
        _db = db;
    }
    public void Update(MedicalHistory medicalHistory)
    {
        _db.MedicalHistories.Update(medicalHistory);
    }
}