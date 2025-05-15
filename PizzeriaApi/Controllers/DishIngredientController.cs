using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Domain.RequestModels.DishIngredientReq;

namespace PizzeriaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishIngredientController : ControllerBase
    {
        private readonly IDishIngredientsService _dishIngredientsService;

        public DishIngredientController(IDishIngredientsService dishIngredientsService)
        {
            _dishIngredientsService = dishIngredientsService;
        }

        [HttpPost("AddDishIngredient")]

        public async Task<IActionResult> AddDishIngredient([FromBody] AddDishIngredientReq req)
        {
            var result = await _dishIngredientsService.AddDishIngredientAsync(req);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpPost("AddManyDishIngredients")]
        public async Task<IActionResult> AddManyDishIngredients([FromBody] IEnumerable<AddDishIngredientReq> req)
        {
            var result = await _dishIngredientsService.AddDishIngredientsAsync(req);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpPut("UpdateDishIngredient")]
        public async Task<IActionResult> UpdateDishIngredient([FromBody] UpdateDishIngredientReq req)
        {
            var result = await _dishIngredientsService.UpdateDishIngredientAsync(req);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpDelete("DeleteDishIngredient")]
        public async Task<IActionResult> DeleteDishIngredient([FromQuery] int dishIngredientId, int ingredientId)
        {
            var result = await _dishIngredientsService.DeleteIngredientAsync(dishIngredientId,ingredientId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetDishIngredients")]
        public async Task<IActionResult> GetDishIngredients([FromQuery] int dishId)
        {
            var result = await _dishIngredientsService.GetDishIngredientsAsyncWithDishId(dishId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetDishIngredient")]

        public async Task<IActionResult> GetDishIngredient([FromQuery] int dishId, int ingredientId)
        {
            var result = await _dishIngredientsService.GetDishIngredientAsync(dishId, ingredientId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetIngredientsByDishId")]
        public async Task<IActionResult> GetIngredientsByDishId([FromQuery] int dishId)
        {
            var result = await _dishIngredientsService.GetIngredientsByDishIdAsync(dishId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetDishesByIngredientId")]
        public async Task<IActionResult> GetDishesByIngredientId([FromQuery] int ingredientId)
        {
            var result = await _dishIngredientsService.GetDishesByIngredientIdAsync(ingredientId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetIngredientQuantityForDish")]
        public async Task<IActionResult> GetIngredientQuantityForDish([FromQuery] int dishId, int ingredientId)
        {
            var result = await _dishIngredientsService.GetIngredientQuantityForDishAsync(dishId, ingredientId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("CalculateEventuallIngredientCost")]
        public async Task<IActionResult> CalculateEventuallIngredientCost([FromQuery] int ingredientId, decimal quantity)
        {
            var result = await _dishIngredientsService.CalculateEventuallIngredientCostAsync(ingredientId, quantity);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("CalculateCostForDish")]
        public async Task<IActionResult> CalculateCostForDish([FromQuery] int dishId)
        {
            var result = await _dishIngredientsService.CalculateCostForDishAsync(dishId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetRecommendedPriceForDish")]
        public async Task<IActionResult> GetRecommendedPriceForDish([FromQuery] int dishId)
        {
            var result = await _dishIngredientsService.GetRecommendedPriceForDishAsync(dishId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("EvaluateCurrentPriceForDish")]
        public async Task<IActionResult> EvaluateCurrentPriceForDish([FromQuery] int dishId)
        {
            var result = await _dishIngredientsService.EvaluateCurrentPriceForDishAsync(dishId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("DishHasIngredient")]
        public async Task<IActionResult> DishHasIngredient([FromQuery] int dishId, int ingredientId)
        {
            var result = await _dishIngredientsService.DishHasIngredientAsync(dishId, ingredientId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("DishHasIngredientByName")]
        public async Task<IActionResult> DishHasIngredientByName([FromQuery] int dishId, string ingredientName)
        {
            var result = await _dishIngredientsService.DishHasIngredientByNameAsync(dishId, ingredientName);

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
