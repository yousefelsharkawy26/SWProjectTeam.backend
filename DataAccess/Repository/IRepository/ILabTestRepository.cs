using Models;

namespace DataAccess.Repository.IRepository;

public interface ILabTestRepository : IRepository<LabTest>
{
    void Update(LabTest obj);
}
