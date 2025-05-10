using Models;

namespace DataAccess.Repository.IRepository;
public interface IDiagnosisRepository : IRepository<Diagnosis>
{
    void Update(Diagnosis obj);
}
