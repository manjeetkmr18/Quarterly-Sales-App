using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quarterly_Sales_App.Models;
using Quarterly_Sales_App.Models.Data;
using System.Linq;

namespace Quarterly_Sales_App.Controllers
{
    public class EmployeesController : Controller
    {

        private readonly Models.Data.AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public IActionResult Index()
        {
            var employees = _context.Employees.Include(e => e.Manager).ToList();
            return View(employees);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Check if an employee with the same first name, last name, and date of birth exists
                var existingEmployee = _context.Employees.FirstOrDefault(e =>
                    e.FirstName == employee.FirstName &&
                    e.LastName == employee.LastName &&
                    e.DateOfBirth == employee.DateOfBirth);

                if (existingEmployee != null)
                {
                    ModelState.AddModelError("", "An employee with the same first name, last name, and date of birth already exists.");
                    return View(employee);
                }

                // Check if the manager exists and is not the same person as the employee
                if (employee.ManagerId.HasValue)
                {
                    var manager = _context.Employees.Find(employee.ManagerId.Value);
                    if (manager == null)
                    {
                        ModelState.AddModelError("", "Invalid manager selection.");
                        return View(employee);
                    }
                }

                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }


    }
}
