using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.Models
{
    public class DishIngredient
    {
     
        public int DishId { get; set; }

        [ForeignKey("DishId")]
        public Dish Dish { get; set; } = null!;

        public int IngredientId { get; set; }

        [ForeignKey("IngredientId")]
        public Ingredient Ingredient { get; set; } = null!;

        public decimal Quantity { get; set; } 
        public Unit Unit { get; set; } = Unit.Gram;
    }

    public enum Unit
    {
        Gram = 1,
        Kilo = 2
    }
}
