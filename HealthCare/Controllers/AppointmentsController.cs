using HealthCare.Core.DTOS.ClinicModule.RequestDto;
using HealthCare.Services.IServices;
using HealthCare.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentServices _appointmentServices;

        public AppointmentsController(IAppointmentServices appointmentServices)
        {
            _appointmentServices = appointmentServices;
        }


        [HttpPost("AddDay")]
        public async Task<IActionResult> Days(string DayName)
        {
            var result = await _appointmentServices.Days(DayName);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
