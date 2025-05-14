using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.RequestModels.OrderItemReq
{
    public class UpdateOrderItemReq
    {
        [Required(ErrorMessage = "OrderItemId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderItemId must be a positive integer.")]
        public int OrderItemId { get; set; }


        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive integer.")]
        public int Quantity { get; set; }
    }
}
