using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.Interfaces
{
    public interface ICategoriesRepo
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<Category> GetCategoryByIdAsync(int categoryId);

        Task<Category> GetCategoryByNameAsync(string Categoryname);

        Task<bool> AddCategory(Category category);

        Task<bool> DeleteCategoryById(int categoryId);

    }
}
