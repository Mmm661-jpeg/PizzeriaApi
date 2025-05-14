using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.DTO_s
{
    public class IngredientDTO
    {
        public int Id { get; set; }

       
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
