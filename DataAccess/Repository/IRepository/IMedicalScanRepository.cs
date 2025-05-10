using Models;

namespace DataAccess.Repository.IRepository;

public interface IMedicalScanRepository : IRepository<MedicalScan>
{
    void Update(MedicalScan obj);
}
