using Models;
using DataAccess.Context;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository;

class MedicalExaminationRepository : Repository<MedicalExamination>, IMedicalExaminationRepository
{
    private readonly AppDbContext _db;
    public MedicalExaminationRepository(AppDbContext db) : base(db)
    {
        _db = db;
    }
    public void Update(MedicalExamination obj)
    {
        _db.MedicalExaminations.Update(obj);
    }
}