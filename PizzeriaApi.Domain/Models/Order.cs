using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.Models
{
    public class Order
    {
        [Key]
        [Range(0,int.MaxValue)]
        public int Id { get; set; }


        [Required]
        public string UserId { get; set; } = null!;
        public PizzeriaUser? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0,double.MaxValue)]
        public decimal TotalPrice { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending; 

        public bool UsedBonusReward { get; set; } = false;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    public enum OrderStatus
    {
        Pending = 1,
        Paid = 2,
        Deliver = 3,
        Cancelled = 4,
    }
}
