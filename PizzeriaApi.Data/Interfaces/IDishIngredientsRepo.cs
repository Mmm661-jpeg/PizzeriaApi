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
        Task<bool> AddIngredientAsync(Ingredient ingredient);
        Task<bool> DeleteIngredientAsync(int ingredientId);

        Task<bool> UpdateIngredientAsync(Ingredient ingredient);

        Task<IEnumerable<Ingredient>> GetAllAsync();

        Task<Ingredient?> GetIngredientByNameAsync(string ingredientName);

        Task<Ingredient?> GetIngredientByIdAsync(int ingredientId);
    }
}
