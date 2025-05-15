using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.RequestModels.IngredientReq
{
    public class AddIngredientReq
    {
        [Required]
        [StringLength(100,ErrorMessage = "Ingredient name cannot exceed 4 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
