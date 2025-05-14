using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.DTO_s
{
    public class OrderDTO
    {

        public int Id { get; set; }


     
        public string UserId { get; set; } 

    
       

        public DateTime CreatedAt { get; set; } 

        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }

        public bool UsedBonusReward { get; set; }

        public DateTime? CancelledAt { get; set; }

        public string? CancellationReason { get; set; }
        public DateTime? FinalizedAt { get; set; }
    }
}
