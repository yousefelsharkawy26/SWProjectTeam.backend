using Models;

namespace DataAccess.Repository.IRepository;
public interface IMedicalHistoryRepository : IRepository<MedicalHistory>
{
    void Update(MedicalHistory medicalHistory);
}
