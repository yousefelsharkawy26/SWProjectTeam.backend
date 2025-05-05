using DataAccess.Context;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository;
public class AddressRepository : Repository<Address>, IAddressRepository
{
    private readonly AppDbContext _db;
    public AddressRepository(AppDbContext db) 
        : base(db)
    {
        _db = db;
    }

    public void Update(Address obj)
    {
        _db.Addresses.Update(obj);
    }
}
