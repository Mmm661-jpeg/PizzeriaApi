using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.Interfaces
{
    public interface IDishesRepo
    {
        Task<IEnumerable<Dish>> GetAllDishesAsync();

        Task<IEnumerable<Dish>> GetDishesByCategoryIdAsync(int categoryId);

        Task<IEnumerable<Dish>> GetDishesByNameAsync(string dishName);

        Task<Dish> GetOneDishByIdAsync(int DishId);

        Task<Dish> GetOneDishByNameAsync(string dishName);

        Task<bool> AddDishAsync(Dish dish);

        Task<bool> UpdateDishAsync(Dish dish);

        Task<bool> DeleteDishAsync(Dish dish);

        //service: OrderByprice,OrderByDate,FilterOutPrice
    }
}
