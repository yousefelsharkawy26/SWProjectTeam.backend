using Models;

namespace DataAccess.Repository.IRepository;

public interface IClinicRepository : IRepository<Clinic>
{
    void Update(Clinic obj);
}
