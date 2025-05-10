using Models;

namespace DataAccess.Repository.IRepository;
public interface IStockRepository : IRepository<Stock>
{
    void Update(Stock obj);
}
