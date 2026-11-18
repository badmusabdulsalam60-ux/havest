using Application.Dtos;
using Application.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CropsController : ControllerBase
    {
        private readonly ICropServices _cropServices;

        public CropsController(ICropServices cropServices)
        {
            _cropServices = cropServices;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _cropServices.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Farmer,Admin")]
        public async Task<IActionResult> Create([FromBody] CreateCropRequestModel model)
        {
            var result = await _cropServices.CreateAsync(model);

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Farmer,Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCropRequestModel model)
        {
            var result = await _cropServices.UpdateAsync(id, model);

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
