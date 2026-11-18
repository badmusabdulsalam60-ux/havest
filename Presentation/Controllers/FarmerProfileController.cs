using System.Security.Claims;
using Application.Dtos;
using Application.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/farmer-profile")]
    [Authorize(Roles = "Farmer")]
    public class FarmerProfileController : ControllerBase
    {
        private readonly IFarmerProfileServices _farmerServices;

        public FarmerProfileController(IFarmerProfileServices farmerServices)
        {
            _farmerServices = farmerServices;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var result = await _farmerServices.GetProfileAsync(GetUserId());

            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateFarmerProfileRequestModel model)
        {
            var result = await _farmerServices.UpdateProfileAsync(GetUserId(), model);

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
