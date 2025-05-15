using Microsoft.Extensions.Logging;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels.CategoryReq;
using PizzeriaApi.Domain.UtilModels;

namespace PizzeriaApi.Core.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepo _categoriesRepo;
        private readonly ILogger<CategoriesService> _logger;

        public CategoriesService(ICategoriesRepo categoriesRepo, ILogger<CategoriesService> logger)
        {
            _categoriesRepo = categoriesRepo;
            _logger = logger;
        }

        public async Task<OperationResult<bool?>> AddCategory(AddCategoryReq categoryReq)
        {
            try
            {
                var newCategory = new Category
                {
                    Name = categoryReq.CategoryName.Trim(),
                };

                var result = await _categoriesRepo.AddCategory(newCategory);

                if (result)
                {
                    return OperationResult<bool?>.Success(null, "Category added successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to add category");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category");
                return OperationResult<bool?>.Failure(null, "Error adding category");
            }
        }

        public async Task<OperationResult<bool?>> DeleteCategoryById(int categoryId)
        {
            if (categoryId <= 0)
            {
                return OperationResult<bool?>.Failure(null, "Invalid category ID");
            }

            try
            {
                var result = await _categoriesRepo.DeleteCategoryById(categoryId);

                if (result)
                {
                    return OperationResult<bool?>.Success(null, "Category deleted successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to delete category");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category");
                return OperationResult<bool?>.Failure(null, "Error deleting category");
            }
        }

        public async Task<OperationResult<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoriesRepo.GetAllCategoriesAsync();

                if (categories == null || !categories.Any())
                {
                    return OperationResult<IEnumerable<CategoryDTO>>.Failure(Enumerable.Empty<CategoryDTO>(), "No categories found!");
                }

                var categoryDto = MapManyCategories(categories);

                return OperationResult<IEnumerable<CategoryDTO>>.Success(categoryDto, "Categories retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all categories");
                return OperationResult<IEnumerable<CategoryDTO>>.Failure(null, "Error retrieving all categories");
            }
        }

        public async Task<OperationResult<CategoryDTO?>> GetCategoryByIdAsync(int categoryId)
        {
            if (categoryId <= 0)
            {
                return OperationResult<CategoryDTO?>.Failure(null, "Invalid category ID");
            }
            try
            {
                var category = await _categoriesRepo.GetCategoryByIdAsync(categoryId);

                if (category == null)
                {
                    return OperationResult<CategoryDTO?>.Failure(null, "Category not found");
                }

                var categoryDto = MapOneCategory(category);

                return OperationResult<CategoryDTO?>.Success(categoryDto, "Category retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category by ID");
                return OperationResult<CategoryDTO?>.Failure(null, "Error retrieving category by ID");
            }
        }

        public async Task<OperationResult<CategoryDTO?>> GetCategoryByNameAsync(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                return OperationResult<CategoryDTO?>.Failure(null, "Invalid category name");
            }
            try
            {
                var category = await _categoriesRepo.GetCategoryByNameAsync(categoryName);

                if (category == null)
                {
                    return OperationResult<CategoryDTO?>.Failure(null, "Category not found");
                }

                var categoryDto = MapOneCategory(category);

                return OperationResult<CategoryDTO?>.Success(categoryDto, "Category retrieved successfully");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category by name");
                return OperationResult<CategoryDTO?>.Failure(null, "Error retrieving category by name");
            }
        }

        public async Task<OperationResult<bool?>> UpdateCategory(UpdateCategoryReq updateCategoryReq)
        {
            try
            {
                var categoryToUpdate = await _categoriesRepo.GetCategoryByIdAsync(updateCategoryReq.CategoryId);


                if (string.IsNullOrWhiteSpace(updateCategoryReq.CategoryName))
                {
                    return OperationResult<bool?>.Failure(null, "Category name cannot be empty.");
                }

                var trimmedName = updateCategoryReq.CategoryName.Trim();



                if (categoryToUpdate == null)
                {
                    return OperationResult<bool?>.Failure(null, "Category not found");
                }

                bool categoryExists = await _categoriesRepo.CategoryNameExistsAsync(trimmedName);

                if (categoryExists)
                {
                    return OperationResult<bool?>.Failure(null, "Category name already exists");
                }



                categoryToUpdate.Name = trimmedName;

                var result = await _categoriesRepo.UpdateCategory(categoryToUpdate);

                if (result)
                {
                    return OperationResult<bool?>.Success(null, "Category updated successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to update category");
                }

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating category");
                return OperationResult<bool?>.Failure(null, "Error updating category");
            }

        }

        private CategoryDTO MapOneCategory(Category category)
        {
           try
            {
                return new CategoryDTO
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name,
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error mapping category to DTO");
                throw;
            }
        }

        private IEnumerable<CategoryDTO> MapManyCategories(IEnumerable<Category> categories)
        {
           try
            {
                return categories.Select(c => MapOneCategory(c));
               
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error mapping categories to DTOs");
                throw;
            }
        }

       
    }
}
