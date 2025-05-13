using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Domain.UtilModels
{
    public static class UtilEnums
    {
        public enum DishPriceEvaluation
        {
            TooLow = 1,
            JustRight = 2,
            TooHigh = 3
        }

        public enum UserRoles //update too this in repos also;
        {
            RegularUser = 1,
            PremiumUser = 2
        }
    }

   
}
