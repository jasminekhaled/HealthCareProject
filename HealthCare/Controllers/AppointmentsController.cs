using HealthCare.Core.DTOS.AppointmentModule.RequestDto;
using HealthCare.Core.DTOS.ClinicModule.RequestDto;
using HealthCare.Core.Models.AppointmentModule;
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


        [HttpPost("AddClinicAppointment")]
        public async Task<IActionResult> AddClinicAppointment(int clinicId, [FromForm]AddAppointmentRequestDto dto)
        {
            var result = await _appointmentServices.AddClinicAppointment(clinicId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("AddLabAppointment")]
        public async Task<IActionResult> AddLabAppointment(int labId, [FromForm] AddAppointmentRequestDto dto)
        {
            var result = await _appointmentServices.AddLabAppointment(labId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("AddXrayAppointment")]
        public async Task<IActionResult> AddXrayAppointment(int xrayId, [FromForm] AddAppointmentRequestDto dto)
        {
            var result = await _appointmentServices.AddXrayAppointment(xrayId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpPost("AddClinicAppointmentDatesToAnAppointment")]
        public async Task<IActionResult> AddClinicAppointmentDatesToAnAppointment(int clinicAppointmentId, [FromForm]AddAppointmentDateRequestDto dto)
        {
            var result = await _appointmentServices.AddClinicAppointmentDatesToAnAppointment(clinicAppointmentId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("AddXrayAppointmentDatesToAnAppointment")]
        public async Task<IActionResult> AddXrayAppointmentDatesToAnAppointment(int xrayAppointmentId, [FromForm]AddAppointmentDateRequestDto dto)
        {
            var result = await _appointmentServices.AddXrayAppointmentDatesToAnAppointment(xrayAppointmentId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("AddLabAppointmentDatesToAnAppointment")]
        public async Task<IActionResult> AddLabAppointmentDatesToAnAppointment(int labAppointmentId, [FromForm]AddAppointmentDateRequestDto dto)
        {
            var result = await _appointmentServices.AddLabAppointmentDatesToAnAppointment(labAppointmentId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("BookAnAppointmentOfClinic")]
        public async Task<IActionResult> BookAnAppointmentOfClinic(int patientId, int clinicAppointmentDateId, string date)
        {
            var result = await _appointmentServices.BookAnAppointmentOfClinic(patientId, clinicAppointmentDateId, date);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("BookAnAppointmentOfLab")]
        public async Task<IActionResult> BookAnAppointmentOfLab(int patientId, int labAppointmentDateId, string date)
        {
            var result = await _appointmentServices.BookAnAppointmentOfLab(patientId, labAppointmentDateId, date);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("BookAnAppointmentOfXray")]
        public async Task<IActionResult> BookAnAppointmentOfXray(int patientId, int xrayAppointmentDateId, string date)
        {
            var result = await _appointmentServices.BookAnAppointmentOfXray(patientId, xrayAppointmentDateId, date);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
