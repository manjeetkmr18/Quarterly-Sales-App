using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quarterly_Sales_App.Models;
using Quarterly_Sales_App.Models.Data;
using System.Linq;

namespace Quarterly_Sales_App.Controllers
{
    public class SalesDataController : Controller
    {
        private readonly AppDbContext _context;
        public SalesDataController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SalesData
        public IActionResult Index(int? employeeId)
        {
            var salesData = _context.SalesData.Include(s => s.Employee).AsQueryable();

            if (employeeId.HasValue)
            {
                salesData = salesData.Where(s => s.EmployeeId == employeeId);
            }

            ViewBag.EmployeeId = new SelectList(_context.Employees, "EmployeeId", "FullName");
            return View(salesData.ToList());
        }

        // GET: SalesData/Create
        public IActionResult Create()
        {
            ViewBag.Employees = _context.Employees.ToList();
            return View();
        }

        // POST: SalesData/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SalesData salesData)
        {
            if (ModelState.IsValid)
            {
                // Check if the employee exists
                var employee = _context.Employees.Find(salesData.EmployeeId);
                if (employee == null)
                {
                    ModelState.AddModelError("", "Invalid employee selection.");
                    ViewBag.Employees = _context.Employees.ToList();
                    return View(salesData);
                }

                // Check if the sales data already exists
                var existingSalesData = _context.SalesData.FirstOrDefault(s =>
                    s.Quarter == salesData.Quarter &&
                    s.Year == salesData.Year &&
                    s.EmployeeId == salesData.EmployeeId);

                if (existingSalesData != null)
                {
                    ModelState.AddModelError("", "Sales data for the selected employee, quarter, and year already exists.");
                    ViewBag.Employees = _context.Employees.ToList();
                    return View(salesData);
                }

                _context.SalesData.Add(salesData);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Employees = _context.Employees.ToList();
            return View(salesData);
        }
        // public IActionResult Index()
        // {
        //      return View();
        //  }
    }
}
