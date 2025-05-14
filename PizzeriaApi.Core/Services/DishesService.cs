using Microsoft.Extensions.Logging;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels.DishReq;
using PizzeriaApi.Domain.UtilModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PizzeriaApi.Domain.UtilModels.UtilEnums;

namespace PizzeriaApi.Core.Services
{
    public class DishesService : IDishesService
    {
        private readonly IDishesRepo _dishesRepo;
        private readonly ILogger<DishesService> _logger;

        public DishesService(IDishesRepo dishesRepo, ILogger<DishesService> logger)
        {
            _dishesRepo = dishesRepo;
            _logger = logger;
        }

        public async Task<OperationResult<bool?>> AddDishAsync(AddDishReq dishReq)
        {
            try
            {
                var newDish = new Dish
                {
                    Name = dishReq.Name.Trim(),
                    Description = dishReq.Description?.Trim(),
                    Price = dishReq.Price,
                    CategoryId = dishReq.CategoryId,
                };

                var result = await _dishesRepo.AddDishAsync(newDish);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Dish added successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to add dish");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding dish");
                return OperationResult<bool?>.Failure(null, "Error adding dish");
            }
        }

        public async Task<OperationResult<bool?>> DeleteDishAsync(int dishId)
        {
            try
            {
                var result = await _dishesRepo.DeleteDishAsync(dishId);

                if(result)
                {
                    return OperationResult<bool?>.Success(true, "Dish deleted successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to delete dish");
                }


            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting dish");
                return OperationResult<bool?>.Failure(null, "Error deleting dish");
            }
        }

        public async Task<OperationResult<IEnumerable<DishDTO>>> GetAllDishesAsync(int filter = 3)
        {
            try
            {
                if(!Enum.IsDefined(typeof(DishesFilter), filter))
                {
                    return OperationResult<IEnumerable<DishDTO>>.Failure(Enumerable.Empty<DishDTO>(), "Invalid filter option");
                }

                var filterEnum = (DishesFilter)filter;

                var dishes = await _dishesRepo.GetAllDishesAsync();

                if (dishes == null || !dishes.Any())
                {
                    return OperationResult<IEnumerable<DishDTO>>.Failure(Enumerable.Empty<DishDTO>(), "No dishes found");
                }

                var dishesDTO = MapManyDishes(dishes);

                if(filterEnum == DishesFilter.LowPrice)
                {
                    dishesDTO = dishesDTO.OrderBy(d => d.Price);
                }
                else if (filterEnum == DishesFilter.HighPrice)
                {
                    dishesDTO = dishesDTO.OrderByDescending(d => d.Price);
                }

                return OperationResult<IEnumerable<DishDTO>>.Success(dishesDTO, "Dishes retrieved successfully");

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting all dishes");
                return OperationResult<IEnumerable<DishDTO>>.Failure(Enumerable.Empty<DishDTO>(), "Error getting all dishes");
            }
        }

        public async Task<OperationResult<IEnumerable<DishDTO>>> GetDishesByCategoryIdAsync(int categoryId)
        {
            try
            {
                var dishes = await _dishesRepo.GetDishesByCategoryIdAsync(categoryId);

                if (dishes == null || !dishes.Any())
                {
                    return OperationResult<IEnumerable<DishDTO>>.Failure(Enumerable.Empty<DishDTO>(), "No dishes found for the given category ID");
                }

                var dishesDTO = MapManyDishes(dishes);

                return OperationResult<IEnumerable<DishDTO>>.Success(dishesDTO, "Dishes retrieved successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting dishes by category ID");
                return OperationResult<IEnumerable<DishDTO>>.Failure(Enumerable.Empty<DishDTO>(), "Error getting dishes by category ID");
            }
        }

        public async Task<OperationResult<IEnumerable<DishDTO>>> GetDishesByNameAsync(string dishName)
        {
            try
            {
                var dishes = await _dishesRepo.GetDishesByNameAsync(dishName);

                if (dishes == null || !dishes.Any())
                {
                    return OperationResult<IEnumerable<DishDTO>>.Failure(Enumerable.Empty<DishDTO>(), "No dishes found for the given name");
                }

                var dishesDTO = MapManyDishes(dishes);

                return OperationResult<IEnumerable<DishDTO>>.Success(dishesDTO, "Dishes retrieved successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting dishes by name");
                return OperationResult<IEnumerable<DishDTO>>.Failure(Enumerable.Empty<DishDTO>(), "Error getting dishes by name");

            }
        }

        public async Task<OperationResult<DishDTO?>> GetOneDishByIdAsync(int dishId)
        {
            try
            {
                var dish = await _dishesRepo.GetOneDishByIdAsync(dishId);

                if (dish == null)
                {
                    return OperationResult<DishDTO?>.Failure(null, "Dish not found");
                }

                var dishDTO = MapOneDish(dish);

                return OperationResult<DishDTO?>.Success(dishDTO, "Dish retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dish by ID");
                return OperationResult<DishDTO?>.Failure(null, "Error getting dish by ID"   );
            }
        }

        public async Task<OperationResult<DishDTO?>> GetOneDishByNameAsync(string dishName)
        {
            try
            {
                var dish = await _dishesRepo.GetOneDishByNameAsync(dishName);

                if (dish == null)
                {
                    return OperationResult<DishDTO?>.Failure(null, "Dish not found");
                }

                var dishDTO = MapOneDish(dish);

                return OperationResult<DishDTO?>.Success(dishDTO, "Dish retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dish by name");
                return OperationResult<DishDTO?>.Failure(null, "Error getting dish by name");
            }
        }

        public async Task<OperationResult<bool?>> UpdateDishAsync(UpdateDishReq dishReq)
        {
            try
            {
                var existingDish = await _dishesRepo.GetOneDishByIdAsync(dishReq.DishId);

                if (existingDish == null)
                {
                    return OperationResult<bool?>.Failure(null, "Dish not found");
                }

                existingDish.Name = dishReq.Name.Trim();

                existingDish.Description = dishReq.Description?.Trim();

                existingDish.Price = dishReq.Price;

                existingDish.CategoryId = dishReq.CategoryId;

                var result = await _dishesRepo.UpdateDishAsync(existingDish);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Dish updated successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to update dish");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating dish");
                return OperationResult<bool?>.Failure(null, "Error updating dish");
            }
        }

        private DishDTO MapOneDish(Dish dish)
        {
            try
            {
                return new DishDTO
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Description = dish.Description,
                    Price = dish.Price,
                    CategoryId = dish.CategoryId,

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping dish to DTO");
                throw;
            }
        }

        private IEnumerable<DishDTO> MapManyDishes(IEnumerable<Dish> dishes)
        {
            try
            {
                return dishes.Select(d => MapOneDish(d));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping dishes to DTOs");
                throw;
            }
        }
    }
}
