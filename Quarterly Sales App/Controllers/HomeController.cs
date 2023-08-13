using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quarterly_Sales_App.Models;
using Quarterly_Sales_App.Models.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Quarterly_Sales_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
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
        }
        public IActionResult Index(int? employeeId)
        {
            viewData();
            var salesData = _context.SalesData.Include(s => s.Employee).AsQueryable();
            var lstSale = new List<SalesData>();
            if (employeeId.HasValue)
            {
                lstSale = salesData.Where(x => x.EmployeeId == employeeId).ToList();
            }
            else
            {
                lstSale = salesData.ToList();
            }
            return View(lstSale);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
