using HealthCare.Core.DTOS.AppointmentModule.RequestDto;
using HealthCare.Core.DTOS.BandModule.RequestDtos;
using HealthCare.Services.IServices;
using HealthCare.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("AddPrivateBand")]
        public async Task<IActionResult> AddPrivateBand(string token)
        {
            var result = await _bandServices.AddPrivateBand(token);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("AddPublicBand")]
        public async Task<IActionResult> AddPublicBand(string token, string patientNationalId)
        {
            var result = await _bandServices.AddPublicBand(token, patientNationalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("DeletePrivateBand")]
        public async Task<IActionResult> DeletePrivateBand(string token, int bandId)
        {
            var result = await _bandServices.DeletePrivateBand(token, bandId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("DeletePublicBand")]
        public async Task<IActionResult> DeletePublicBand(string token, int bandId)
        {
            var result = await _bandServices.DeletePublicBand(token, bandId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("PrivateBandActivation")]
        public async Task<IActionResult> PrivateBandActivation(string token, int bandId)
        {
            var result = await _bandServices.PrivateBandActivation(token, bandId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("PublicBandActivation")]
        public async Task<IActionResult> PublicBandActivation(string token, int bandId)
        {
            var result = await _bandServices.PublicBandActivation(token, bandId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("BandSaved")]
        public async Task<IActionResult> BandSaved(string token, int bandId)
        {
            var result = await _bandServices.BandSaved(token, bandId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("ChangePatientOfPrivateBand")]
        public async Task<IActionResult> ChangePatientOfPrivateBand(string token, int bandId, ChangeBandPatientDto dto)
        {
            var result = await _bandServices.ChangePatientOfPrivateBand(token, bandId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetHospitalPrivateBands")]
        public async Task<IActionResult> GetHospitalPrivateBands(string token, int hospitalId)
        {
            var result = await _bandServices.GetHospitalPrivateBands(token, hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetPatientBand")]
        public async Task<IActionResult> GetPatientBand(string token)
        {
            var result = await _bandServices.GetPatientBand(token);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetPublicBands")]
        public async Task<IActionResult> GetPublicBands()
        {
            var result = await _bandServices.GetPublicBands();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetSavedPrivateBands")]
        public async Task<IActionResult> GetSavedPrivateBands(string token, int hospitalId)
        {
            var result = await _bandServices.GetSavedPrivateBands(token, hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetSavedPublicBands")]
        public async Task<IActionResult> GetSavedPublicBands(string token)
        {
            var result = await _bandServices.GetSavedPublicBands(token);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("BandCurrentState")]
        public async Task<IActionResult> BandCurrentState(string token, int bandId)
        {
            var result = await _bandServices.BandCurrentState(token, bandId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetPublicBandByUniqueId")]
        public async Task<IActionResult> GetPublicBandByUniqueId(string uniqueId)
        {
            var result = await _bandServices.GetPublicBandByUniqueId(uniqueId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetPrivateBandByUniqueId")]
        public async Task<IActionResult> GetPrivateBandByUniqueId(string token, string uniqueId)
        {
            var result = await _bandServices.GetPrivateBandByUniqueId(token, uniqueId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


    }
}
