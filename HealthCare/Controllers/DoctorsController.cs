using HealthCare.Core.DTOS.ClinicModule.RequestDto;
using HealthCare.Core.DTOS.DoctorModule.RequestDtos;
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
        public async Task<IActionResult> AddSpecialization([FromForm] SpecializationRequestDto dto)
        {
            var result = await _doctorServices.AddSpecialization(dto);
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

        [HttpDelete("DeleteDoctor")]
        public async Task<IActionResult> DeleteDoctor(int doctorId)
        {
            var result = await _doctorServices.DeleteDoctor(doctorId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("ListOfDoctors")]
        public async Task<IActionResult> ListOfDoctors()
        {
            var result = await _doctorServices.ListOfDoctors();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("DoctorDetails")]
        public async Task<IActionResult> DoctorDetails(int doctorId)
        {
            var result = await _doctorServices.DoctorDetails(doctorId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPatch("EditDoctor")]
        public async Task<IActionResult> EditDoctor(int doctorId, [FromForm] EditDoctorDto dto)
        {
            var result = await _doctorServices.EditDoctor(doctorId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetDoctorByName")]
        public async Task<IActionResult> GetDoctorByName(string FullName)
        {
            var result = await _doctorServices.GetDoctorByName(FullName);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("ListOfDoctorsinHospital")]
        public async Task<IActionResult> ListOfDoctorsinHospital(int hospitalId)
        {
            var result = await _doctorServices.ListOfDoctorsinHospital(hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("ListOfHospitalsADoctorWorksin")]
        public async Task<IActionResult> ListOfHospitalsADoctorWorksin(int doctorId)
        {
            var result = await _doctorServices.ListOfHospitalsADoctorWorksin(doctorId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("RateTheDoctor")]
        public async Task<IActionResult> RateTheDoctor(int doctorId, int PatientId, [FromForm] RateRequestDto dto)
        {
            var result = await _doctorServices.RateTheDoctor(doctorId, PatientId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("AddDoctorToHospital")]
        public async Task<IActionResult> AddDoctorToHospital(int doctorId, int hospitalAdminId)
        {
            var result = await _doctorServices.AddDoctorToHospital(doctorId, hospitalAdminId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("DeleteDoctorFromHospital")]
        public async Task<IActionResult> DeleteDoctorFromHospital(int doctorId, int hospitalAdminId)
        {
            var result = await _doctorServices.DeleteDoctorFromHospital(doctorId, hospitalAdminId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }





    }
}
