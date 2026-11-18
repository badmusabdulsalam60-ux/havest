using System.Security.Claims;
using Application.Dtos;
using Application.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/buyer-profile")]
    [Authorize(Roles = "Buyer")]
    public class BuyerProfileController : ControllerBase
    {
        private readonly IBuyerProfileServices _buyerServices;

        public BuyerProfileController(IBuyerProfileServices buyerServices)
        {
            _buyerServices = buyerServices;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var result = await _buyerServices.GetProfileAsync(GetUserId());

            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateBuyerProfileRequestModel model)
        {
            var result = await _buyerServices.UpdateProfileAsync(GetUserId(), model);

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
