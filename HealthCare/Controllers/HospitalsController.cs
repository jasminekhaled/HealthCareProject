using HealthCare.Core.DTOS.HospitalModule.RequestDto;
using HealthCare.Services.IServices;
using HealthCare.Services.Services;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles ="SuperAdmin")]
        [HttpPost("AddHospital")]
        public async Task<IActionResult> AddHospital([FromForm] HospitalRequestDto dto)
        {
            var result = await _hospitalServices.AddHospital(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("ListOfHospitals")]
        public async Task<IActionResult> ListOfHospitals()
        {
            var result = await _hospitalServices.ListOfHospitals();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("DeleteHospital")]
        public async Task<IActionResult> DeleteHospital(int hospitalId)
        {
            var result = await _hospitalServices.DeleteHospital(hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("GetHospitalByName")]
        public async Task<IActionResult> GetHospitalByName(string Name)
        {
            var result = await _hospitalServices.GetHospitalByName(Name);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("GetHospitalByGovernorate")]
        public async Task<IActionResult> GetHospitalByGovernorate(int governoratetId)
        {
            var result = await _hospitalServices.GetHospitalByGovernorate( governoratetId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("HospitalDetails")]
        public async Task<IActionResult> HospitalDetails(int hospitalId)
        {
            var result = await _hospitalServices.HospitalDetails(hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPatch("EditHospital")]
        public async Task<IActionResult> EditHospital(int hospitalId, [FromForm] EditHospitalDto dto)
        {
            var result = await _hospitalServices.EditHospital(hospitalId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin, SuperAdmin")]
        [HttpPost("AddHospitalAdmin")]
        public async Task<IActionResult> AddHospitalAdmin(int hospitalId, [FromForm] HospitalAdminRequestDto dto)
        {
            var result = await _hospitalServices.AddHospitalAdmin(hospitalId, dto);
            if (result.Data != null)
            {
                var CookieOptions = new CookieOptions()
                {
                    HttpOnly = true,
                    Expires = result.Data.ExpiresOn
                };
                Response.Cookies.Append("refreshToken", result.Data.RefreshToken, CookieOptions);
            }
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin, SuperAdmin")]
        [HttpDelete("DeleteHospitalAdmin")]
        public async Task<IActionResult> DeleteHospitalAdmin(int hospitalAdminId)
        {
            var result = await _hospitalServices.DeleteHospitalAdmin(hospitalAdminId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpGet("HospitalAdminDetails")]
        public async Task<IActionResult> HospitalAdminDetails()
        {
            var result = await _hospitalServices.HospitalAdminDetails();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPatch("EditHospitalAdmin")]
        public async Task<IActionResult> EditHospitalAdmin([FromForm] EditHospitalAdminDto dto)
        {
            var result = await _hospitalServices.EditHospitalAdmin(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin, SuperAdmin")]
        [HttpGet("ListOfSpecificHospitalAdmins")]
        public async Task<IActionResult> ListOfSpecificHospitalAdmins(int HospitalId)
        {
            var result = await _hospitalServices.ListOfSpecificHospitalAdmins(HospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("AddGovernorate")]
        public async Task<IActionResult> AddGovernorate(string name)
        {
            var result = await _hospitalServices.AddGovernorate(name);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("ListOfGovernorates")]
        public async Task<IActionResult> ListOfGovernorates()
        {
            var result = await _hospitalServices.ListOfGovernorates();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("DeleteGovernorate")]
        public async Task<IActionResult> DeleteGovernorate(int governorateId)
        {
            var result = await _hospitalServices.DeleteGovernorate(governorateId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
