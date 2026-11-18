using System.Security.Claims;
using Application.Dtos;
using Application.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProduceListingsController : ControllerBase
    {
        private readonly IProduceListingServices _listingServices;

        public ProduceListingsController(IProduceListingServices listingServices)
        {
            _listingServices = listingServices;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Buyers browse and search: GET api/producelistings?search=tomato
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailable([FromQuery] string? search)
        {
            var result = await _listingServices.GetAvailableAsync(search);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _listingServices.GetByIdAsync(id);

            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create([FromBody] CreateProduceListingRequestModel model)
        {
            var result = await _listingServices.CreateAsync(GetUserId(), model);

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
