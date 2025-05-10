using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.Interfaces
{
    public interface IDishIngredientsRepo
    {
        Task<bool> AddDishIngredientAsync(DishIngredient dishIngredient);

        Task<bool> AddDishIngredientsAsync(IEnumerable<DishIngredient> ingredients);
        Task<bool> DeleteIngredientAsync(int dishIngredient);

        Task<bool> UpdateDishIngredientAsync(DishIngredient dishIngredient);

        Task<DishIngredient?> GetDishIngredientAsync(int dishId, int ingredientId);

        Task<IEnumerable<DishIngredient>> GetDishIngredientsAsyncWithDishId(int dishId);

        Task<IEnumerable<Dish>> GetDishesByIngredientIdAsync(int ingredientId);

        Task<IEnumerable<Ingredient>> GetIngredientsByDIshIdAsync(int dishId);

        Task<decimal> GetIngredientQuantityForDishAsync(int dishId, int ingredientId);

        Task<decimal> CalculateEventuallIngredientCostAsync(int ingredientId, decimal quantity);

        Task<decimal> CalculateIngredientCostForDishAsync(int ingredientId, int dishId);

        Task<decimal> CalculateCostForDishAsync(int dishId);

        Task<decimal> GetRecommendedPriceForDishAsync(int dishId);

        Task<int> EvaluateCurrentPriceForDishAsync(int dishId);

        Task<bool> DishHasIngredientAsync(int dishId, int ingredientId);
    }
}
