using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDD.Domain.Library.Entities;
using TDD.Repositories.Library.Core;
using TDD.WebSite.Controllers;
using Xunit;

namespace TDD.WebSite.Tests
{
    public class EmployeesControllerTests
    {
        [Fact]
        public async void Index_GetEmployees_ReturnsIndexView()
        {
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            employeeRepositroy.Setup(mock => mock.GetEmployees());
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            var result = await employeeController.Index() as ViewResult;

            Assert.Equal("Index", result.ViewName);
            employeeRepositroy.Verify(mock => mock.GetEmployees(), Times.Once());
            
            employeeController.Dispose();
        }

        [Fact]
        public async void Details_IdIsNull_ReturnsNotFound()
        {
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            var result = await employeeController.Details(null) as NotFoundResult;
  
            Assert.NotNull(result);

            employeeController.Dispose();
        }

        [Fact]
        public async void Details_GetEmployeeById_EmployeeFound_ReturnsViewDetailsModelEmployee()
        {
            // arange
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            employeeRepositroy.Setup(mock => mock.GetEmployeeById(1)).ReturnsAsync(new Employee());
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            // act
            var result = await employeeController.Details(1) as ViewResult;

            // assert
            Assert.Equal("Details", result.ViewName);
            employeeRepositroy.Verify(mock => mock.GetEmployeeById(1), Times.Once());
            Assert.NotNull(result);

            employeeController.Dispose();
        }

        [Fact]
        public async void Details_GetEmployeeById_ResusltIsNull_ReturnsNotFound()
        {
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            employeeRepositroy.Setup(mock => mock.GetEmployeeById(1)).Returns(Task.FromResult<Employee>(null));
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            var result = await employeeController.Details(1) as NotFoundResult;

            employeeRepositroy.Verify(mock => mock.GetEmployeeById(1), Times.Once());
            Assert.NotNull(result);

            employeeController.Dispose();
        }

        [Fact]
        public void Create_HttpGet_ReturnsCreateView()
        {
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            var result = employeeController.Create() as ViewResult;

            Assert.Equal("Create", result.ViewName);

            employeeController.Dispose();
        }

        [Fact]
        public async void Create_HttpPost_ModelStateNotValid_ReturnsCreateView()
        {
            // arange
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            var employeeController = new EmployeesController(employeeRepositroy.Object);
            employeeController.ModelState.AddModelError("test", "test");
            var employee = new Employee();

            // act
            var result = await employeeController.Create(employee) as ViewResult;

            // assert
            Assert.False(employeeController.ModelState.IsValid);
            Assert.Equal("Create", result.ViewName);
            Assert.Same(employee, result.Model as Employee);

            employeeController.Dispose();
        }

        [Fact]
        public async void Create_HttpPost_AddEmployee_RedirectToIndexAction()
        {
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            var employee = new Employee();
            employeeRepositroy.Setup(mock => mock.AddEmployee(employee));
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            var result = await employeeController.Create(employee);

            employeeRepositroy.Verify(mock => mock.AddEmployee(employee), Times.Once());
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            employeeController.Dispose();
        }

        [Fact]
        public async void Edit_IdIsNull_ReturnsNotFound()
        {
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            var result = await employeeController.Edit(null) as NotFoundResult;

            Assert.NotNull(result);

            employeeController.Dispose();
        }

        [Fact]
        public async void Edit_GetEmployeeById_EmployeeFound_ReturnsViewEdit_ModelEmployee()
        {
            // arange
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            employeeRepositroy.Setup(mock => mock.GetEmployeeById(1)).ReturnsAsync(new Employee());
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            // act
            var result = await employeeController.Edit(1) as ViewResult;

            // assert
            Assert.Equal("Edit", result.ViewName);
            employeeRepositroy.Verify(mock => mock.GetEmployeeById(1), Times.Once());
            Assert.NotNull(result);

            employeeController.Dispose();
        }

        [Fact]
        public async void Edit_GetEmployeeById_ResusltIsNull_ReturnsNotFound()
        {
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            employeeRepositroy.Setup(mock => mock.GetEmployeeById(1)).Returns(Task.FromResult<Employee>(null));
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            var result = await employeeController.Edit(1) as NotFoundResult;

            employeeRepositroy.Verify(mock => mock.GetEmployeeById(1), Times.Once());
            Assert.NotNull(result);

            employeeController.Dispose();
        }

        [Fact]
        public async void Edit_HttpPost_IdNotEqualToEmployeeId_ReturnsNotFound()
        {
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            var employeeController = new EmployeesController(employeeRepositroy.Object);
            var id = 1;
            var employee = new Employee { EmployeeId = 2 };

            var result = await employeeController.Edit(id, employee) as NotFoundResult;

            Assert.NotNull(result);

            employeeController.Dispose();
        }

        [Fact]
        public async void Edit_HttpPost_ModelStateNotValid_ReturnsEditView()
        {
            // Arange
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            var employeeController = new EmployeesController(employeeRepositroy.Object);
            var id = 1;
            var employee = new Employee { EmployeeId = 1 };
            employeeController.ModelState.AddModelError("test", "test");

            // Act
            var result = await employeeController.Edit(id, employee) as ViewResult;

            // Assert
            Assert.False(employeeController.ModelState.IsValid);
            Assert.Equal("Edit", result.ViewName);
            Assert.Same(employee, result.Model as Employee);

            employeeController.Dispose();
        }


        [Fact]
        public async void Edit_HttpPost_DbUpdateConcurrencyException_EmployeeNotExist_ReturnsNotFound()
        {
            // Arange
            var id = 1;
            var employee = new Employee { EmployeeId = 1 };

            var employeeRepositroy = new Mock<IEmpoyeeRepository>();

            employeeRepositroy.Setup(mock => 
            mock.UpdateEmployee(employee)).Throws(
                new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() }));

            employeeRepositroy.Setup(mock => mock.EmployeeExists(id)).Returns(false);

            var employeeController = new EmployeesController(employeeRepositroy.Object);
            
            // Act
            var result = await employeeController.Edit(id, employee) as NotFoundResult;

            // Assert
            employeeRepositroy.Verify(mock => mock.UpdateEmployee(employee), Times.Once());
            employeeRepositroy.Verify(mock => mock.EmployeeExists(id), Times.Once());
            
            Assert.NotNull(result);

            employeeController.Dispose();
        }

        [Fact]
        public async void Edit_HttpPost_DbUpdateConcurrencyException_EmployeeExist_ReturnsThrow()
        {
            // Arange
            var id = 1;
            var employee = new Employee { EmployeeId = 1 };

            var employeeRepositroy = new Mock<IEmpoyeeRepository>();

            employeeRepositroy.Setup(mock =>
            mock.UpdateEmployee(employee)).Throws(
                new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() }));

            employeeRepositroy.Setup(mock => mock.EmployeeExists(id)).Returns(true);

            var employeeController = new EmployeesController(employeeRepositroy.Object);

            // Act & Assert
            await Assert.ThrowsAnyAsync<Exception>(() => employeeController.Edit(id, employee));
            employeeRepositroy.Verify(mock => mock.UpdateEmployee(employee), Times.Once());
            employeeRepositroy.Verify(mock => mock.EmployeeExists(id), Times.Once());

            employeeController.Dispose();
        }

        [Fact]
        public async void Edit_HttpPost_UpdateEmployee_RedirectToIndexAction()
        {
            var id = 1;
            var employee = new Employee { EmployeeId = 1 };

            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            employeeRepositroy.Setup(mock => mock.UpdateEmployee(employee));
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            var result = await employeeController.Edit(id, employee);

            employeeRepositroy.Verify(mock => mock.UpdateEmployee(employee), Times.Once());
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            employeeController.Dispose();
        }

        [Fact]
        public async void Delete_IdIsNull_ReturnsNotFound()
        {
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            var result = await employeeController.Delete(null) as NotFoundResult;

            Assert.NotNull(result);

            employeeController.Dispose();
        }

        [Fact]
        public async void Delete_GetEmployeeById_ResusltIsNull_ReturnsNotFound()
        {
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            employeeRepositroy.Setup(mock => mock.GetEmployeeById(1)).Returns(Task.FromResult<Employee>(null));
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            var result = await employeeController.Delete(1) as NotFoundResult;

            employeeRepositroy.Verify(mock => mock.GetEmployeeById(1), Times.Once());
            Assert.NotNull(result);

            employeeController.Dispose();
        }

        [Fact]
        public async void Delete_GetEmployeeById_EmployeeFound_ReturnsDeleteView()
        {
            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            employeeRepositroy.Setup(mock => mock.GetEmployeeById(1)).Returns(Task.FromResult(new Employee()));
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            var result = await employeeController.Delete(1) as ViewResult;

            employeeRepositroy.Verify(mock => mock.GetEmployeeById(1), Times.Once());
            Assert.NotNull(result);
            Assert.Equal("Delete", result.ViewName);

            employeeController.Dispose();
        }

        [Fact]
        public async void Delete_HttpPost_EmployeeEmployee_RedirectToIndexAction()
        {
            var id = 1;

            var employeeRepositroy = new Mock<IEmpoyeeRepository>();
            employeeRepositroy.Setup(mock => mock.DeleteEmployee(id));
            var employeeController = new EmployeesController(employeeRepositroy.Object);

            var result = await employeeController.DeleteConfirmed(id);

            employeeRepositroy.Verify(mock => mock.DeleteEmployee(id), Times.Once());
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            employeeController.Dispose();
        }
    }
}
