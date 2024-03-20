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
    public class MedicalHistoriesController : ControllerBase
    {
        private readonly IMedicalHistoryServices _medicalHistoryServices;
        public MedicalHistoriesController(IMedicalHistoryServices medicalHistoryServices)
        {
            _medicalHistoryServices = medicalHistoryServices;
        }

        [HttpPost("AddMedicalHistory")]
        public async Task<IActionResult> AddMedicalHistory(string userName, [FromForm]AddMedicalHistoryDto dto)
        {
            var result = await _medicalHistoryServices.AddMedicalHistory(userName, dto);
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

        [Authorize(Roles = "Patient, Doctor")]
        [HttpPost("AddTestFilesToMedicalHistory")]
        public async Task<IActionResult> AddTestFilesToMedicalHistory(int medicalHistoryId, [FromForm] AddFilesDto dto)
        {
            var result = await _medicalHistoryServices.AddTestFilesToMedicalHistory(medicalHistoryId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient, Doctor")]
        [HttpPost("AddXrayFilesToMedicalHistory")]
        public async Task<IActionResult> AddXrayFilesToMedicalHistory(int medicalHistoryId, [FromForm] AddFilesDto dto)
        {
            var result = await _medicalHistoryServices.AddXrayFilesToMedicalHistory(medicalHistoryId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient, Doctor")]
        [HttpDelete("DeleteFileFromMedicalHistory")]
        public async Task<IActionResult> DeleteFileFromMedicalHistory(int fileId)
        {
            var result = await _medicalHistoryServices.DeleteFileFromMedicalHistory(fileId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("GetMedicalHistoryByDoctor")]
        public async Task<IActionResult> GetMedicalHistoryByDoctor(int medicalHistoryId)
        {
            var result = await _medicalHistoryServices.GetMedicalHistoryByDoctor(medicalHistoryId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [Authorize(Roles = "Patient")]
        [HttpGet("GetMedicalHistoryByPatient")]
        public async Task<IActionResult> GetMedicalHistoryByPatient()
        {
            var result = await _medicalHistoryServices.GetMedicalHistoryByPatient();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [Authorize(Roles = "Patient")]
        [HttpPut("EditMedicalHistoryByPatient")]
        public async Task<IActionResult> EditMedicalHistoryByPatient([FromForm] EditMedicalHistoryDto dto)
        {
            var result = await _medicalHistoryServices.EditMedicalHistoryByPatient(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



    }
}
