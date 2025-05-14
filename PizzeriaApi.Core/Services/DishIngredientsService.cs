using Microsoft.Extensions.Logging;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels.DishIngredientReq;
using PizzeriaApi.Domain.UtilModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Core.Services
{
    public class DishIngredientsService:IDishIngredientsService
    {
        private readonly IDishIngredientsRepo _dishIngredientsRepo;
        private readonly ILogger<DishIngredientsService> _logger;

        public DishIngredientsService(IDishIngredientsRepo dishIngredientsRepo, ILogger<DishIngredientsService> logger)
        {
            _dishIngredientsRepo = dishIngredientsRepo;
            _logger = logger;
        }

        public async Task<OperationResult<bool?>> AddDishIngredientAsync(AddDishIngredientReq dishIngredientReq)
        {
            try
            {

                var isValidUnit = Enum.TryParse<Unit>(dishIngredientReq.IngredientUnit, true, out var parsedUnit);

                if (!isValidUnit)
                {
                    return OperationResult<bool?>.Failure(null, "Invalid ingredient unit");
                }

                var newDishIngredient = new DishIngredient
                {
                    DishId = dishIngredientReq.DishId,
                    IngredientId = dishIngredientReq.IngredientId,
                    Quantity = dishIngredientReq.Quantity,
                    Unit = parsedUnit
                };

                var result = await _dishIngredientsRepo.AddDishIngredientAsync(newDishIngredient);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Dish ingredient added successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to add dish ingredient");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding dish ingredient");
                return OperationResult<bool?>.Failure(null, "Error adding dish ingredient");
            }
        }

        public async Task<OperationResult<bool?>> AddDishIngredientsAsync(IEnumerable<AddDishIngredientReq> req)
        {
            try
            {
                var dishIngredients = MapManyOposite(req);

                var result = await _dishIngredientsRepo.AddDishIngredientsAsync(dishIngredients);

                if (result)
                {
                   return OperationResult<bool?>.Success(true, "Dish ingredients added successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to add dish ingredients");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding dish ingredients");
                return OperationResult<bool?>.Failure(null, "Error adding dish ingredients");
            }
        }

        public async Task<OperationResult<decimal?>> CalculateCostForDishAsync(int dishId)
        {
            try
            {
                var result = await _dishIngredientsRepo.CalculateCostForDishAsync(dishId);

                if (result != null)
                {
                    return OperationResult<decimal?>.Success(result, "Cost calculated successfully");
                }
                else
                {
                    return OperationResult<decimal?>.Failure(null, "Failed to calculate cost for dish");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error calculating cost for dish");
                return OperationResult<decimal?>.Failure(null, "Error calculating cost for dish");
            }
        }

        public async Task<OperationResult<decimal?>> CalculateEventuallIngredientCostAsync(int ingredientId, decimal quantity)
        {
            try
            {
                var result = await _dishIngredientsRepo.CalculateEventuallIngredientCostAsync(ingredientId, quantity);
                if (result != null)
                {
                    return OperationResult<decimal?>.Success(result, "Eventual ingredient cost calculated successfully");
                }
                else
                {
                    return OperationResult<decimal?>.Failure(null, "Failed to calculate eventual ingredient cost");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error calculating eventual ingredient cost");
                return OperationResult<decimal?>.Failure(null, "Error calculating eventual ingredient cost");
            }
        }

        public async Task<OperationResult<bool?>> DeleteIngredientAsync(int dishId, int ingredientId)
        {
            try
            {
                var result = await _dishIngredientsRepo.DeleteIngredientAsync(dishId, ingredientId);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Ingredient deleted successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to delete ingredient");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting ingredient");
                return OperationResult<bool?>.Failure(null, "Error deleting ingredient");
            }
        }

        public async Task<OperationResult<bool?>> DishHasIngredientAsync(int dishId, int ingredientId)
        {
            try
            {
                var result = await _dishIngredientsRepo.DishHasIngredientAsync(dishId, ingredientId);

                if (result)
                {
                    return OperationResult<bool?>.Success(result, "Dish has ingredient check completed");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to check if dish has ingredient");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error checking if dish has ingredient");
                return OperationResult<bool?>.Failure(null, "Error checking if dish has ingredient");
            }
        }

        public async Task<OperationResult<bool?>> DishHasIngredientByNameAsync(int dishId, string ingredientName)
        {
            try
            {
                var result = await _dishIngredientsRepo.DishHasIngredientByNameAsync(dishId, ingredientName);

                if (result)
                {
                    return OperationResult<bool?>.Success(result, "Dish has ingredient check completed");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to check if dish has ingredient by name");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error checking if dish has ingredient by name");
                return OperationResult<bool?>.Failure(null, "Error checking if dish has ingredient by name");
            }
        }

        public async Task<OperationResult<string?>> EvaluateCurrentPriceForDishAsync(int dishId)
        {
            try
            {
                var result = await _dishIngredientsRepo.EvaluateCurrentPriceForDishAsync(dishId);

                if (result != null)
                {
                    return OperationResult<string?>.Success(result.ToString(), "Current price evaluated successfully");
                }
                else
                {
                    return OperationResult<string?>.Failure(null, "Failed to evaluate current price for dish");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error evaluating current price for dish");
                return OperationResult<string?>.Failure(null, "Error evaluating current price for dish");
            }
        }

        public async Task<OperationResult<IEnumerable<DishDTO>>> GetDishesByIngredientIdAsync(int ingredientId)
        {
            try
            {
                var result = await _dishIngredientsRepo.GetDishesByIngredientIdAsync(ingredientId);

                if (result == null)
                {
                    return OperationResult<IEnumerable<DishDTO>>.Failure(Enumerable.Empty<DishDTO>(), "No dishes found for the given ingredient ID");
                }

                var dto = MapManyDishes(result);

                return OperationResult<IEnumerable<DishDTO>>.Success(dto, "Dishes retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dishes by ingredient ID");
                return OperationResult<IEnumerable<DishDTO>>.Failure(Enumerable.Empty<DishDTO>(), "Error getting dishes by ingredient ID");
            }
        }

        public async Task<OperationResult<DishIngredientDTO?>> GetDishIngredientAsync(int dishId, int ingredientId)
        {
            try
            {
                var result = await _dishIngredientsRepo.GetDishIngredientAsync(dishId, ingredientId);

                if (result == null)
                {
                    return OperationResult<DishIngredientDTO?>.Failure(null, "Dish ingredient not found");
                }

                var dto = MapOneDishIngredient(result);

                return OperationResult<DishIngredientDTO?>.Success(dto, "Dish ingredient retrieved successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting dish ingredient");
                return OperationResult<DishIngredientDTO?>.Failure(null, "Error getting dish ingredient");
            }
        }

        public Task<OperationResult<IEnumerable<DishIngredientDTO>>> GetDishIngredientsAsyncWithDishId(int dishId)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<decimal?>> GetIngredientQuantityForDishAsync(int dishId, int ingredientId)
        {
            try
            {
                var result = await _dishIngredientsRepo.GetIngredientQuantityForDishAsync(dishId, ingredientId);

                if (result == null)
                {
                    return OperationResult<decimal?>.Failure(null, "Ingredient quantity not found");
                }

                return OperationResult<decimal?>.Success(result, "Ingredient quantity retrieved successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting ingredient quantity for dish");
                return OperationResult<decimal?>.Failure(null, "Error getting ingredient quantity for dish");
            }
        }

        public async Task<OperationResult<IEnumerable<IngredientDTO>>> GetIngredientsByDishIdAsync(int dishId)
        {
            try
            {
                var result = await _dishIngredientsRepo.GetIngredientsByDIshIdAsync(dishId);

                if (result == null)
                {
                    return OperationResult<IEnumerable<IngredientDTO>>.Failure(Enumerable.Empty<IngredientDTO>(), "No ingredients found for the given dish ID");
                }

                var dto = MapManyIngredients(result);

                return OperationResult<IEnumerable<IngredientDTO>>.Success(dto, "Ingredients retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ingredients by dish ID");
                return OperationResult<IEnumerable<IngredientDTO>>.Failure(Enumerable.Empty<IngredientDTO>(), "Error getting ingredients by dish ID");
            }
        }

        public async Task<OperationResult<decimal?>> GetRecommendedPriceForDishAsync(int dishId)
        {
            try
            {
                var result = await _dishIngredientsRepo.GetRecommendedPriceForDishAsync(dishId);

                if (result == null)
                {
                    return OperationResult<decimal?>.Failure(null, "Recommended price not found");
                }

                return OperationResult<decimal?>.Success(result, "Recommended price retrieved successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting recommended price for dish");
                return OperationResult<decimal?>.Failure(null, "Error getting recommended price for dish");
            }
        }

        public async Task<OperationResult<bool?>> UpdateDishIngredientAsync(UpdateDishIngredientReq req)
        {
            try
            {
                var existingIngredient = await _dishIngredientsRepo.GetDishIngredientAsync(req.DishId, req.IngredientId);
                var isValidUnit = Enum.TryParse<Unit>(req.IngredientUnit, true, out var parsedUnit);

                if (!isValidUnit)
                {
                    return OperationResult<bool?>.Failure(null, "Invalid ingredient unit");
                }

                if (existingIngredient == null)
                {
                    return OperationResult<bool?>.Failure(null, "Dish ingredient not found");
                }

                existingIngredient.Quantity = req.Quantity;
                existingIngredient.Unit = parsedUnit;

                var result = await _dishIngredientsRepo.UpdateDishIngredientAsync(existingIngredient);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Dish ingredient updated successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to update dish ingredient");
                }


            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating dish ingredient");
                return OperationResult<bool?>.Failure(null, "Error updating dish ingredient");
            }
        }

        private DishIngredient MapOneOposite(AddDishIngredientReq req)
        {
           try
            {
                if (!Enum.TryParse<Unit>(req.IngredientUnit, true, out var parsedUnit))
                {
                    _logger.LogWarning("Invalid unit: {Unit} for DishId {DishId}, defaulting to Gram", req.IngredientUnit, req.DishId);
                    parsedUnit = Unit.Gram; 
                }

                return new DishIngredient
                {
                    DishId = req.DishId,
                    IngredientId = req.IngredientId,
                    Quantity = req.Quantity,
                    Unit = parsedUnit
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error mapping one dish ingredient");
                throw;
            }
        }

        private IEnumerable<DishIngredient> MapManyOposite(IEnumerable<AddDishIngredientReq> reqs)
        {
            try
            {
                return reqs.Select(MapOneOposite);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping many dish ingredients");
                throw;
            }
        }

        private DishIngredientDTO MapOneDishIngredient(DishIngredient dishIngredient)
        {
            try
            {
                return new DishIngredientDTO
                {
                    DishId = dishIngredient.DishId,
                    IngredientId = dishIngredient.IngredientId,
                    Quantity = dishIngredient.Quantity,
                    IngredientUnit = dishIngredient.Unit.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping one dish ingredient");
                throw;
            }
        }

        private IEnumerable<DishIngredientDTO> MapManyDishIngredients(IEnumerable<DishIngredient> dishIngredients)
        {
            try
            {
                return dishIngredients.Select(d => MapOneDishIngredient(d));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping many dish ingredients");
                throw;
            }
        }

        private IEnumerable<DishDTO> MapManyDishes(IEnumerable<Dish> dishes)
        {
            try
            {
                return dishes.Select(d => new DishDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    CategoryId = d.CategoryId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping many dishes");
                throw;
            }
        }

        private IEnumerable<IngredientDTO> MapManyIngredients(IEnumerable<Ingredient> ingredients)
        {
            try
            {
                return ingredients.Select(i => new IngredientDTO
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping many ingredients");
                throw;
            }
        }
    }
}
