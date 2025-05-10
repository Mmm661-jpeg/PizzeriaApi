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
    public class IngredientsRepo: IIngredientsRepo
    {
        private readonly PizzeriaApiDBContext _dbContext;
        private readonly ILogger<IngredientsRepo> _logger;

        public IngredientsRepo(PizzeriaApiDBContext dbContext, ILogger<IngredientsRepo> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> AddIngredientAsync(Ingredient ingredient)
        {
            if (ingredient == null)
            {
                _logger.LogWarning("AddIngredientAsync: Ingredient invalid!");
                return false;
            }

            try
            {
                await _dbContext.AddAsync(ingredient);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: AddIngredientAsync failed");
                return false;
            }
        }

        public async Task<bool> DeleteIngredientAsync(int ingredientId)
        {
            if (ingredientId <= 0)
            {
                _logger.LogDebug("DeleteIngredientAsync: ingredientId: {IngredientId} invalid", ingredientId);
                return false;
            }

            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: DeleteIngredientAsync failed");
                return false;
            }
        }

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
        {
            try
            {
                var Ingredients = await _dbContext.Ingredients.ToListAsync();

                if (!Ingredients.Any())
                {
                    _logger.LogDebug("GetAllIngredientsAsync: No ingredients found!");
                    return null;
                }

                return Ingredients;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetAllIngredientsAsync failed");
                return null;
            }
        }

        public async Task<Ingredient?> GetIngredientByIdAsync(int ingredientId)
        {
            if (ingredientId <= 0)
            {
                _logger.LogWarning("GetIngredientByIdAsync: ingredientId: {IngredientId} invalid", ingredientId);
                return null;
            }

            try
            {
                var ingredients = await _dbContext.Ingredients.FirstOrDefaultAsync(i => i.Id == ingredientId);

                if (ingredients == null)
                {
                    _logger.LogDebug("No ingredients found wiht ingredientId: {IngredientId}", ingredientId);
                    return null;
                }

                return ingredients;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error:  GetIngredientByIdAsync failed!");
                return null;
            }
        }

        public async Task<Ingredient?> GetIngredientByNameAsync(string ingredientName) //asNoTracking
        {
            if (string.IsNullOrEmpty(ingredientName))
            {
                _logger.LogWarning("GetIngredientByNameAsync: ingredientName invalid");
                return null;
            }


            try
            {
                var ingredient = await _dbContext.Ingredients.AsNoTracking().FirstOrDefaultAsync(i => i.Name == ingredientName);

                if (ingredient == null)
                {
                    _logger.LogDebug("GetIngredientByNameAsync: no ingrediennt found with name: {IngredientName}", ingredientName);
                    return null;
                }

                return ingredient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetIngredientByNameAsync failed");
                return null;
            }
        }

        public async Task<bool> UpdateIngredientAsync(Ingredient ingredient)
        {
            if (ingredient == null)
            {
                _logger.LogWarning("UpdateIngredientAsync: ingredient invalid");
                return false;
            }

            try
            {
                var affected = await _dbContext.Ingredients
                    .Where(i => i.Id == ingredient.Id)
                    .ExecuteUpdateAsync(i => i.SetProperty(x => x.Name, ingredient.Name)
                    .SetProperty(x => x.Price, ingredient.Price));

                if (affected == 0)
                {
                    _logger.LogDebug("UpdateIngredientAsync: Updating ingredient with id: {IngredientId} failed", ingredient.Id);
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: UpdateIngredientAsync failed");
                return false;
            }
        }


    }
    
    

}
