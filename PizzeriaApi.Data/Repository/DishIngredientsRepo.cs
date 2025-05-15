using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzeriaApi.Data.DataModels;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.UtilModels;

namespace PizzeriaApi.Data.Repository
{
    public class DishIngredientsRepo : IDishIngredientsRepo
    {
        private readonly PizzeriaApiDBContext _dbContext;
        private readonly ILogger<DishIngredientsRepo> _logger;
        private const decimal profitMargin = 1.2m; // 20% profit margin

        public DishIngredientsRepo(PizzeriaApiDBContext dbContext, ILogger<DishIngredientsRepo> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> AddDishIngredientAsync(DishIngredient dishIngredient)
        {
            if (dishIngredient == null)
            {
                _logger.LogWarning("AddDishIngredientAsync: dishIngredient is null.");
                return false;
            }

            try
            {
                var existingDishIngredient = await _dbContext.DishIngredients
                    .AnyAsync(di => di.DishId == dishIngredient.DishId && di.IngredientId == dishIngredient.IngredientId);

                if (existingDishIngredient)
                {
                    _logger.LogInformation("AddDishIngredientAsync: DishIngredient with DishId {DishId} and IngredientId {IngredientId} already exists.", dishIngredient.DishId, dishIngredient.IngredientId);
                    return false;
                }

                await _dbContext.DishIngredients.AddAsync(dishIngredient);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: AddDishIngredientAsync failed for DishId {DishId} and IngredientId {IngredientId}", dishIngredient.DishId, dishIngredient.IngredientId);
                return false;
            }
        }

        public async Task<bool> AddDishIngredientsAsync(IEnumerable<DishIngredient> ingredients)
        {
            if (ingredients == null || !ingredients.Any())
            {
                _logger.LogWarning("AddDishIngredientsAsync: ingredients is null or empty.");
                return false;
            }

            try
            {
                var ingredientsId = ingredients.Select(i => i.IngredientId).ToList();

                var dishId = ingredients.First().DishId;

                var existingDishIngredientsIds = await _dbContext.DishIngredients
                    .Where(di => di.DishId == dishId && ingredientsId.Contains(di.IngredientId))
                     .Select(di => di.IngredientId)
                    .ToListAsync();

                var newIngredients = ingredients
                   .Where(i => !existingDishIngredientsIds.Contains(i.IngredientId))
                   .ToList();

                if (!newIngredients.Any())
                {
                    _logger.LogInformation("AddDishIngredientsAsync: All ingredients already exist for DishId {DishId}", dishId);
                    return false;
                }

                await _dbContext.DishIngredients.AddRangeAsync(newIngredients);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: AddDishIngredientsAsync failed");
                return false;
            }
        }

        public async Task<decimal?> CalculateCostForDishAsync(int dishId)
        {
            if (dishId <= 0)
            {
                _logger.LogWarning("CalculateCostForDishAsync: Invalid DishId {DishId}", dishId);
                return null;
            }

            try
            {




                var ingredientCosts = await _dbContext.DishIngredients
                                    .Where(di => di.DishId == dishId)
                                     .SumAsync(di => di.Ingredient.Price * di.Quantity);

                return ingredientCosts;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: CalculateCostForDishAsync failed for DishId {DishId}", dishId);
                return null;
            }
        }

        public async Task<decimal?> CalculateEventuallIngredientCostAsync(int ingredientId, decimal quantity)
        {
            if (ingredientId <= 0 || quantity <= 0)
            {
                _logger.LogWarning("CalculateEventuallIngredientCostAsync: Invalid IngredientId {IngredientId} or Quantity {Quantity}", ingredientId, quantity);
                return null;
            }

            try
            {
                var ingredient = await _dbContext.Ingredients
                .Where(i => i.Id == ingredientId)
                .Select(i => new { i.Id, i.Price })
                .FirstOrDefaultAsync();

                if (ingredient == null)
                {
                    _logger.LogWarning("CalculateEventuallIngredientCostAsync: Ingredient with Id {IngredientId} does not exist", ingredientId);
                    return null;
                }

                return ingredient.Price * quantity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: CalculateEventuallIngredientCostAsync failed for IngredientId {IngredientId}", ingredientId);
                return null;
            }
        }

        public async Task<bool> DeleteIngredientAsync(int dishId, int ingredientId)
        {

            if (dishId <= 0 || ingredientId <= 0)
            {
                _logger.LogWarning("DeleteIngredientAsync: Invalid DishId {DishId} or IngredientId {IngredientId}", dishId, ingredientId);
                return false;
            }

            try
            {
                var dishIngredient = await _dbContext.DishIngredients
                                            .FirstOrDefaultAsync(di => di.DishId == dishId && di.IngredientId == ingredientId);

                if (dishIngredient == null)
                {
                    _logger.LogDebug("DeleteIngredientAsync: DishIngredient not found for DishId {DishId} and IngredientId {IngredientId}", dishId, ingredientId);
                    return false;
                }

                _dbContext.DishIngredients.Remove(dishIngredient);
                await _dbContext.SaveChangesAsync();

                return true;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: DeleteIngredientAsync failed for DishId {DishId} and IngredientId {IngredientId}", dishId, ingredientId);
                return false;
            }
        }

        public async Task<bool> DishHasIngredientAsync(int dishId, int ingredientId)
        {
            if (dishId <= 0 || ingredientId <= 0)
            {
                _logger.LogWarning("DishHasIngredientAsync: Invalid DishId {DishId} or IngredientId {IngredientId}", dishId, ingredientId);
                return false;
            }

            try
            {
                var result = await _dbContext.DishIngredients
                    .AnyAsync(di => di.DishId == dishId && di.IngredientId == ingredientId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: DishHasIngredientAsync failed for DishId {DishId} and IngredientId {IngredientId}", dishId, ingredientId);
                return false;
            }
        }

        public async Task<bool> DishHasIngredientByNameAsync(int dishId, string ingredientName)
        {
            if (string.IsNullOrEmpty(ingredientName) || dishId <= 0)
            {
                _logger.LogWarning("DishHasIngredientByNameAsync: Invalid DishId {DishId} or IngredientName {IngredientName}", dishId, ingredientName);
                return false;
            }

            try
            {
                var result = await _dbContext.DishIngredients
                    .AnyAsync(di => di.DishId == dishId && di.Ingredient.Name.Equals(ingredientName, StringComparison.OrdinalIgnoreCase));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: DishHasIngredientByNameAsync failed for DishId {DishId} and IngredientName {IngredientName}", dishId, ingredientName);
                return false;
            }
        }

        public async Task<UtilEnums.DishPriceEvaluation?> EvaluateCurrentPriceForDishAsync(int dishId)
        {
            if (dishId <= 0)
            {
                _logger.LogWarning("EvaluateCurrentPriceForDishAsync: Invalid DishId {DishId}", dishId);
                return null;
            }
            try
            {


                var dishPrice = await _dbContext.Dishes
                    .Where(d => d.Id == dishId)
                    .Select(d => d.Price)
                    .FirstOrDefaultAsync();

                if (dishPrice == 0)
                {
                    _logger.LogWarning("EvaluateCurrentPriceForDishAsync: Dish with Id {DishId} has no price", dishId);
                    return null;
                }

                var ingredientCosts = await _dbContext.DishIngredients
                                        .Where(di => di.DishId == dishId)
                                        .SumAsync(di => di.Ingredient.Price * di.Quantity);


                if (dishPrice < ingredientCosts)
                {
                    return UtilEnums.DishPriceEvaluation.TooLow;
                }
                else if (dishPrice > (ingredientCosts * profitMargin))
                {
                    return UtilEnums.DishPriceEvaluation.TooHigh;
                }
                else
                {
                    return UtilEnums.DishPriceEvaluation.JustRight;
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: EvaluateCurrentPriceForDishAsync failed for DishId {DishId}", dishId);
                return null;
            }
        }

        public async Task<IEnumerable<Dish>> GetDishesByIngredientIdAsync(int ingredientId)
        {
            if (ingredientId <= 0)
            {
                _logger.LogWarning("GetDishesByIngredientIdAsync: Invalid IngredientId {IngredientId}", ingredientId);
                return null;
            }

            try
            {
                var dishes = await _dbContext.DishIngredients
                    .Where(di => di.IngredientId == ingredientId)
                    .Select(di => di.Dish)
                    .ToListAsync();

                if (!dishes.Any())
                {
                    _logger.LogDebug("GetDishesByIngredientIdAsync: No dishes found for IngredientId {IngredientId}", ingredientId);
                }

                return dishes;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetDishesByIngredientIdAsync failed for IngredientId {IngredientId}", ingredientId);
                return null;
            }
        }

        public async Task<DishIngredient?> GetDishIngredientAsync(int dishId, int ingredientId)
        {
            if (dishId <= 0 || ingredientId <= 0)
            {
                _logger.LogWarning("GetDishIngredientAsync: Invalid DishId {DishId} or IngredientId {IngredientId}", dishId, ingredientId);
                return null;
            }

            try
            {
                var dishIngredient = await _dbContext.DishIngredients.FirstOrDefaultAsync(di => di.DishId == dishId && di.IngredientId == ingredientId);

                if (dishIngredient == null)
                {
                    _logger.LogDebug("GetDishIngredientAsync: No DishIngredient found for DishId {DishId} and IngredientId {IngredientId}", dishId, ingredientId);
                }

                return dishIngredient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetDishIngredientAsync failed for DishId {DishId} and IngredientId {IngredientId}", dishId, ingredientId);
                return null;
            }
        }

        public async Task<IEnumerable<DishIngredient>> GetDishIngredientsAsyncWithDishId(int dishId)
        {
            if (dishId <= 0)
            {
                _logger.LogWarning("GetDishIngredientsAsyncWithDishId: Invalid DishId {DishId}", dishId);
                return null;
            }

            try
            {
                var dishIngredients = await _dbContext.DishIngredients
                    .Where(di => di.DishId == dishId)
                    .ToListAsync();

                if (!dishIngredients.Any())
                {
                    _logger.LogDebug("GetDishIngredientsAsyncWithDishId: No DishIngredients found for DishId {DishId}", dishId);
                }

                return dishIngredients;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetDishIngredientsAsyncWithDishId failed for DishId {DishId}", dishId);
                return null;
            }
        }

        public async Task<decimal?> GetIngredientQuantityForDishAsync(int dishId, int ingredientId)
        {
            if (dishId <= 0 || ingredientId <= 0)
            {
                _logger.LogWarning("GetIngredientQuantityForDishAsync: Invalid DishId {DishId} or IngredientId {IngredientId}", dishId, ingredientId);
                return null;
            }
            try
            {
                var quantity = await _dbContext.DishIngredients
                    .Where(di => di.DishId == dishId && di.IngredientId == ingredientId)
                    .Select(di => di.Quantity)
                    .FirstOrDefaultAsync();

              
                return quantity;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetIngredientQuantityForDishAsync failed for DishId {DishId} and IngredientId {IngredientId}", dishId, ingredientId);
                return null;
            }
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsByDIshIdAsync(int dishId)
        {
            if (dishId <= 0)
            {
                _logger.LogWarning("GetIngredientsByDIshIdAsync: Invalid DishId {DishId}", dishId);
                return null;
            }

            try
            {
                var ingredients = await _dbContext.DishIngredients
                    .Where(di => di.DishId == dishId)
                    .Select(di => di.Ingredient)
                    .ToListAsync();

                if (!ingredients.Any())
                {
                    _logger.LogDebug("GetIngredientsByDIshIdAsync: No ingredients found for DishId {DishId}", dishId);
                }

                return ingredients;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetIngredientsByDIshIdAsync failed for DishId {DishId}", dishId);
                return null;
            }
        }

        public async Task<decimal?> GetRecommendedPriceForDishAsync(int dishId)
        {
            if (dishId <= 0)
            {
                _logger.LogWarning("GetRecommendedPriceForDishAsync: Invalid DishId {DishId}", dishId);
                return null;
            }

            try
            {
                var dishPrice = await _dbContext.Dishes
                    .Where(d => d.Id == dishId)
                    .Select(d => d.Price)
                    .FirstOrDefaultAsync();

                var ingredientCosts = await _dbContext.DishIngredients
                                       .Where(di => di.DishId == dishId)
                                       .SumAsync(di => di.Ingredient.Price * di.Quantity);

                if (ingredientCosts == 0 || dishPrice == 0)
                {
                    _logger.LogWarning("GetRecommendedPriceForDishAsync: Dish with Id {DishId} has no price or ingredient costs", dishId);
                    return null;
                }

                var recommendedPrice = ingredientCosts * profitMargin;

                return Math.Max(recommendedPrice, dishPrice);


            }


            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetRecommendedPriceForDishAsync failed for DishId {DishId}", dishId);
                return null;
            }
        }

        public async Task<bool> UpdateDishIngredientAsync(DishIngredient dishIngredient)
        {
            if (dishIngredient == null || dishIngredient.DishId <= 0 || dishIngredient.IngredientId <= 0)
            {
                _logger.LogWarning("UpdateDishIngredientAsync: Invalid input.");
                return false;
            }

            try
            {
                var existing = await _dbContext.DishIngredients
                    .FirstOrDefaultAsync(di => di.DishId == dishIngredient.DishId && di.IngredientId == dishIngredient.IngredientId);

                if (existing == null)
                {
                    _logger.LogWarning("UpdateDishIngredientAsync: DishIngredient not found for DishId {DishId} and IngredientId {IngredientId}",
                        dishIngredient.DishId, dishIngredient.IngredientId);
                    return false;
                }

                existing.Quantity = dishIngredient.Quantity;
                existing.Unit = dishIngredient.Unit ?? existing.Unit;

                await _dbContext.SaveChangesAsync();
                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: UpdateDishIngredientAsync failed for DishId {DishId} and IngredientId {IngredientId}",
                    dishIngredient.DishId, dishIngredient.IngredientId);
                return false;
            }
        }
    }
}
