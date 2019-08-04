﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TDD.Domain.Library.Entities;

namespace TDD.Repositories.Library.Core
{
    public interface IEmpoyeeRepository
    {
        Task<List<Employee>> GetEmployees();
        Task<Employee> GetEmployeeById(int id);
    }
}
