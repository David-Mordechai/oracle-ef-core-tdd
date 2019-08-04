using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDD.Data.Library;
using TDD.Domain.Library.Entities;
using Xunit;

namespace TDD.Repositories.Library.Tests
{
    // in this link we have examples how to use in memory database for tests
    //https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/
    public class EmpoyeeRepositoryTests 
    {

        [Fact]
        public async void GetEmployees_NoCondition_ReturnsAllEmployees()
        {
            var options = new DbContextOptionsBuilder<EmployeeContext>()
                .UseInMemoryDatabase(databaseName: "GetEmployees_NoCondition_ReturnsAllEmployees")
                .Options;

            // Run the test against one instance of the context
            using (var context = new EmployeeContext(options))
            {
                var repository = new EmpoyeeRepository(context);
                await SeedEmployiees(context);

                var result = await repository.GetEmployees();
                Assert.True(result.Count() == 2);
            }
        }

        [Fact]
        public async void GetEmployeeById_EmployeeFound_ReturnsEmployee()
        {
            var options = new DbContextOptionsBuilder<EmployeeContext>()
                .UseInMemoryDatabase(databaseName: "GetEmployeeById_EmployeeFound_ReturnsEmployee")
                .Options;

            // Run the test against one instance of the context
            using (var context = new EmployeeContext(options))
            {
                var repository = new EmpoyeeRepository(context);
                await SeedEmployiees(context);

                var result = await repository.GetEmployeeById(1);
                Assert.True(result.EmployeeId == 1);
            }
        }

        [Fact]
        public async void GetEmployeeById_EmployeeNotFound_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<EmployeeContext>()
                .UseInMemoryDatabase(databaseName: "GetEmployeeById_EmployeeNotFound_ReturnsNull")
                .Options;

            // Run the test against one instance of the context
            using (var context = new EmployeeContext(options))
            {
                var repository = new EmpoyeeRepository(context);
                await SeedEmployiees(context);

                var result = await repository.GetEmployeeById(3);
                Assert.True(result == null);
            }
        }

        async Task SeedEmployiees(EmployeeContext context)
        {
            var employeesList = new List<Employee>
            {
                new Employee { EmployeeId= 1, FirstName="Tom", LastName= "Cat",DateOfBirth=DateTime.Now.AddDays(-365), PhoneNumber ="055-5555555", Email="Tom.Cat@gmail.com"},
                new Employee { EmployeeId= 2, FirstName="Jerry", LastName= "Mouse",DateOfBirth=DateTime.Now.AddDays(-100), PhoneNumber ="057-7777777", Email="Jerry.Mouse@gmail.com"}
            };
            await context.AddRangeAsync(employeesList);
            context.SaveChanges();
        }
    }

}
