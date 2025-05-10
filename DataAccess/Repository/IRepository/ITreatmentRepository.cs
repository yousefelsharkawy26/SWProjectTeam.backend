using Models;

namespace DataAccess.Repository.IRepository;
public interface ITreatmentRepository : IRepository<TreatmentPlan>
{
    void Update(TreatmentPlan obj);
}
