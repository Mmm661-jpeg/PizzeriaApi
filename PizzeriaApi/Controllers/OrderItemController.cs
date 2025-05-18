using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Domain.RequestModels.OrderItemReq;

namespace PizzeriaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "RegularUser,PremiumUser")]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemsService _orderItemService;

        public OrderItemController(IOrderItemsService orderItemService)
        {
            _orderItemService = orderItemService;
        }

       
        [HttpPost("AddOneOrderItem")]

        public async Task<IActionResult> AddOneOrderItem([FromBody] AddOrderItemReq req)
        {
            var result = await _orderItemService.AddOneOrderItemAsync(req);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        
        [HttpPost("AddManyOrderItems")]

        public async Task<IActionResult> AddManyOrderItems([FromBody] IEnumerable<AddOrderItemReq>  req)
        {
            var result = await _orderItemService.AddManyOrderItemAsync(req);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

       
        [HttpPut("UpdateOrderItem")]
        public async Task<IActionResult> UpdateOrderItem([FromBody] UpdateOrderItemReq req)
        {
            var result = await _orderItemService.UpdateOrderItemAsync(req);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpDelete("DeleteOrderItem")]
        public async Task<IActionResult> DeleteOrderItem([FromQuery] int orderItemId)
        {
            var result = await _orderItemService.DeleteOrderItemAsync(orderItemId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetOrderItemsByOrderId")]
        public async Task<IActionResult> GetOrderItemsByOrderId([FromQuery] int orderId)
        {
            var result = await _orderItemService.GetOrderItemsByorderIdAsync(orderId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetOrderItemById")]
        public async Task<IActionResult> GetOrderItemById([FromQuery] int orderItemId)
        {
            var result = await _orderItemService.GetItemsByIdAsync(orderItemId);

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
