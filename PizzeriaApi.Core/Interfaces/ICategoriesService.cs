using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.UtilModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.RequestModels.CategoryReq;

namespace PizzeriaApi.Core.Interfaces
{
    public interface ICategoriesService
    {
        Task<OperationResult<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync();

        Task<OperationResult<CategoryDTO?>> GetCategoryByIdAsync(int categoryId);

        Task<OperationResult<CategoryDTO?>> GetCategoryByNameAsync(string categoryName);

        Task<OperationResult<bool?>> AddCategory(AddCategoryReq categoryReq);

        Task<OperationResult<bool?>> DeleteCategoryById(int categoryId);

        Task<OperationResult<bool?>> UpdateCategory(UpdateCategoryReq updateCategoryReq);
    }
}
