using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quarterly_Sales_App.Models;
using Quarterly_Sales_App.Models.Data;
using System.Collections.Generic;
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
        public IActionResult Index(int? employeeId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var AsQuerylabel = _context.Employees.AsQueryable();
            List<Employee> fooList = new List<Employee>();
            if(employeeId != null && employeeId > 0)
            {
                fooList = AsQuerylabel.Where(x => x.ManagerId == employeeId.Value).Include(x=>x.Manager).ToList();
            }
            else
            {
                fooList = AsQuerylabel.Include(x => x.Manager).ToList();
            }
            var lstEmp = AsQuerylabel.ToList();
            foreach (var item in lstEmp)
            {
                var data = new SelectListItem()
                {
                    Text = item.FirstName + " " + item.LastName,
                    Value = item.EmployeeId.ToString()
                };
                list.Add(data);
            }
            var selectList = new SelectList(list, "Value", "Text");
            ViewBag.EmployeeId = selectList;
            var employees = fooList;
            return View(employees);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewBagData();
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            ViewBagData();
            if (ModelState.IsValid)
            {
                // Check if an employee with the same first name, last name, and date of birth exists
                var existingEmployee = _context.Employees.FirstOrDefault(e =>
                    e.FirstName == employee.FirstName &&
                    e.LastName == employee.LastName &&
                    e.DateOfBirth == employee.DateOfBirth);

                if (existingEmployee != null)
                {
                    ModelState.AddModelError("existingEmployee", "An employee with the same first name, last name, and date of birth already exists.");
                    return View(employee);
                }

                // Check if the manager exists and is not the same person as the employee
                if (employee.ManagerId.HasValue)
                {
                    var manager = _context.Employees.Find(employee.ManagerId.Value);
                    var fll = employee.FirstName + employee.LastName;
                    if(manager != null)
                    {
                        var ext = manager.FirstName + manager.LastName;
                        if (fll.ToLower().Equals(ext.ToLower()))
                        {
                            ModelState.AddModelError("managerInvalid", "Manager and Employee can't be the same person.");
                            return View(employee);
                        }
                    }
                }

                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }

        private void ViewBagData()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var fooList = _context.Employees.ToList();
            foreach (var item in fooList)
            {
                var data = new SelectListItem()
                {
                    Text = item.FirstName + " " + item.LastName,
                    Value = item.EmployeeId.ToString()
                };
                list.Add(data);
            }
            var selectList = new SelectList(list, "Value", "Text");
            ViewBag.Managers = selectList;
        }
    }
}
