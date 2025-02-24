using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
        public required string ProductName { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Unit Price cannot be negative.")]
        public decimal UnitPrice { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "No. of Units cannot be negative.")]
        public int NoOfUnits { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Discount cannot be negative.")]
        public decimal Discount { get; set; }

        public int OrderId { get; set; }
    }
}
