using DataAccess.Context;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository;
public class InventoryRepository : Repository<Inventory>, IInventoryRepository
{
    private readonly AppDbContext _db;
    public InventoryRepository(AppDbContext db) 
        : base(db)
    {
        _db = db;
    }

    public void Update(Inventory obj)
    {
        _db.Inventories.Update(obj);
    }
}
