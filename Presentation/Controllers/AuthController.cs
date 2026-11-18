using Application.Dtos;
using Application.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public AuthController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("register/farmer")]
        public async Task<IActionResult> RegisterFarmer([FromBody] CreateFarmerProfileRequestModel model)
        {
            var result = await _userServices.RegisterFarmerAsync(model);

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register/buyer")]
        public async Task<IActionResult> RegisterBuyer([FromBody] CreateBuyerProfileRequestModel model)
        {
            var result = await _userServices.RegisterBuyerAsync(model);

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var result = await _userServices.LoginAsync(model);

            if (!result.IsSuccessful)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}
