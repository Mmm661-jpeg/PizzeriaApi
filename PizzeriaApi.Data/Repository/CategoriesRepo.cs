using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzeriaApi.Data.DataModels;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.Repository
{
    public class CategoriesRepo : ICategoriesRepo
    {
        private readonly PizzeriaApiDBContext _dbContext;
        private readonly ILogger<CategoriesRepo> _logger;

        public CategoriesRepo(PizzeriaApiDBContext dbContext, ILogger<CategoriesRepo> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> AddCategory(Category category)
        {
            if (category == null)
            {
                _logger.LogWarning("AddCategoryAsync: category is null.");
                return false;
            }

            var categoryExists = await _dbContext.Categories
                .AnyAsync(c => c.Name == category.Name);

            if (categoryExists)
            {
                _logger.LogInformation("AddCategoryAsync: Category {CategoryName} already exists.", category.Name);
                return false;
            }

            try
            {


                await _dbContext.Categories.AddAsync(category);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: AddCategoryAsync with name: {CategoryName} failed", category.Name);
                return false;
            }
        }

        public Task<bool> DeleteCategoryById(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category?>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _dbContext.Categories.ToListAsync();

                if (!categories.Any())
                {
                    _logger.LogDebug("GetAllCategoriesAsync: No categories found!");
                }

                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetAllCategoriesAsync failed");
                return null;
            }
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId)
        {
            if (categoryId <= 0)
            {
                _logger.LogWarning("GetCategoryByIdAsync: Category id invalid: {CategoryId}", categoryId);
                return null;
            }
            try
            {
                var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

                if (category == null)
                {
                    _logger.LogDebug("GetCategoryByIdAsync: No categories found!");
                }

                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetCategoryByIdAsync failed");
                return null;
            }
        }

        public async Task<Category?> GetCategoryByNameAsync(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                _logger.LogWarning("GetCategoryByNameAsync: Category name Invalid: {CategoryName}", categoryName);
                return null;
            }

            try
            {
                var category = await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Name == categoryName);

                if (category == null)
                {
                    _logger.LogDebug("GetCategoryByNameAsync: No categories found!");
                }

                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetCategoryByNameAsync failed");
                return null;
            }
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            if (string.IsNullOrEmpty(category.Name))
            {
                _logger.LogWarning("UpdateCategoryAsync: Category name invalid: {CategoryName}", category.Name);
                return false;
            }

            try
            {
                _dbContext.Categories.Update(category);

                var affectedrows = await _dbContext.SaveChangesAsync();

                if (affectedrows == 0)
                {
                    _logger.LogDebug("UpdateCategoryAsync: Updating category with name: {CategoryName} failed", category.Name);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: UpdateCategoryAsync failed");
                return false;
            }
        }

        public async Task<bool> CategoryNameExistsAsync(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                _logger.LogWarning("CategoryNameExistsAsync: Category name invalid: {CategoryName}", categoryName);
                return false;
            }

            try
            {

                var exists = await _dbContext.Categories.AnyAsync(c => c.Name == categoryName);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: CategoryNameExistsAsync failed");
                return false;
            }
        }
    }
}
