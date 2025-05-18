using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Domain.RequestModels.PizzeriaUserReq;
using System.Security.Claims;

namespace PizzeriaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzeriaUserController : ControllerBase
    {
        private readonly IPizzeriaUserService _pizzeriaUserService;

        public PizzeriaUserController(IPizzeriaUserService pizzeriaUserService)
        {
            _pizzeriaUserService = pizzeriaUserService;
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

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserReq registerUserReq)
        {
            var result = await _pizzeriaUserService.RegisterUserAsync(registerUserReq);
            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginReq loginReq)
        {
            var result = await _pizzeriaUserService.LoginAsync(loginReq);
            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserReq updateUserReq)
        {
            var userId = ReadUserIdFromToken();

            if(userId == null)
            {
                return Unauthorized(new { Data = false, Message = "Unauthorized" });
            }

            updateUserReq.UserId = userId;

            var result = await _pizzeriaUserService.UpdateUserAsync(updateUserReq);
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
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] string userId)// 2 versions
        {
            var result = await _pizzeriaUserService.DeleteUserAsync(userId);
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
        [HttpDelete("DeleteMyUser")]
        public async Task<IActionResult> DeleteMyUser()// 2 versions
        {
            var userId = ReadUserIdFromToken();

            if (userId == null)
            {
                return Unauthorized(new { Data = false, Message = "Unauthorized" });
            }

            var result = await _pizzeriaUserService.DeleteUserAsync(userId);
            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("GetUserWithId")]
        public async Task<IActionResult> GetUserWithId([FromQuery] string userId) //2 versions
        {
            var result = await _pizzeriaUserService.GetUserWithIdAsync(userId);
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
        [Authorize]
        [HttpGet("GetMyUser")]
        public async Task<IActionResult> GetMyUser() //2 versions
        {
            var userId = ReadUserIdFromToken();

            if (userId == null)
            {
                return Unauthorized(new { Data = false, Message = "Unauthorized" });
            }

            var result = await _pizzeriaUserService.GetUserWithIdAsync(userId);
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
        [HttpGet("GetUserByUsername")]
        public async Task<IActionResult> GetUserByUsername([FromQuery] string username)
        {
            var result = await _pizzeriaUserService.GetUserByUsernameAsync(username);
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
        [HttpGet("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var result = await _pizzeriaUserService.GetUserByEmailAsync(email);
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
        [HttpGet("GetPremiumUsers")]
        public async Task<IActionResult> GetPremiumUsers()
        {
            var result = await _pizzeriaUserService.GetPremiumUsers();
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
        [HttpGet("GetRegularUsers")]
        public async Task<IActionResult> GetRegularUsers()
        {
            var result = await _pizzeriaUserService.GetRegularUsers();
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
        [HttpGet("GetUsersWithOrders")]

        public async Task<IActionResult> GetUsersWithOrders()
        {
            var result = await _pizzeriaUserService.GetUsersWithOrders();
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
        [HttpGet("GetUsersByOrderStatus")]

        public async Task<IActionResult> GetUsersByOrderStatus([FromQuery] string orderStatus)
        {
            var result = await _pizzeriaUserService.GetUsersByorderStatus(orderStatus);
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
        [HttpGet("GetBonusByUserId")]
        public async Task<IActionResult> GetBonusByUserId([FromQuery] string userId) //2 versions
        {
            var result = await _pizzeriaUserService.GetBonusByUserIdAsync(userId);
            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }


        [Authorize(AuthenticationSchemes ="Bearer",Roles ="PremiumUser")]
        [HttpGet("GetMyBonus")]
        public async Task<IActionResult> GetMyBonus() //2 versions
        {

            var userId = ReadUserIdFromToken();

            if (userId == null)
            {
                return Unauthorized(new { Data = false, Message = "Unauthorized" });
            }

            var result = await _pizzeriaUserService.GetBonusByUserIdAsync(userId);
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
        [HttpPut("UpdateBonus")]
        public async Task<IActionResult> UpdateBonus([FromQuery] string userId, [FromQuery] int newBonusValue)
        {
            var result = await _pizzeriaUserService.UpdateBonusAsync(userId, newBonusValue);
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
        [HttpGet("UserCanUseBonus")]

        public async Task<IActionResult> UserCanUseBonus([FromQuery] string userId) // 2 versions
        {
            var result = await _pizzeriaUserService.UsercanUseBonus(userId);
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
        [HttpGet("CanUseMyBonus")]

        public async Task<IActionResult> CanUseMyBonus() // 2 versions
        {

            var userId = ReadUserIdFromToken();

            if (userId == null)
            {
                return Unauthorized(new { Data = false, Message = "Unauthorized" });
            }

            var result = await _pizzeriaUserService.UsercanUseBonus(userId);
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
        [HttpGet("GetAllUsers")]

        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _pizzeriaUserService.GetAllUsers();
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
        [HttpGet("GetUsersWithNoOrders")]
        public async Task<IActionResult> GetUsersWithNoOrders()
        {
            var result = await _pizzeriaUserService.GetUsersWithNoOrders();
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
        [HttpPut("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole([FromQuery] string userId, [FromQuery] string role)
        {
            var result = await _pizzeriaUserService.UpdateUserRoleAsync(userId, role);
            if (result.IsSuccess)
            {
                return Ok(new { Data = result.Data, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Data = result.Data, Message = result.Message });
            }
        }

        [HttpGet("test-auth")]
        public IActionResult TestAuth()
        {
            if (User.Identity?.IsAuthenticated == true)
                return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
            else
                return Unauthorized();
        }





    }
}
