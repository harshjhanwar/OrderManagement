using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Models
{
    public enum OrderDeliveryStatus { 
        Deliverd,
        NotDelivered,
        Returned
    }

    public enum OrderStatus { 
        Shipped,
        InTransit,
        Reached
    }

    public class Orders
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [EnumDataType(typeof(OrderDeliveryStatus), ErrorMessage = "Invalid Order Delivery Status.")]
        public OrderDeliveryStatus OrderDeliveryStatus { get; set; }
        [Required]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid Order Status.")]
        public OrderStatus OrderStatus { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Invoice number must be a positive integer.")]
        public int Invoice { get; set; }
        [Required]
        public required DateTime OrderDate { get; set; }
        [Required]
        public DateTime? ShippedDate { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
        public required string Company {  get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Store name cannot exceed 100 characters.")]
        public required string Store { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Order Total cannot be negative.")]
        public decimal OrderTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Payment Total cannot be negative.")]
        public decimal PaymentTotal { get; set; }
    }

}
