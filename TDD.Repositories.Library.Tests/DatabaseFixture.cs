using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TDD.Data.Library;
using TDD.Domain.Library.Entities;

namespace TDD.Repositories.Library.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<EmployeeContext>()
                .UseInMemoryDatabase(databaseName: $"db_employee_tests")
                .Options;

            Db = new EmployeeContext(options);
            SeedEmployiees();
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public EmployeeContext Db { get; private set; }


        async void SeedEmployiees()
        {
            var employeesList = new List<Employee>
            {
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "Tom",
                    LastName = "Cat",
                    DateOfBirth = DateTime.Now.AddDays(-365),
                    PhoneNumber = "055-5555555",
                    Email = "Tom.Cat@gmail.com"
                },
                new Employee
                {
                    EmployeeId = 2,
                    FirstName = "Jerry",
                    LastName = "Mouse",
                    DateOfBirth = DateTime.Now.AddDays(-100),
                    PhoneNumber = "057-7777777",
                    Email = "Jerry.Mouse@gmail.com"
                },
                new Employee
                {
                    EmployeeId = 3,
                    FirstName = "Duck",
                    LastName = "Ducker",
                    DateOfBirth = DateTime.Now.AddDays(-50),
                    PhoneNumber = "058-8888888",
                    Email = "Duck.Ducker@gmail.com"
                }
            };
            await Db.Employees.AddRangeAsync(employeesList);
            await Db.SaveChangesAsync();
        }
    }
}
