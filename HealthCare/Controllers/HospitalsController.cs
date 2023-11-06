using HealthCare.Core.DTOS.HospitalModule.RequestDto;
using HealthCare.Services.IServices;
using HealthCare.Services.Services;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly IHospitalServices _hospitalServices;
        public HospitalsController(IHospitalServices hospitalServices)
        {
            _hospitalServices = hospitalServices;
        }

        [HttpPost("AddHospital")]
        public async Task<IActionResult> AddHospital([FromForm] HospitalRequestDto dto)
        {
            var result = await _hospitalServices.AddHospital(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        
    }
}
