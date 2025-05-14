using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.RequestModels.OrderReq
{
    public class SetOrderPaidReq
    {

        [Required(ErrorMessage = "OrderId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderId must be a positive integer.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        [StringLength(450, ErrorMessage = "UserId must be less than 450 characters.")]

        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "AmountPaid is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "AmountPaid must be a non-negative number.")]

        public decimal AmountPaid { get; set; }
        public bool UseBonus { get; set; } = false;
    }
}
