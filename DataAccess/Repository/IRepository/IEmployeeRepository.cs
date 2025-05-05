using Models;

namespace DataAccess.Repository.IRepository;
public interface IEmployeeRepository: IRepository<Employee>
{
    void Update(Employee employee);
}
