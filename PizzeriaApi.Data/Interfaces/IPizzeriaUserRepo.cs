using Microsoft.AspNetCore.Identity;
using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.Interfaces
{
    public interface IPizzeriaUserRepo
    {
        Task<PizzeriaUser?> GetUserWithIdAsync(string userId);

        Task<PizzeriaUser?> GetUserByUsernameAsync(string username);

        Task<PizzeriaUser?> GetUserByEmailAsync(string email);



        Task<IEnumerable<PizzeriaUser>> GetPremiumUsers();

        Task<IEnumerable<PizzeriaUser>> GetRegularUsers();

        Task<IEnumerable<PizzeriaUser>> GetUsersWithOrders();

        Task<IEnumerable<PizzeriaUser>> GetUsersByorderStatus(string orderStatus);

        Task<int?> GetBonusByUserIdAsync(string userId);
        Task<bool> UpdateBonusAsync(string userId, int newBonusValue);

        Task<bool> UsercanUseBonus(string userId);

        Task<IEnumerable<PizzeriaUser>> GetAllUsers();

        Task<IEnumerable<PizzeriaUser>> GetUsersWithNoOrders();



    }
}
