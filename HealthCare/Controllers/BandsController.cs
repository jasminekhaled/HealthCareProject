using HealthCare.Core.DTOS.AppointmentModule.RequestDto;
using HealthCare.Services.IServices;
using HealthCare.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BandsController : ControllerBase
    {
        private readonly IBandServices _bandServices;

        public BandsController(IBandServices bandServices)
        {
            _bandServices = bandServices;
        }

        [HttpPost("AddPrivateBand")]
        public async Task<IActionResult> AddPrivateBand(string token)
        {
            var result = await _bandServices.AddPrivateBand(token);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("AddPublicBand")]
        public async Task<IActionResult> AddPublicBand(string token, string patientNationalId)
        {
            var result = await _bandServices.AddPublicBand(token, patientNationalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("DeletePrivateBand")]
        public async Task<IActionResult> DeletePrivateBand(string token, int bandId)
        {
            var result = await _bandServices.DeletePrivateBand(token, bandId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("DeletePublicBand")]
        public async Task<IActionResult> DeletePublicBand(string token, int bandId)
        {
            var result = await _bandServices.DeletePublicBand(token, bandId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
