using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzeriaApi.Data.DataModels;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.Repository
{
    public class DishIngredientsRepo:IDishIngredientsRepo
    {
        private readonly PizzeriaApiDBContext _dbContext;
        private readonly ILogger<DishIngredientsRepo> _logger;

        public DishIngredientsRepo(PizzeriaApiDBContext dbContext, ILogger<DishIngredientsRepo> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task<bool> AddDishIngredientAsync(DishIngredient dishIngredient)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddDishIngredientsAsync(IEnumerable<DishIngredient> ingredients)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> CalculateCostForDishAsync(int dishId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> CalculateEventuallIngredientCostAsync(int ingredientId, decimal quantity)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> CalculateIngredientCostForDishAsync(int ingredientId, int dishId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteIngredientAsync(int dishIngredient)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DishHasIngredientAsync(int dishId, int ingredientId)
        {
            throw new NotImplementedException();
        }

        public Task<int> EvaluateCurrentPriceForDishAsync(int dishId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Dish>> GetDishesByIngredientIdAsync(int ingredientId)
        {
            throw new NotImplementedException();
        }

        public Task<DishIngredient?> GetDishIngredientAsync(int dishId, int ingredientId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DishIngredient>> GetDishIngredientsAsyncWithDishId(int dishId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetIngredientQuantityForDishAsync(int dishId, int ingredientId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ingredient>> GetIngredientsByDIshIdAsync(int dishId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetRecommendedPriceForDishAsync(int dishId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDishIngredientAsync(DishIngredient dishIngredient)
        {
            throw new NotImplementedException();
        }
    }
}
