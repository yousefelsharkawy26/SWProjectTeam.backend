using Models;
using DataAccess.Context;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository;
class EmployeeRepository: Repository<Employee>, IEmployeeRepository
{
    private readonly AppDbContext _db;

    public EmployeeRepository(AppDbContext db)
        : base(db) { _db = db; }

    public void Update(Employee employee)
    {
        _db.Employees.Update(employee);
    }
}
