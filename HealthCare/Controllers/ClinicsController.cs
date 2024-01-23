using HealthCare.Core.DTOS.ClinicModule.RequestDto;
using HealthCare.Services.IServices;
using HealthCare.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicServices _clinicServices;

        public ClinicsController(IClinicServices clinicServices)
        {
            _clinicServices = clinicServices;
        }

        [HttpPost("AddXraySpecialization")]
        public async Task<IActionResult> AddXraySpecialization([FromForm]SpecializationRequestDto dto)
        {
            var result = await _clinicServices.AddXraySpecialization(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("DeleteXraySpecialization")]
        public async Task<IActionResult> DeleteXraySpecialization(int xraySpecializationId)
        {
            var result = await _clinicServices.DeleteXraySpecialization(xraySpecializationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
