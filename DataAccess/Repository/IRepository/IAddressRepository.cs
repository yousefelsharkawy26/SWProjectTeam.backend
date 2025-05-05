using Models;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository.IRepository;

public interface IAddressRepository : IRepository<Address>
{
    void Update(Address obj);
}
