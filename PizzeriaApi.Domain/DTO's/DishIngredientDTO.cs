using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.DTO_s
{
    public class DishIngredientDTO
    {
        public int DishId { get; set; }
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public string IngredientUnit { get; set; }

        public Unit Unit { get; set; }
    }
}
