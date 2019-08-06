using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Employee> AddEmployee(Employee employee)
        {
            try
            {
                var result = _context.Employees.Add(employee).Entity;
                await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Employee was not added, error message - {ex.Message}");
            }
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            try
            {
                var result = _context.Employees.Update(employee).Entity;
                await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Employee was not updated, error message - {ex.Message}");
            }
        }

        public async Task DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return;
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

        }

        public bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}