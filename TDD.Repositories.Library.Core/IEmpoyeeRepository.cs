using System.Collections.Generic;
using System.Threading.Tasks;
using TDD.Domain.Library.Entities;

namespace TDD.Repositories.Library.Core
{
    public interface IEmpoyeeRepository
    {
        Task<List<Employee>> GetEmployees();
        Task<Employee> GetEmployeeById(int id);
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee> UpdateEmployee(Employee employee);
        Task DeleteEmployee(int id);
        bool EmployeeExists(int id);
    }
}
