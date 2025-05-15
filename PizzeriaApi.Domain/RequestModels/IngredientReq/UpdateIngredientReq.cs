using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.RequestModels.IngredientReq
{
    public class UpdateIngredientReq
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Ingredient ID must be a positive integer.")]
        public int IngredientId { get; set; }

        [StringLength(100, ErrorMessage = "Ingredient name cannot exceed 4 characters.")]
        public string Name { get; set; }

        [Range(0, double.MaxValue,ErrorMessage = "Price must be greater than zero. ")]

        public decimal? Price { get; set; }
    }
}
