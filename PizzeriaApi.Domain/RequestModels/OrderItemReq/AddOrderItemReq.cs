using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.RequestModels.OrderItemReq
{
    public class AddOrderItemReq
    {
        [Required(ErrorMessage ="OrderId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderId must be a positive integer.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "DishId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "DishId must be a positive integer.")]
        public int DishId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive integer.")]
        public int Quantity { get; set; }

    }
}
