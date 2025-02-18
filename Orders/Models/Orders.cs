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
        public OrderDeliveryStatus OrderDeliveryStatus { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }
        [Required]
        public int Invoice { get; set; }
        [Required]
        public required DateTime OrderDate { get; set; }

        public DateTime? ShippedDate { get; set; }
        [Required]
        [StringLength(100)]
        public required string Company {  get; set; }
        [Required]
        [StringLength (100)]
        public required string Store { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentTotal { get; set; }
    }
}
