using Microsoft.Extensions.Logging;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels.IngredientReq;
using PizzeriaApi.Domain.UtilModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Core.Services
{
    public class IngredientsService : IIngredientsService
    {
        private readonly IIngredientsRepo _ingredientsRepo;
        private readonly ILogger<IngredientsService> _logger;

        public IngredientsService(IIngredientsRepo ingredientsRepo, ILogger<IngredientsService> logger)
        {
            _ingredientsRepo = ingredientsRepo;
            _logger = logger;
        }

        public async Task<OperationResult<bool?>> AddIngredientAsync(AddIngredientReq req)
        {
            try
            {
                var newIngredient = new Ingredient
                {
                    Name = req.Name.Trim(),
                    Price = req.Price
                };

                var result = await _ingredientsRepo.AddIngredientAsync(newIngredient);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Ingredient added successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(false, "Failed to add ingredient");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: AddIngredientAsync failed");
                return OperationResult<bool?>.Failure(false, "Error: AddIngredientAsync failed");
            }
        }

        public async Task<OperationResult<bool?>> DeleteIngredientAsync(int ingredientId)
        {
            try
            {
                var result = await _ingredientsRepo.DeleteIngredientAsync(ingredientId);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Ingredient deleted successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(false, "Failed to delete ingredient");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: DeleteIngredientAsync failed");
                return OperationResult<bool?>.Failure(false, "Error: DeleteIngredientAsync failed");
            }
        }

        public async Task<OperationResult<IEnumerable<IngredientDTO>>> GetAllIngredientsAsync()
        {
            try
            {
                var result = await _ingredientsRepo.GetAllIngredientsAsync();

                if (result == null || !result.Any())
                {
                    return OperationResult<IEnumerable<IngredientDTO>>.Failure(Enumerable.Empty<IngredientDTO>(), "No ingredients found");
                }

                var ingredients = MapManyIngredients(result);

                return OperationResult<IEnumerable<IngredientDTO>>.Success(ingredients, "Ingredients retrieved successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error: GetAllIngredientsAsync failed");
                return OperationResult<IEnumerable<IngredientDTO>>.Failure(null, "Error: GetAllIngredientsAsync failed");
            }
        }

        public async Task<OperationResult<IngredientDTO?>> GetIngredientByIdAsync(int ingredientId)
        {
            try
            {
                var result = await _ingredientsRepo.GetIngredientByIdAsync(ingredientId);

                if (result == null)
                {
                    
                    return OperationResult<IngredientDTO?>.Failure(null, "No ingredient found with the given ID");
                }

                var ingredient = MapOneIngredient(result);

                return OperationResult<IngredientDTO?>.Success(ingredient, "Ingredient retrieved successfully");


            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error: GetIngredientByIdAsync failed");
                return OperationResult<IngredientDTO?>.Failure(null, "Error: GetIngredientByIdAsync failed");
            }
        }

        public async Task<OperationResult<IngredientDTO?>> GetIngredientByNameAsync(string ingredientName)
        {
            try
            {
                var result = await _ingredientsRepo.GetIngredientByNameAsync(ingredientName);

                if (result == null)
                {
                    
                    return OperationResult<IngredientDTO?>.Failure(null, "No ingredient found with the given name");
                }

                var ingredient = MapOneIngredient(result);

                return OperationResult<IngredientDTO?>.Success(ingredient, "Ingredient retrieved successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error: GetIngredientByNameAsync failed");
                return OperationResult<IngredientDTO?>.Failure(null, "Error: GetIngredientByNameAsync failed");
            }
        }

        public async Task<OperationResult<bool?>> UpdateIngredientAsync(UpdateIngredientReq req)
        {
            try
            {
                var ingredientToUpdate = await _ingredientsRepo.GetIngredientByIdAsync(req.IngredientId);

                if (ingredientToUpdate == null)
                {
                    
                    return OperationResult<bool?>.Failure(false, "No ingredient found with the given ID");
                }


                ingredientToUpdate.Name = req.Name.Trim() ?? ingredientToUpdate.Name;
                ingredientToUpdate.Price = req.Price ?? ingredientToUpdate.Price;

                var result = await _ingredientsRepo.UpdateIngredientAsync(ingredientToUpdate);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Ingredient updated successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(false, "Failed to update ingredient");
                }

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error: UpdateIngredientAsync failed");
                return OperationResult<bool?>.Failure(false, "Error: UpdateIngredientAsync failed");
            }
        }


        private IngredientDTO MapOneIngredient(Ingredient ingredient)
        {
            try
            {
                return new IngredientDTO
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name,
                    Price = ingredient.Price
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: MapOneIngredient failed");
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
                _logger.LogError(ex, "Error: MapManyIngredients failed");
                throw;
            }
        }
    }
}
