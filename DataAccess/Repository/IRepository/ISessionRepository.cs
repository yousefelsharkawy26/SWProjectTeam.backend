using Models;

namespace DataAccess.Repository.IRepository;

public interface ISessionRepository : IRepository<PlanSession>
{
    void Update(PlanSession obj);
}
