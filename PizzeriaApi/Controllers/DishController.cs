using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Domain.RequestModels.DishReq;

namespace PizzeriaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishesService _dishesService;

        public DishController(IDishesService dishesService)
        {
            _dishesService = dishesService;
        }

        [HttpGet("AddDish")]

        public async Task<IActionResult> AddDish([FromBody] AddDishReq req)
        {
            var result = await _dishesService.AddDishAsync(req);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpPut("UpdateDish")]

        public async Task<IActionResult> UpdateDish([FromBody] UpdateDishReq req)
        {
            var result = await _dishesService.UpdateDishAsync(req);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpDelete("DeleteDish")]

        public async Task<IActionResult> DeleteDish([FromQuery] int dishId)
        {
            var result = await _dishesService.DeleteDishAsync(dishId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetAllDishes")]

        public async Task<IActionResult> GetAllDishes([FromQuery] int filter = 3)
        {
            var result = await _dishesService.GetAllDishesAsync(filter);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetDishesByCategoryId")]

        public async Task<IActionResult> GetDishesByCategoryId([FromQuery] int categoryId)
        {
            var result = await _dishesService.GetDishesByCategoryIdAsync(categoryId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetDishesByName")]
        public async Task<IActionResult> GetDishesByName([FromQuery] string dishName)
        {
            var result = await _dishesService.GetDishesByNameAsync(dishName);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetOneDishById")]
        public async Task<IActionResult> GetOneDishById([FromQuery] int dishId)
        {
            var result = await _dishesService.GetOneDishByIdAsync(dishId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetOneDishByName")]
        public async Task<IActionResult> GetOneDishByName([FromQuery] string dishName)
        {
            var result = await _dishesService.GetOneDishByNameAsync(dishName);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

       

        

    }
}
