using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.RequestModels.OrderReq
{
    public class CancelOrderReq
    {
        [Required(ErrorMessage = "Order ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Order ID must be a positive integer.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        [StringLength(450, ErrorMessage = "User ID cannot exceed 450 characters.")]
        public string UserId { get; set; }

        [StringLength(100, ErrorMessage = "Reason cannot exceed 100 characters.")]
        public string Reason { get; set; } = "No reason provided";
    }
}
