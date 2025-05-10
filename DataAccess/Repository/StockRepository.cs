using DataAccess.Context;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository;
public class StockRepository : Repository<Stock>, IStockRepository
{
    private readonly AppDbContext _db;
    public StockRepository(AppDbContext db) 
        : base(db)
    {
        _db = db;
    }

    public void Update(Stock obj)
    {
        _db.Stocks.Update(obj);
    }
}
