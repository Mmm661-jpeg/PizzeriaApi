using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.RequestModels.DishReq
{
    public class UpdateDishReq
    {
        public int DishId { get; set; }

        //[Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Dish name cannot exceed 50 characters.")]
        public string? Name { get; set; } = null!;

        //[Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Dish price cannot be less than 0.")]
        public decimal? Price { get; set; }


        [StringLength(100, ErrorMessage = "Dish description cannot exceed 100 characters.")]
        public string? Description { get; set; }


        //[Required(ErrorMessage = "Category id is required")]
        public int? CategoryId { get; set; }
    }
}
