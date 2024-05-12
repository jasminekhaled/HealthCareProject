using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AppointmentModule.RequestDto;
using HealthCare.Core.DTOS.AppointmentModule.ResponseDto;
using HealthCare.Core.DTOS.ClinicModule.RequestDto;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Services.IServices;
using HealthCare.Services.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost("AddClinicAppointment")]
        public async Task<IActionResult> AddClinicAppointment(int clinicId, [FromForm]AddAppointmentRequestDto dto)
        {
            var result = await _appointmentServices.AddClinicAppointment(clinicId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost("AddLabAppointment")]
        public async Task<IActionResult> AddLabAppointment(int labId, [FromForm] AddAppointmentRequestDto dto)
        {
            var result = await _appointmentServices.AddLabAppointment(labId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost("AddXrayAppointment")]
        public async Task<IActionResult> AddXrayAppointment(int xrayId, [FromForm] AddAppointmentRequestDto dto)
        {
            var result = await _appointmentServices.AddXrayAppointment(xrayId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost("AddClinicAppointmentDatesToAnAppointment")]
        public async Task<IActionResult> AddClinicAppointmentDatesToAnAppointment(int clinicAppointmentId, [FromForm]AddAppointmentDateRequestDto dto)
        {
            var result = await _appointmentServices.AddClinicAppointmentDatesToAnAppointment(clinicAppointmentId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost("AddXrayAppointmentDatesToAnAppointment")]
        public async Task<IActionResult> AddXrayAppointmentDatesToAnAppointment(int xrayAppointmentId, [FromForm]AddAppointmentDateRequestDto dto)
        {
            var result = await _appointmentServices.AddXrayAppointmentDatesToAnAppointment(xrayAppointmentId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost("AddLabAppointmentDatesToAnAppointment")]
        public async Task<IActionResult> AddLabAppointmentDatesToAnAppointment(int labAppointmentId, [FromForm]AddAppointmentDateRequestDto dto)
        {
            var result = await _appointmentServices.AddLabAppointmentDatesToAnAppointment(labAppointmentId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpPost("BookAnAppointmentOfClinic")]
        public async Task<IActionResult> BookAnAppointmentOfClinic(int clinicAppointmentDateId, string date)
        {
            var result = await _appointmentServices.BookAnAppointmentOfClinic(clinicAppointmentDateId, date);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpPost("BookAnAppointmentOfLab")]
        public async Task<IActionResult> BookAnAppointmentOfLab(int labAppointmentDateId, string date)
        {
            var result = await _appointmentServices.BookAnAppointmentOfLab(labAppointmentDateId, date);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpPost("BookAnAppointmentOfXray")]
        public async Task<IActionResult> BookAnAppointmentOfXray(int xrayAppointmentDateId, string date)
        {
            var result = await _appointmentServices.BookAnAppointmentOfXray(xrayAppointmentDateId, date);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpDelete("CancelClinicReservation")]
        public async Task<IActionResult> CancelClinicReservation(int clinicReservationId)
        {
            var result = await _appointmentServices.CancelClinicReservation(clinicReservationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpDelete("CancelLabReservation")]
        public async Task<IActionResult> CancelLabReservation(int labReservationId)
        {
            var result = await _appointmentServices.CancelLabReservation(labReservationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpDelete("CancelXrayReservation")]
        public async Task<IActionResult> CancelXrayReservation(int xrayReservationId)
        {
            var result = await _appointmentServices.CancelXrayReservation(xrayReservationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpDelete("DeleteClinicAppointment")]
        public async Task<IActionResult> DeleteClinicAppointment(int clinicAppointmentId)
        {
            var result = await _appointmentServices.DeleteClinicAppointment(clinicAppointmentId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpDelete("DeleteXrayAppointment")]
        public async Task<IActionResult> DeleteXrayAppointment(int xrayAppointmentId)
        {
            var result = await _appointmentServices.DeleteXrayAppointment(xrayAppointmentId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpDelete("DeleteLabAppointment")]
        public async Task<IActionResult> DeleteLabAppointment(int labAppointmentId)
        {
            var result = await _appointmentServices.DeleteLabAppointment(labAppointmentId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpDelete("DeleteClinicAppointmentDateOfAnAppointment")]
        public async Task<IActionResult> DeleteClinicAppointmentDateOfAnAppointment(int clinicAppointmentDateId)
        {
            var result = await _appointmentServices.DeleteClinicAppointmentDateOfAnAppointment(clinicAppointmentDateId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpDelete("DeleteLabAppointmentDateOfAnAppointment")]
        public async Task<IActionResult> DeleteLabAppointmentDateOfAnAppointment(int labAppointmentDateId)
        {
            var result = await _appointmentServices.DeleteLabAppointmentDateOfAnAppointment(labAppointmentDateId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpDelete("DeleteXrayAppointmentDateOfAnAppointment")]
        public async Task<IActionResult> DeleteXrayAppointmentDateOfAnAppointment(int xrayAppointmentDateId)
        {
            var result = await _appointmentServices.DeleteXrayAppointmentDateOfAnAppointment(xrayAppointmentDateId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

       // [Authorize]
        [HttpGet("ListOfAppointmentOfClinic")]
        public async Task<IActionResult> ListOfAppointmentOfClinic(int clinicId)
        {
            var result = await _appointmentServices.ListOfAppointmentOfClinic(clinicId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        //[Authorize]
        [HttpGet("ListOfAppointmentOfLab")]
        public async Task<IActionResult> ListOfAppointmentOfLab(int labId)
        {
            var result = await _appointmentServices.ListOfAppointmentOfLab(labId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

       // [Authorize]
        [HttpGet("ListOfAppointmentOfXray")]
        public async Task<IActionResult> ListOfAppointmentOfXray(int xrayId)
        {
            var result = await _appointmentServices.ListOfAppointmentOfXray(xrayId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Doctor, HospitalAdmin")]
        [HttpGet("ListOfReservationsOfDoctor")]
        public async Task<IActionResult> ListOfReservationsOfDoctor(int doctorId, int hospitalId)
        {
            var result = await _appointmentServices.ListOfReservationsOfDoctor(doctorId, hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("ListOfReservationsOfPatient")]
        public async Task<IActionResult> ListOfReservationsOfPatient()
        {
            var result = await _appointmentServices.ListOfReservationsOfPatient();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Doctor, HospitalAdmin")]
        [HttpDelete("DoneReservation")]
        public async Task<IActionResult> DoneReservation(int reservationId)
        {
            var result = await _appointmentServices.DoneReservation(reservationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Doctor, HospitalAdmin")]
        [HttpDelete("CancelReservationByDoctor")]
        public async Task<IActionResult> CancelReservationByDoctor(int reservationId)
        {
            var result = await _appointmentServices.CancelReservationByDoctor(reservationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpDelete("CancelReservation")]
        public async Task<IActionResult> CancelReservation(int reservationId)
        {
            var result = await _appointmentServices.CancelReservation(reservationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("ListOfDays")]
        public async Task<IActionResult> ListOfDays()
        {
            var result = await _appointmentServices.ListOfDays();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


    }
}
