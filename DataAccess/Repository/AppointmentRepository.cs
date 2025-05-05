using DataAccess.Context;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository;

class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    private readonly AppDbContext _db;
    public AppointmentRepository(AppDbContext db)
        : base(db)
    {
        _db = db;
    }

    public void Update(Appointment appointment)
    {
        _db.Appointments.Update(appointment);
    }

}
