using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Core.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(PizzeriaUser pizzeriaUser);
    }
}
