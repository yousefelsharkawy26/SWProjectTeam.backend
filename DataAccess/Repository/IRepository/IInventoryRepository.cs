using Models;

namespace DataAccess.Repository.IRepository;
public interface IInventoryRepository : IRepository<Inventory>
{
    void Update(Inventory obj);
}
