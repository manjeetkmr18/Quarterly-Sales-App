using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quarterly_Sales_App.Models;
using Quarterly_Sales_App.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

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
        public async Task<IActionResult> Index(int? employeeId, int? year, int? quater, int? page)
        {
            int pageSize = 3;
            page = page.HasValue ? page : 1;
            viewData();
            var salesData = _context.SalesData.Include(s => s.Employee).AsQueryable();
            Expression<Func<SalesData, bool>> predicate = null;
            if (employeeId.HasValue)
            {
                predicate = predicate != null ? predicate.And(x => x.EmployeeId == employeeId) : (x => x.EmployeeId == employeeId);
            }
            if (year.HasValue)
            {
                predicate = predicate != null ? predicate.And(x => x.Year == year) : (x => x.Year == year);
            }
            if (quater.HasValue)
            {
                predicate = predicate != null ? predicate.And(x => x.Quarter == quater) : (x => x.Quarter == quater);
            }
            if (predicate != null)
            {
                salesData = salesData.Where(predicate);
            }
            //var pag = await PagingList<SalesData>.CreateAsync(salesData.AsNoTracking(), pageNumber ?? 1, pageSize);
            var pag = salesData.ToPagedList(page ?? 1, pageSize);
            return View(pag);
        }
        private void viewData()
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
            ViewBag.EmployeeId = selectList;
            List<SelectListItem> listYear = new List<SelectListItem>();
            int startY = 2001;
            for (int i = 0; i < 50; i++)
            {
                var data = new SelectListItem()
                {
                    Text = (2001 + i).ToString(),
                    Value = (2001 + i).ToString()
                };
                listYear.Add(data);
            }
            var selectYear = new SelectList(listYear, "Value", "Text");
            ViewBag.year = selectYear;

        }
        // GET: SalesData/Create
        public IActionResult Create()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var AsQuerylabel = _context.Employees.AsQueryable();
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
            ViewBag.Employees = selectList;
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
