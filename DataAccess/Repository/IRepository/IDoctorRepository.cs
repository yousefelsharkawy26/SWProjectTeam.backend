using Models;

namespace DataAccess.Repository.IRepository;
public interface IDoctorRepository : IRepository<Doctor>
{
    void Update(Doctor doctor);
}
