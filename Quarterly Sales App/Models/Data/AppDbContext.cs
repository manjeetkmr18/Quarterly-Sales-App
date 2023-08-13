using Microsoft.EntityFrameworkCore;
using System;

namespace Quarterly_Sales_App.Models.Data
{
    public partial class AppDbContext : DbContext
    {
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<SalesData> SalesData { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    }
}
