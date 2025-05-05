using Models;

namespace DataAccess.Repository.IRepository;

public interface IAllergicRepository : IRepository<Allergic>
{
    void Update(Allergic obj);
}
