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
        public async Task<IActionResult> AddMedicalHistory(string email, [FromForm]AddMedicalHistoryDto dto)
        {
            var result = await _medicalHistoryServices.AddMedicalHistory(email, dto);
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

    }
}
