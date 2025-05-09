using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.Models
{
    public class Category
    {
        [Key]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
