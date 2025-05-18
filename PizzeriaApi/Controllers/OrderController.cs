using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Domain.RequestModels.OrderReq;
using System.Security.Claims;

namespace PizzeriaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrderController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        private string? ReadUserIdFromToken()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return null; //Eventualy: throw new UnauthorizedAccessException
            }

            return userId;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "RegularUser,PremiumUser")]
        [HttpPost("CreateOrder")]

        public async Task<IActionResult> CreateOrder()
        {

            var userId = ReadUserIdFromToken();

            if (userId == null)
            {
                return Unauthorized(new { Data = false, Message = "Unauthorized" });
            }


            var result = await _ordersService.CreateOrderAsync(userId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPut("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusReq req)
        {

           

            var result = await _ordersService.UpdateOrderStatusAsync(req);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder([FromQuery] int orderId)
        {

          


            var result = await _ordersService.DeleteOrderAsync(orderId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder([FromBody] CancelOrderReq req)
        {

            var userId = ReadUserIdFromToken();

            if (userId == null)
            {
                return Unauthorized(new { Data = false, Message = "Unauthorized" });
            }

            req.UserId = userId;


            var result = await _ordersService.CancelOrderAsync(req);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("GetOrdersByUserId")]
        public async Task<IActionResult> GetOrdersByUserId([FromQuery] string userId)//2 versions
        {
            var result = await _ordersService.GetOrdersByUserIdAsync(userId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "RegularUser,PremiumUser")]
        [HttpGet("GetMyOrders")]
        public async Task<IActionResult> GetOrdersByUserId()//2 versions
        {
            var userId = ReadUserIdFromToken();

            if (userId == null)
            {
                return Unauthorized(new { Data = false, Message = "Unauthorized" });
            }


            var result = await _ordersService.GetOrdersByUserIdAsync(userId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("GetOrderByOrderId")]
        public async Task<IActionResult> GetOrderByOrderId([FromQuery] int orderId)
        {
            var result = await _ordersService.GetOrderByOrderIdAsync(orderId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _ordersService.GetAllOrdersAsync();

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("GetOrdersByDate")]
        public async Task<IActionResult> GetOrdersByDate([FromQuery] DateTime? to, [FromQuery] DateTime? from)
        {
            var result = await _ordersService.GetOrdersByDateAsync(to, from);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("GetOrdersByStatus")]
        public async Task<IActionResult> GetOrdersByStatus([FromQuery] string status)
        {
            var result = await _ordersService.GetOrdersByStatusAsync(status);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("GetOrdersUsingBonus")]
        public async Task<IActionResult> GetOrdersUsingBonus()
        {
            var result = await _ordersService.GetOrdersUsingBonusAsync();

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("GetPendingOrderForUser")]

        public async Task<IActionResult> GetPendingOrderForUser([FromQuery] string userId) //2 versions
        {
            var result = await _ordersService.GetPendingOrderForUserAsync(userId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "RegularUser,PremiumUser")]
        [HttpGet("GetMyPendingOrders")]

        public async Task<IActionResult> GetPendingOrderForUser() //2 versions
        {

            var userId = ReadUserIdFromToken();

            if (userId == null)
            {
                return Unauthorized(new { Data = false, Message = "Unauthorized" });
            }

            var result = await _ordersService.GetPendingOrderForUserAsync(userId);

            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "RegularUser,PremiumUser")]
        [HttpPost("SetOrderPaid")]
        public async Task<IActionResult> SetOrderPaid([FromBody] SetOrderPaidReq req)
        {
            var userId = ReadUserIdFromToken();

            if (userId == null)
            {
                return Unauthorized(new { Data = false, Message = "Unauthorized" });
            }

            req.UserId = userId;

            var result = await _ordersService.SetOrderPaid(req);

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
