using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels.IngredientReq;
using PizzeriaApi.Domain.UtilModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Core.Interfaces
{
    public interface IIngredientsService
    {
        Task<OperationResult<bool?>> AddIngredientAsync(AddIngredientReq req);
        Task<OperationResult<bool?>> DeleteIngredientAsync(int ingredientId);

        Task<OperationResult<bool?>> UpdateIngredientAsync(UpdateIngredientReq req);

        Task<OperationResult<IEnumerable<IngredientDTO>>> GetAllIngredientsAsync();

        Task<OperationResult<IngredientDTO?>> GetIngredientByNameAsync(string ingredientName);

        Task<OperationResult<IngredientDTO?>> GetIngredientByIdAsync(int ingredientId);
    }
}
