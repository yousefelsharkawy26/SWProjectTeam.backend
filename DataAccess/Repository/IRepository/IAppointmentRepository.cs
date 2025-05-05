using Models;

namespace DataAccess.Repository.IRepository;
public interface IAppointmentRepository : IRepository<Appointment>
{
    void Update(Appointment appointment);
}
