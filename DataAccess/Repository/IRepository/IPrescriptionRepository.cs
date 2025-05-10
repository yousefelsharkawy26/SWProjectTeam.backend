using Models;

namespace DataAccess.Repository.IRepository;

public interface IPrescriptionRepository : IRepository<Prescription>
{
    void Update(Prescription obj);
}
