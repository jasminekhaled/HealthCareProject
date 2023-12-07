using HealthCare.Core.DTOS.HospitalModule.RequestDto;
using HealthCare.Services.IServices;
using HealthCare.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorServices _doctorServices;

        public DoctorsController(IDoctorServices doctorServices)
        {
            _doctorServices = doctorServices;
        }


        [HttpPost("AddSpecialization")]
        public async Task<IActionResult> AddSpecialization(string Name)
        {
            var result = await _doctorServices.AddSpecialization(Name);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("DeleteSpecialization")]
        public async Task<IActionResult> DeleteSpecialization(int specializationId)
        {
            var result = await _doctorServices.DeleteSpecialization(specializationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("ListOfSpecialization")]
        public async Task<IActionResult> ListOfSpecialization()
        {
            var result = await _doctorServices.ListOfSpecialization();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
