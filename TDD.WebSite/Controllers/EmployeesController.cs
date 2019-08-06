using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TDD.Domain.Library.Entities;
using TDD.Repositories.Library.Core;

namespace TDD.WebSite.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmpoyeeRepository _empoyeeRepository;

        public EmployeesController(IEmpoyeeRepository empoyeeRepository)
        { 
            _empoyeeRepository = empoyeeRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _empoyeeRepository.GetEmployees());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _empoyeeRepository.GetEmployeeById((int)id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,DateOfBirth,PhoneNumber,Email")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _empoyeeRepository.AddEmployee(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _empoyeeRepository.GetEmployeeById((int)id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,DateOfBirth,PhoneNumber,Email")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _empoyeeRepository.UpdateEmployee(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_empoyeeRepository.EmployeeExists((int)employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _empoyeeRepository.GetEmployeeById((int)id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _empoyeeRepository.DeleteEmployee(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
