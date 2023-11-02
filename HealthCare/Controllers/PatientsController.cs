using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.DTOS.PatientModule.RequestDto;
using HealthCare.Services.IServices;
using HealthCare.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientServices _patientServices;
        public PatientsController(IPatientServices patientServices)
        {
            _patientServices = patientServices;
        }
        
        [Authorize(Roles = "SuperAdmin")] 
        [HttpGet("ListOfPatients")]
        public async Task<IActionResult> ListOfPatients()
        {
            var result = await _patientServices.ListOfPatients();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("GetPatientByUserName")]
        public async Task<IActionResult> GetPatientByUserName(string userName)
        {
            var result = await _patientServices.GetPatientByUserName(userName);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("GetPatientByNationalId")]
        public async Task<IActionResult> GetPatientByNationalId(string nationalId)
        {
            var result = await _patientServices.GetPatientByNationalId(nationalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("DeletePatient")]
        public async Task<IActionResult> DeletePatient(int patientId)
        {
            var result = await _patientServices.DeletePatient(patientId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient, SuperAdmin")]
        [HttpGet("PatientDetails")]
        public async Task<IActionResult> PatientDetails(int patientId)
        {
            var result = await _patientServices.PatientDetails(patientId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpPut("EditPatient")]
        public async Task<IActionResult> EditPatient(int PatientId, [FromForm]EditPatientDto dto)
        {
            var result = await _patientServices.EditPatient(PatientId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
