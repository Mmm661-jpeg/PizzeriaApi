using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels;
using PizzeriaApi.Domain.UtilModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Core.Interfaces
{
    public interface IPizzeriaUserService
    {
        Task<OperationResult<string?>> RegisterUserAsync(RegisterUserReq registerUserReq);

        Task<OperationResult<string?>> LoginAsync(LoginReq loginReq);

        Task<OperationResult<string?>> UpdateUserAsync(UpdateUserReq updateUserReq);

        Task<OperationResult<string?>> DeleteUserAsync(string userId);




        Task<OperationResult<PizzeriaUserDTO?>> GetUserWithIdAsync(string userId);

        Task<OperationResult<PizzeriaUserDTO?>> GetUserByUsernameAsync(string username);

        Task<OperationResult<PizzeriaUserDTO?>> GetUserByEmailAsync(string email);



        Task<OperationResult<IEnumerable<PizzeriaUserDTO>>> GetPremiumUsers();

        Task<OperationResult<IEnumerable<PizzeriaUserDTO>>> GetRegularUsers();

        Task<OperationResult<IEnumerable<PizzeriaUserDTO>>> GetUsersWithOrders();

        Task<OperationResult<IEnumerable<PizzeriaUserDTO>>> GetUsersByorderStatus(string orderStatus);

        Task<OperationResult<int?>> GetBonusByUserIdAsync(string userId);
        Task<OperationResult<bool?>> UpdateBonusAsync(string userId, int newBonusValue);

        Task<OperationResult<bool?>> UsercanUseBonus(string userId);

        Task<OperationResult<IEnumerable<PizzeriaUserDTO>>> GetAllUsers();

        Task<OperationResult<IEnumerable<PizzeriaUserDTO>>> GetUsersWithNoOrders();


        Task<OperationResult<bool?>> UpdateUserRoleAsync(string userId, string role);

    }
}
