using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.Models
{
    public class Ingredient
    {
        [Key]
        [Range(1,int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]  
        [Range(0, double.MaxValue)]                   
        public decimal Price { get; set; }

        public ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
    }
}
