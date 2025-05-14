using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels.DishIngredientReq;
using PizzeriaApi.Domain.UtilModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Core.Interfaces
{
    public interface IDishIngredientsService
    {
        Task<OperationResult<bool?>> AddDishIngredientAsync(AddDishIngredientReq dishIngredientReq);

        Task<OperationResult<bool?>> AddDishIngredientsAsync(IEnumerable<AddDishIngredientReq> dishIngredientReq);
        Task<OperationResult<bool?>> DeleteIngredientAsync(int dishId, int ingredientId);

        Task<OperationResult<bool?>> UpdateDishIngredientAsync(UpdateDishIngredientReq req);

        Task<OperationResult<DishIngredientDTO?>> GetDishIngredientAsync(int dishId, int ingredientId);

        Task<OperationResult<IEnumerable<DishIngredientDTO>>> GetDishIngredientsAsyncWithDishId(int dishId);

        Task<OperationResult<IEnumerable<DishDTO>>> GetDishesByIngredientIdAsync(int ingredientId);

        Task<OperationResult<IEnumerable<IngredientDTO>>> GetIngredientsByDishIdAsync(int dishId);

        Task<OperationResult<decimal?>> GetIngredientQuantityForDishAsync(int dishId, int ingredientId);

        Task<OperationResult<decimal?>> CalculateEventuallIngredientCostAsync(int ingredientId, decimal quantity);


        Task<OperationResult<decimal?>> CalculateCostForDishAsync(int dishId);

        Task<OperationResult<decimal?>> GetRecommendedPriceForDishAsync(int dishId);

        Task<OperationResult<string?>> EvaluateCurrentPriceForDishAsync(int dishId);

        Task<OperationResult<bool?>> DishHasIngredientAsync(int dishId, int ingredientId);

         Task<OperationResult<bool?>> DishHasIngredientByNameAsync(int dishId, string ingredientName);
    }
}
