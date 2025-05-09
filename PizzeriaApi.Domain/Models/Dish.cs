using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.Models
{
    public class Dish
    {
        [Key]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0,double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(100)]
        public string? Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!;

        public ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
