using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Domain.RequestModels.IngredientReq;

namespace PizzeriaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientsService _ingredientsService;

        public IngredientController(IIngredientsService ingredientsService)
        {
            _ingredientsService = ingredientsService;
        }



        [HttpPost("AddIngredient")]
        public async Task<IActionResult> AddIngredient([FromBody] AddIngredientReq req)
        {
            var result = await _ingredientsService.AddIngredientAsync(req);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }



        [HttpPut("UpdateIngredient")]
        public async Task<IActionResult> UpdateIngredient([FromBody] UpdateIngredientReq req)
        {
            var result = await _ingredientsService.UpdateIngredientAsync(req);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }


      
        [HttpDelete("DeleteIngredient")]
        public async Task<IActionResult> DeleteIngredient([FromQuery] int ingredientId)
        {
            var result = await _ingredientsService.DeleteIngredientAsync(ingredientId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

    
        [HttpGet("GetAllIngredients")]
        public async Task<IActionResult> GetAllIngredients()
        {
            var result = await _ingredientsService.GetAllIngredientsAsync();

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetIngredientById")]
        public async Task<IActionResult> GetIngredientById([FromQuery] int ingredientId)
        {
            var result = await _ingredientsService.GetIngredientByIdAsync(ingredientId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetIngredientByName")]
        public async Task<IActionResult> GetIngredientByName([FromQuery] string ingredientName)
        {
            var result = await _ingredientsService.GetIngredientByNameAsync(ingredientName);

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
