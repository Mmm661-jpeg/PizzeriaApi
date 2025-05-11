using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using PizzeriaApi.Data.DataModels;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.Repository
{
    public class DishesRepo:IDishesRepo
    {
        private readonly PizzeriaApiDBContext _dbContext;
        private readonly ILogger<DishesRepo> _logger;   

        public DishesRepo(PizzeriaApiDBContext dbContext, ILogger<DishesRepo> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> AddDishAsync(Dish dish)
        {
            if(dish == null)
            {
                _logger.LogWarning("AddDishAsync: Dish is null");
                return false;
            }

            var dishExist = await _dbContext.Dishes.AnyAsync(d => d.Name == dish.Name);

            if(dishExist)
            {
                _logger.LogInformation("AddDishAsync: Dish {DishName} already exists.", dish.Name);
                return false;
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var incomingIngredientsIds = dish.DishIngredients.Select(i => i.IngredientId).Distinct();
                var existingIngredients = await _dbContext.Ingredients.Where(i => incomingIngredientsIds.Contains(i.Id)).ToListAsync();

                if (existingIngredients.Count != incomingIngredientsIds.Count())
                {
                    _logger.LogWarning("AddDishAsync: Some ingredients do not exist in the database.");
                    await transaction.RollbackAsync();
                    return false;
                }

                var quantities = dish.DishIngredients.Select(i => i.Quantity);

                if(quantities.Any(q => q <= 0))
                {
                    _logger.LogWarning("AddDishAsync: Some quantities are invalid.");
                    await transaction.RollbackAsync();
                    return false;
                }

                await _dbContext.AddAsync(dish);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error: AddDishAsync failed");
                return false;
            }
        }

        public Task<bool> DeleteDishAsync(Dish dish)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Dish>> GetAllDishesAsync()
        {
            try
            {
                var dishes = await _dbContext.Dishes.ToListAsync();

                if(!dishes.Any())
                {
                    _logger.LogDebug("GetAllDishesAsync: No dishes found!");
                    return null;
                }

                return dishes;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error: GetAllDishesAsync failed");
                return null;
            }
        }

        public async Task<IEnumerable<Dish>> GetDishesByCategoryIdAsync(int categoryId)
        {
            if(categoryId <= 0)
            {
                _logger.LogWarning("GetDishesByCategoryIdAsync: categoryId: {CategoryId} invalid", categoryId);
                return null;
            }

            try
            {
                var dishes = await _dbContext.Dishes.Where(d => d.CategoryId == categoryId)
                                                    .ToListAsync();
                if(!dishes.Any())
                {
                    _logger.LogDebug("GetDishesByCategoryIdAsync: No dishes found! with id: {CategoryId}",categoryId);
                    return null;
                }

                return dishes;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error: GetDishesByCategoryIdAsync failed");
                return null;
            }
        }

        public async Task<IEnumerable<Dish>> GetDishesByNameAsync(string dishName)
        {
            if (string.IsNullOrEmpty(dishName))
            {
                _logger.LogWarning("GetDishesByNameAsync: dishName: {DishName} invalid", dishName);
                return null;
            }

            try
            {
                var dishes = await _dbContext.Dishes.AsNoTracking().Where(d => d.Name == dishName)
                                                    .ToListAsync();
                if (!dishes.Any())
                {
                    _logger.LogDebug("GetDishesByNameAsync: No dishes found! with name: {DishName}", dishName);
                    return null;
                }

                return dishes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetDishesByNameAsync failed");
                return null;
            }
        }

        public async Task<Dish> GetOneDishByIdAsync(int dishId)
        {
            if (dishId <= 0)
            {
                _logger.LogWarning("GetOneDishByIdAsync: dishId: {DishId} invalid", dishId);
                return null;
            }

            try
            {
                var dish = await _dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
                if (dish == null)
                {
                    _logger.LogDebug("GetOneDishByIdAsync: No dishes found! with id: {DishId}", dishId);
                    return null;
                }

                return dish;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetOneDishByIdAsync failed");
                return null;
            }
        }

        public async Task<Dish> GetOneDishByNameAsync(string dishName)
        {
            if (string.IsNullOrEmpty(dishName))
            {
                _logger.LogWarning("GetOneDishByNameAsync: dishName: {DishName} invalid", dishName);
                return null;
            }

            try
            {
                var dish = await _dbContext.Dishes.AsNoTracking().FirstOrDefaultAsync(d => d.Name == dishName);

                if (dish == null)
                {
                    _logger.LogDebug("GetOneDishByNameAsync: No dishes found! with name: {DishName}", dishName);
                    return null;
                }

                return dish;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetOneDishByNameAsync failed");
                return null;
            }
        }

        public async Task<bool> UpdateDishAsync(Dish dish) // inte hela modellen utan endast updaterade fät eller dto.
        {
            if(dish == null)
            {
                _logger.LogWarning("UpdateDishAsync: Dish invalid");
                return false;
            }


            try
            {
                var affected = await _dbContext.Dishes.Where(d => d.Id == dish.Id)
                                      .ExecuteUpdateAsync(setter =>
                                        setter
                                        .SetProperty(s => s.Name, dish.Name)
                                        .SetProperty(s => s.Description, dish.Description)
                                        .SetProperty(s => s.Price, dish.Price)
                                        .SetProperty(s => s.CategoryId, dish.CategoryId)
                                        );


                return affected > 0;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error: UpdateDishAsync failed");
                return false;
            }
        }
    }
}
