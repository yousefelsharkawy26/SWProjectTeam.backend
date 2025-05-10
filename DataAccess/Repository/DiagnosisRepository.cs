using DataAccess.Context;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository;
public class DiagnosisRepository : Repository<Diagnosis>, IDiagnosisRepository
{
    private readonly AppDbContext _db;
    public DiagnosisRepository(AppDbContext db) 
        : base(db)
    {
        _db = db;
    }

    public void Update(Diagnosis obj)
    {
        _db.Diagnoses.Update(obj);
    }
}
