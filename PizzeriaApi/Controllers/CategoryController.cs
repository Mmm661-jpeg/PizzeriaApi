using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Domain.RequestModels.CategoryReq;

namespace PizzeriaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoryController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]

        [HttpPost("AddCategory")]

        public async Task<IActionResult> AddCategory([FromBody] AddCategoryReq req)
        {
            var result = await _categoriesService.AddCategory(req);

            if (result.IsSuccess)
            {
                return Ok(new {data = result.Data, message = result.Message });
            }
            else
            {
                return BadRequest(new {data = result.Data, message = result.Message });
            }
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPut("UpdateCategory")]

        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryReq req)
        {
            var result = await _categoriesService.UpdateCategory(req);

            if (result.IsSuccess)
            {
                return Ok(new { data = result.Data, message = result.Message });
            }
            else
            {
                return BadRequest(new { data = result.Data, message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]

        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromQuery] int categoryId)
        {
            var result = await _categoriesService.DeleteCategoryById(categoryId);

            if (result.IsSuccess)
            {
                return Ok(new { data = result.Data, message = result.Message });
            }
            else
            {
                return BadRequest(new { data = result.Data, message = result.Message });
            }
        }

        [HttpGet("GetCategoryById")]

        public async Task<IActionResult> GetCategoryById([FromQuery] int categoryId)
        {
            var result = await _categoriesService.GetCategoryByIdAsync(categoryId);

            if (result.IsSuccess)
            {
                return Ok(new { data = result.Data, message = result.Message });
            }
            else
            {
                return BadRequest(new { data = result.Data, message = result.Message });
            }
        }

        [HttpGet("GetCategoryByName")]
        public async Task<IActionResult> GetCategoryByName([FromQuery] string categoryName)
        {
            var result = await _categoriesService.GetCategoryByNameAsync(categoryName);

            if (result.IsSuccess)
            {
                return Ok(new { data = result.Data, message = result.Message });
            }
            else
            {
                return BadRequest(new { data = result.Data, message = result.Message });
            }
        }

       
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {

            var headers = Request.Headers;
            foreach (var header in headers)
            {
                Console.WriteLine($"{header.Key}: {header.Value}");
            }

            var result = await _categoriesService.GetAllCategoriesAsync();

            if (result.IsSuccess)
            {
                return Ok(new { data = result.Data, message = result.Message });
            }
            else
            {
                return BadRequest(new { data = result.Data, message = result.Message });
            }
        }
    }
}
