using System.ComponentModel.DataAnnotations;

namespace Quarterly_Sales_App.Models
{
    public class SalesData
    {
        public int SalesDataId { get; set; }

        [Required(ErrorMessage = "Quarter is required.")]
        [Range(1, 4, ErrorMessage = "Quarter must be between 1 and 4.")]
        public int Quarter { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(2001, int.MaxValue, ErrorMessage = "Year must be after 2000.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
