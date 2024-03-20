using HealthCare.Core.DTOS.ClinicModule.RequestDto;
using HealthCare.Core.DTOS.DoctorModule.RequestDtos;
using HealthCare.Core.DTOS.HospitalModule.RequestDto;
using HealthCare.Services.IServices;
using HealthCare.Services.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("AddSpecialization")]
        public async Task<IActionResult> AddSpecialization([FromForm] SpecializationRequestDto dto)
        {
            var result = await _doctorServices.AddSpecialization(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("DeleteSpecialization")]
        public async Task<IActionResult> DeleteSpecialization(int specializationId)
        {
            var result = await _doctorServices.DeleteSpecialization(specializationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

      //  [Authorize]
        [HttpGet("ListOfSpecialization")]
        public async Task<IActionResult> ListOfSpecialization()
        {
            var result = await _doctorServices.ListOfSpecialization();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddDoctor([FromForm]DoctorRequestDto dto)
        {
            var result = await _doctorServices.AddDoctor(dto);
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

        [Authorize(Roles = "HospitalAdmin")]
        [HttpDelete("DeleteDoctor")]
        public async Task<IActionResult> DeleteDoctor(int doctorId)
        {
            var result = await _doctorServices.DeleteDoctor(doctorId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("ListOfDoctors")]
        public async Task<IActionResult> ListOfDoctors()
        {
            var result = await _doctorServices.ListOfDoctors();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("DoctorDetails")]
        public async Task<IActionResult> DoctorDetails()
        {
            var result = await _doctorServices.DoctorDetails();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPatch("EditDoctor")]
        public async Task<IActionResult> EditDoctor([FromForm] EditDoctorDto dto)
        {
            var result = await _doctorServices.EditDoctor(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("GetDoctorByName")]
        public async Task<IActionResult> GetDoctorByName(string FullName)
        {
            var result = await _doctorServices.GetDoctorByName(FullName);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin, SuperAdmin")]
        [HttpGet("ListOfDoctorsinHospital")]
        public async Task<IActionResult> ListOfDoctorsinHospital(int hospitalId)
        {
            var result = await _doctorServices.ListOfDoctorsinHospital(hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("ListOfHospitalsADoctorWorksin")]
        public async Task<IActionResult> ListOfHospitalsADoctorWorksin()
        {
            var result = await _doctorServices.ListOfHospitalsADoctorWorksin();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpPost("RateTheDoctor")]
        public async Task<IActionResult> RateTheDoctor(int doctorId, [FromForm] RateRequestDto dto)
        {
            var result = await _doctorServices.RateTheDoctor(doctorId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost("AddDoctorToHospital")]
        public async Task<IActionResult> AddDoctorToHospital(int doctorId)
        {
            var result = await _doctorServices.AddDoctorToHospital(doctorId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpDelete("DeleteDoctorFromHospital")]
        public async Task<IActionResult> DeleteDoctorFromHospital(int doctorId)
        {
            var result = await _doctorServices.DeleteDoctorFromHospital(doctorId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }





    }
}
