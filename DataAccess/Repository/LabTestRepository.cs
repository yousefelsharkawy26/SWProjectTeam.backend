using DataAccess.Context;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository;
public class LabTestRepository : Repository<LabTest>, ILabTestRepository
{
    private readonly AppDbContext _db;
    public LabTestRepository(AppDbContext db) 
        : base(db)
    {
        _db = db;
    }

    public void Update(LabTest obj)
    {
        _db.LabTests.Update(obj);
    }
}
