using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels.DishReq;
using PizzeriaApi.Domain.UtilModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Core.Interfaces
{
    public interface IDishesService
    {
        Task<OperationResult<IEnumerable<DishDTO>>> GetAllDishesAsync(int filter = 3); //enum: 1,2,3: 1:LowPrice, 2:Highprice, 3:NoOrder

        Task<OperationResult<IEnumerable<DishDTO>>> GetDishesByCategoryIdAsync(int categoryId);

        Task<OperationResult<IEnumerable<DishDTO>>> GetDishesByNameAsync(string dishName);

        Task<OperationResult<DishDTO?>> GetOneDishByIdAsync(int dishId);

        Task<OperationResult<DishDTO?>> GetOneDishByNameAsync(string dishName);

        Task<OperationResult<bool?>> AddDishAsync(AddDishReq dishReq);

        Task<OperationResult<bool?>> UpdateDishAsync(UpdateDishReq dishReq);

        Task<OperationResult<bool?>> DeleteDishAsync(int dishId);

        
    }
}
