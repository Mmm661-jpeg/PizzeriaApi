using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.Interfaces
{
    public interface ICategoriesRepo
    {
        Task<IEnumerable<Category?>> GetAllCategoriesAsync();

        Task<Category?> GetCategoryByIdAsync(int categoryId);

        Task<Category?> GetCategoryByNameAsync(string categoryName);

        Task<bool> AddCategory(Category category);

        Task<bool> DeleteCategoryById(int categoryId);

        Task<bool> UpdateCategory(Category category);

        Task<bool> CategoryNameExistsAsync(string categoryName);

    }
}
