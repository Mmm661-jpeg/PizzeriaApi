using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.RequestModels.OrderReq
{
    public class UpdateOrderStatusReq
    {

        [Required(ErrorMessage = "OrderID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderID must be a positive integer.")]
        public int OrderID { get; set; }


        [Required(ErrorMessage = "OrderStatus is required.")]
        [Range(0, 4, ErrorMessage = "OrderStatus must be between 0 and 4.")]
        public int OrderStatus { get; set; }
    }
}
