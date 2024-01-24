﻿using HealthCare.Core.DTOS.ClinicModule.RequestDto;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Services.IServices;
using HealthCare.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicServices _clinicServices;

        public ClinicsController(IClinicServices clinicServices)
        {
            _clinicServices = clinicServices;
        }

        [HttpPost("AddXraySpecialization")]
        public async Task<IActionResult> AddXraySpecialization([FromForm]SpecializationRequestDto dto)
        {
            var result = await _clinicServices.AddXraySpecialization(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("DeleteXraySpecialization")]
        public async Task<IActionResult> DeleteXraySpecialization(int xraySpecializationId)
        {
            var result = await _clinicServices.DeleteXraySpecialization(xraySpecializationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("AddClinic")]
        public async Task<IActionResult> AddClinic(int hospitalAdminId, [FromForm] AddClinicDto dto)
        {
            var result = await _clinicServices.AddClinic(hospitalAdminId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("AddXrayLab")]
        public async Task<IActionResult> AddXrayLab(int hospitalAdminId, [FromForm] AddClinicDto dto)
        {
            var result = await _clinicServices.AddXrayLab(hospitalAdminId, dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("DeleteClinic")]
        public async Task<IActionResult> DeleteClinic(int clinicId)
        {
            var result = await _clinicServices.DeleteClinic(clinicId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("DeleteXrayLab")]
        public async Task<IActionResult> DeleteXrayLab(int xrayLabId)
        {
            var result = await _clinicServices.DeleteXrayLab(xrayLabId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("ListOfClinicsADoctorWorksin")]
        public async Task<IActionResult> ListOfClinicsADoctorWorksin(int doctorId, int hospitalId)
        {
            var result = await _clinicServices.ListOfClinicsADoctorWorksin(doctorId, hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("ListOfXrayLabsADoctorWorksin")]
        public async Task<IActionResult> ListOfXrayLabsADoctorWorksin(int doctorId, int hospitalId)
        {
            var result = await _clinicServices.ListOfXrayLabsADoctorWorksin(doctorId, hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("ListOfClinicsInHospital")]
        public async Task<IActionResult> ListOfClinicsInHospital(int hospitalId)
        {
            var result = await _clinicServices.ListOfClinicsInHospital(hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("ListOfXrayLabsInHospital")]
        public async Task<IActionResult> ListOfXrayLabsInHospital(int hospitalId)
        {
            var result = await _clinicServices.ListOfXrayLabsInHospital(hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

      

    }
}
