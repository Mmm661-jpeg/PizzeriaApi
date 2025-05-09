using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }


        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        [Required]
        public int DishId { get; set; }

        [ForeignKey("DishId")]
        public Dish Dish { get; set; } = null!;


        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
