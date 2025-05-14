using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.DTO_s
{
    public class DishDTO
    {

        public int Id { get; set; }
        public string Name { get; set; } 
        public decimal Price { get; set; }

        public string Description { get; set; }
        public int CategoryId { get; set; }

    }
}
