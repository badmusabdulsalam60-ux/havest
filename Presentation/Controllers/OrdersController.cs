using System.Security.Claims;
using Application.Dtos;
using Application.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Farmer,Buyer")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("mine")]
        public async Task<IActionResult> GetMine()
        {
            var result = await _orderServices.GetMineAsync(GetUserId());

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _orderServices.GetByIdAsync(GetUserId(), id);

            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        // Farmer: Preparing, ReadyForPickup, Delivered, Cancelled.  Buyer: Completed.
        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequestModel model)
        {
            var result = await _orderServices.UpdateStatusAsync(GetUserId(), id, model.Status);

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
