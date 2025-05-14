using System.ComponentModel.DataAnnotations;

namespace PizzeriaApi.Domain.RequestModels.DishIngredientReq
{
    public class UpdateDishIngredientReq
    {
        [Required(ErrorMessage = "Dish ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Dish ID must be a positive integer.")]
        public int DishId { get; set; }
        [Required(ErrorMessage = "Ingredient ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Ingredient ID must be a positive integer.")]
        public int IngredientId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public decimal Quantity { get; set; }

        [StringLength(4, ErrorMessage = "Ingredient unit cannot exceed 4 characters.")]

        public string IngredientUnit { get; set; }
    }
}
