using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDD.Data.Library;
using TDD.Domain.Library.Entities;
using TDD.Repositories.Library.Core;

namespace TDD.Repositories.Library
{
    public class EmpoyeeRepository : IEmpoyeeRepository
    {
        private readonly EmployeeContext _context;

        public EmpoyeeRepository(EmployeeContext employeeContext)
        {
            _context = employeeContext;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
        }
    }
}
