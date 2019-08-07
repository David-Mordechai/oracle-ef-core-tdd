using System;
using System.Collections.Generic;
using System.Linq;
using TDD.Data.Library;
using TDD.Domain.Library.Entities;
using Xunit;

namespace TDD.Repositories.Library.Tests
{
    // in this link we have examples how to use in memory database for tests
    //https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/
    public class EmpoyeeRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private EmployeeContext Context => _fixture.Db;
        private EmpoyeeRepository Repository =>  new EmpoyeeRepository(Context);

        public EmpoyeeRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void GetEmployees_NoCondition_ReturnsAllEmployees()
        {
            var result = await Repository.GetEmployees();
            var count = Context.Employees.Count();
            Assert.True(result.Count() == count);
            Assert.True(result.GetType() == typeof(List<Employee>));
        }

        [Fact]
        public async void GetEmployeeById_EmployeeFound_ReturnsEmployee()
        {
            Assert.NotNull(await Repository.GetEmployeeById(2));
        }

        [Fact]
        public async void GetEmployeeById_EmployeeNotFound_ReturnsNull()
        {
            Assert.Null(await Repository.GetEmployeeById(0));
        }

        [Fact]
        public async void AddEmployee_Success_ReturnsAddedEmployee()
        {
            var employeeToAdd = new Employee
            {
                EmployeeId = 4,
                FirstName = "New",
                LastName = "Employee",
                DateOfBirth = DateTime.Now.AddDays(-180),
                PhoneNumber = "055-5555555",
                Email = "New.Employee@gmail.com"
            };
            await Repository.AddEmployee(employeeToAdd);
            var savedEmployee = await Repository.GetEmployeeById(4);
            Assert.NotNull(savedEmployee);   
        }

        [Fact]
        public async void AddEmployee_Failure_ReturnsException()
        {
            await Assert.ThrowsAsync<Exception>(() => Repository.AddEmployee(null));
        }

        [Fact]
        public async void UpdateEmployee_Success_ReturnsUpdatedEmployee()
        {
            var employeeToUpdate = Context.Employees.Single(x => x.EmployeeId == 2);
            employeeToUpdate.FirstName = "New updated";
            await Repository.UpdateEmployee(employeeToUpdate);
            var savedEmployee = Context.Employees.Single(x => x.EmployeeId == 2);
            Assert.Equal("New updated", savedEmployee.FirstName );
        }

        [Fact]
        public async void UpdateEmployee_Failure_ReturnsException()
        {
            await Assert.ThrowsAsync<Exception>(() => Repository.UpdateEmployee(null));
        }

        [Fact]
        public async void DeleteEmployee_Success_ReturnsVoid()
        {
            await Repository.DeleteEmployee(3);
            var deletedEmployee = Context.Employees.FirstOrDefault(x => x.EmployeeId == 3);
            Assert.Null(deletedEmployee);
        }

        [Fact]
        public async void DeleteEmployee_NotFound_EmployeeNotRemoved()
        {
            var employee = await Context.Employees.FindAsync(0);
            await Repository.DeleteEmployee(0);
            Assert.Null(employee);
        }

        [Fact]
        public void EmployeeExist_Exist_ReturnsTrue()
        {
            var result = Repository.EmployeeExists(1);
            Assert.True(result);
        }

        [Fact]
        public void EmployeeExist_NotExist_ReturnsFalse()
        {
            var result = Repository.EmployeeExists(0);
            Assert.False(result);
        }
    }
}
