using HealthCare.Core.DTOS.AppointmentModule.RequestDto;
using HealthCare.Core.DTOS.BandModule.RequestDtos;
using HealthCare.Services.IServices;
using HealthCare.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BandsController : ControllerBase
    {
        private readonly IBandServices _bandServices;

        public BandsController(IBandServices bandServices)
        {
            _bandServices = bandServices;
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost("AddPrivateBand")]
        public async Task<IActionResult> AddPrivateBand()
        {
            var result = await _bandServices.AddPrivateBand();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost("AddPublicBand")]
        public async Task<IActionResult> AddPublicBand(string patientNationalId)
        {
            var result = await _bandServices.AddPublicBand(patientNationalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpDelete("DeletePrivateBand")]
        public async Task<IActionResult> DeletePrivateBand(int bandId)
        {
            var result = await _bandServices.DeletePrivateBand(bandId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpDelete("DeletePublicBand")]
        public async Task<IActionResult> DeletePublicBand(int bandId)
        {
            var result = await _bandServices.DeletePublicBand(bandId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        

        [Authorize(Roles = "Doctor")]
        [HttpPost("BandSaved")]
        public async Task<IActionResult> BandSaved(int bandId)
        {
            var result = await _bandServices.BandSaved(bandId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPut("ChangePatientOfPrivateBand")]
        public async Task<IActionResult> ChangePatientOfPrivateBand(int bandId, [FromForm]ChangeBandPatientDto dto)
        {
            var result = await _bandServices.ChangePatientOfPrivateBand(bandId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Doctor, HospitalAdmin, SuperAdmin")]
        [HttpGet("GetHospitalPrivateBands")]
        public async Task<IActionResult> GetHospitalPrivateBands(int hospitalId)
        {
            var result = await _bandServices.GetHospitalPrivateBands(hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("GetPatientBand")]
        public async Task<IActionResult> GetPatientBand()
        {
            var result = await _bandServices.GetPatientBand();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin, Doctor, SuperAdmin")]
        [HttpGet("GetPublicBands")]
        public async Task<IActionResult> GetPublicBands()
        {
            var result = await _bandServices.GetPublicBands();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("GetSavedPrivateBands")]
        public async Task<IActionResult> GetSavedPrivateBands(int hospitalId)
        {
            var result = await _bandServices.GetSavedPrivateBands(hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("GetSavedPublicBands")]
        public async Task<IActionResult> GetSavedPublicBands()
        {
            var result = await _bandServices.GetSavedPublicBands();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        

        [Authorize(Roles = "HospitalAdmin, Doctor, SuperAdmin")]
        [HttpGet("GetPublicBandByUniqueId")]
        public async Task<IActionResult> GetPublicBandByUniqueId(string uniqueId)
        {
            var result = await _bandServices.GetPublicBandByUniqueId(uniqueId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin, Doctor, SuperAdmin")]
        [HttpGet("GetPrivateBandByUniqueId")]
        public async Task<IActionResult> GetPrivateBandByUniqueId(string uniqueId)
        {
            var result = await _bandServices.GetPrivateBandByUniqueId(uniqueId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("BandAlarm")]
        public async Task<IActionResult> BandAlarm(string uniqueId, bool bandAlarm)
        {
            var result = await _bandServices.BandAlarm(uniqueId, bandAlarm);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("ChangePatientFromBand")]
        public async Task<IActionResult> ChangePatientFromBand(string uniqueId, ChangeBandPatientDto dto)
        {
            var result = await _bandServices.ChangePatientFromBand(uniqueId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("FlagStatus")]
        public async Task<IActionResult> FlagStatus(string uniqueId)
        {
            var result = await _bandServices.FlagStatus(uniqueId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("ChangeFlag")]
        public async Task<IActionResult> ChangeFlag(string uniqueId)
        {
            var result = await _bandServices.ChangeFlag(uniqueId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpPost("BandCurrentState")]
        public async Task<IActionResult> BandCurrentState(string uniqueId, BandStateDto dto)
        {
            var result = await _bandServices.BandCurrentState(uniqueId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetBandCurrentState")]
        public async Task<IActionResult> GetBandCurrentState(string uniqueId)
        {
            var result = await _bandServices.GetBandCurrentState(uniqueId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


    }
}
