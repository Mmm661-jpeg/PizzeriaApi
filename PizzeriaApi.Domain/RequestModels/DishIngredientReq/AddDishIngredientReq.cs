using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.RequestModels.DishIngredientReq
{
    public class AddDishIngredientReq
    {
        [Required(ErrorMessage = "Dish ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Dish ID must be a positive integer.")]
        public int DishId { get; set; }

        [Required(ErrorMessage = "Ingredient ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Ingredient ID must be a positive integer.")]
        public int IngredientId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Ingredient unit is required.")]
        [StringLength(4, ErrorMessage = "Ingredient unit cannot exceed 4 characters.")]
        public string IngredientUnit { get; set; }
    }
}
