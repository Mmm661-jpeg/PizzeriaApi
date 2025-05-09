using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.Models
{
    public class PizzeriaUser : IdentityUser
    {
        public int BonusPoints { get; set; } = 0;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
