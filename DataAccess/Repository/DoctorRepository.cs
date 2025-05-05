using Models;
using DataAccess.Context;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository;
class DoctorRepository : Repository<Doctor>, IDoctorRepository
{
    private readonly AppDbContext _db;
    public DoctorRepository(AppDbContext db) : base(db)
    {
        _db = db;
    }
    public void Update(Doctor doctor)
    {
        _db.Doctors.Update(doctor);
    }
}
