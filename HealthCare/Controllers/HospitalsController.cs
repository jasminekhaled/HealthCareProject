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

        [HttpGet("ListOfHospitals")]
        public async Task<IActionResult> ListOfHospitals()
        {
            var result = await _hospitalServices.ListOfHospitals();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpDelete("DeleteHospital")]
        public async Task<IActionResult> DeleteHospital(int hospitalId)
        {
            var result = await _hospitalServices.DeleteHospital(hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetHospitalByName")]
        public async Task<IActionResult> GetHospitalByName(string Name)
        {
            var result = await _hospitalServices.GetHospitalByName(Name);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("GetHospitalByGovernorate")]
        public async Task<IActionResult> GetHospitalByGovernorate(int governoratetId)
        {
            var result = await _hospitalServices.GetHospitalByGovernorate( governoratetId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("HospitalDetails")]
        public async Task<IActionResult> HospitalDetails(int hospitalId)
        {
            var result = await _hospitalServices.HospitalDetails(hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpPatch("EditHospital")]
        public async Task<IActionResult> EditHospital(int hospitalId, [FromForm] EditHospitalDto dto)
        {
            var result = await _hospitalServices.EditHospital(hospitalId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
