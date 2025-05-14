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

        public async Task<bool> DeleteDishAsync(int dishId)
        {
            const int deletedDishId = 1; //Id for "Deleted" dishes

            if(dishId <= 0)
            {
                _logger.LogWarning("DeleteDishAsync: DishId: {DishId} invalid", dishId);
                return false;
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();


            try
            {

                var dishExists = await _dbContext.Dishes.AnyAsync(d => d.Id == dishId);
                if (!dishExists)
                {
                    _logger.LogWarning("DeleteDishAsync: Dish with id {DishId} does not exist.", dishId);
                    return false;
                }

                var pendingOrderItems = await _dbContext.OrderItems
                   .Where(oi => oi.DishId == dishId && oi.Order.Status == OrderStatus.Pending) 
                   .ToListAsync();

                if (pendingOrderItems.Any())
                {
                    _logger.LogInformation("DeleteDishAsync: Deleting pending orders for dishId {DishId}", dishId);

                    foreach (var item in pendingOrderItems)
                    {
                        var orderToUpdate = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == item.OrderId);

                        if(orderToUpdate != null)
                        {
                            orderToUpdate.TotalPrice -= item.Quantity * item.Dish.Price;
                        }
                    }

                    _dbContext.Orders.UpdateRange(pendingOrderItems.Select(oi => oi.Order));
                    
                    _dbContext.OrderItems.RemoveRange(pendingOrderItems);
                }

                var orderItemsToUpdate = await _dbContext.OrderItems
                   .Where(oi => oi.DishId == dishId && oi.Order.Status != OrderStatus.Pending) 
                   .ToListAsync();

                if (orderItemsToUpdate.Any())
                {
                    foreach (var item in orderItemsToUpdate)
                    {
                        item.DishId = deletedDishId; 
                    }
                }

                var deletedDishIngredients = await _dbContext.DishIngredients
                    .Where(di => di.DishId == dishId)
                    .ExecuteDeleteAsync();

                if(deletedDishIngredients == 0)
                {
                    _logger.LogWarning("DeleteDishAsync: No dish ingredients deleted for dishId: {DishId}", dishId);
                    await transaction.RollbackAsync();
                    return false;
                }

                var deletedDish = await _dbContext.Dishes
                    .Where(d => d.Id == dishId)
                    .ExecuteDeleteAsync();

                if (deletedDish == 0)
                {
                    _logger.LogWarning("DeleteDishAsync: No dish deleted for dishId: {DishId}", dishId);
                    await transaction.RollbackAsync();
                    return false;
                }


                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error: DeleteDishAsync failed");
                return false;
            }
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

        public async Task<bool> UpdateDishAsync(Dish dish) 
        {
            if(dish == null)
            {
                _logger.LogWarning("UpdateDishAsync: Dish invalid");
                return false;
            }


            try
            {

                bool categoryExists = await _dbContext.Categories
                                            .AnyAsync(c => c.Id == dish.CategoryId);

                if (!categoryExists)
                {
                    _logger.LogWarning("UpdateDishAsync: Category with ID {CategoryId} does not exist", dish.CategoryId);
                    return false;
                }

                bool nameExists = await _dbContext.Dishes
                                    .AnyAsync(d => d.Name == dish.Name && d.Id != dish.Id);

                if (nameExists)
                {
                    _logger.LogWarning("UpdateDishAsync: Dish name '{DishName}' already exists", dish.Name);
                    return false;
                }



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
