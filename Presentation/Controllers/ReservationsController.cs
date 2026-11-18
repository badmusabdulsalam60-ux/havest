using System.Security.Claims;
using Application.Dtos;
using Application.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationServices _reservationServices;

        public ReservationsController(IReservationServices reservationServices)
        {
            _reservationServices = reservationServices;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Buyer reserves produce
        [HttpPost]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> Create([FromBody] CreateReservationRequestModel model)
        {
            var result = await _reservationServices.CreateAsync(GetUserId(), model);

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        // Farmer sees received reservations, buyer sees his own
        [HttpGet("mine")]
        [Authorize(Roles = "Farmer,Buyer")]
        public async Task<IActionResult> GetMine()
        {
            var result = await _reservationServices.GetMineAsync(GetUserId());

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id:guid}/accept")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Accept(Guid id)
        {
            var result = await _reservationServices.RespondAsync(GetUserId(), id, true);

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id:guid}/reject")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Reject(Guid id)
        {
            var result = await _reservationServices.RespondAsync(GetUserId(), id, false);

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
