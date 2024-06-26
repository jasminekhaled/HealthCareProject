﻿using HealthCare.Core.DTOS.ClinicModule.RequestDto;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.Models;
using HealthCare.Core.Models.ClinicModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Services.IServices;
using HealthCare.Services.Services;
using Microsoft.AspNetCore.Authorization;
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

      //  [Authorize(Roles = "SuperAdmin")]
        [HttpPost("AddXraySpecialization")]
        public async Task<IActionResult> AddXraySpecialization([FromForm]SpecializationRequestDto dto)
        {
            var result = await _clinicServices.AddXraySpecialization(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

       // [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("DeleteXraySpecialization")]
        public async Task<IActionResult> DeleteXraySpecialization(int xraySpecializationId)
        {
            var result = await _clinicServices.DeleteXraySpecialization(xraySpecializationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("AddLabSpecialization")]
        public async Task<IActionResult> AddLabSpecialization([FromForm] LabSpecializationRequestDto dto)
        {
            var result = await _clinicServices.AddLabSpecialization(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("DeleteLabSpecialization")]
        public async Task<IActionResult> DeleteLabSpecialization(int labSpecializationId)
        {
            var result = await _clinicServices.DeleteLabSpecialization(labSpecializationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

      //  [Authorize]
        [HttpGet("ListOfXraySpecialization")]
        public async Task<IActionResult> ListOfXraySpecialization()
        {
            var result = await _clinicServices.ListOfXraySpecialization();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

       // [Authorize]
        [HttpGet("ListOfLabSpecialization")]
        public async Task<IActionResult> ListOfLabSpecialization()
        {
            var result = await _clinicServices.ListOfLabSpecialization();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost("AddClinic")]
        public async Task<IActionResult> AddClinic([FromForm] AddClinicDto dto)
        {
            var result = await _clinicServices.AddClinic(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost("AddXrayLab")]
        public async Task<IActionResult> AddXrayLab([FromForm] AddClinicDto dto)
        {
            var result = await _clinicServices.AddXrayLab(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpPost("AddLab")]
        public async Task<IActionResult> AddLab([FromForm] AddLabDto dto)
        {
            var result = await _clinicServices.AddLab(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpDelete("DeleteClinic")]
        public async Task<IActionResult> DeleteClinic(int clinicId)
        {
            var result = await _clinicServices.DeleteClinic(clinicId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpDelete("DeleteXrayLab")]
        public async Task<IActionResult> DeleteXrayLab(int xrayLabId)
        {
            var result = await _clinicServices.DeleteXrayLab(xrayLabId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "HospitalAdmin")]
        [HttpDelete("DeleteLab")]
        public async Task<IActionResult> DeleteLab(int labId)
        {
            var result = await _clinicServices.DeleteLab(labId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [Authorize]
        [HttpGet("ListOfClinicsADoctorWorksin")]
        public async Task<IActionResult> ListOfClinicsADoctorWorksin(int doctorId, int hospitalId)
        {
            var result = await _clinicServices.ListOfClinicsADoctorWorksin(doctorId, hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [Authorize]
        [HttpGet("ListOfXrayLabsADoctorWorksin")]
        public async Task<IActionResult> ListOfXrayLabsADoctorWorksin(int doctorId, int hospitalId)
        {
            var result = await _clinicServices.ListOfXrayLabsADoctorWorksin(doctorId, hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet("ListOfLabsADoctorWorksin")]
        public async Task<IActionResult> ListOfLabsADoctorWorksin(int doctorId, int hospitalId)
        {
            var result = await _clinicServices.ListOfLabsADoctorWorksin(doctorId, hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

       // [Authorize]
        [HttpGet("ListOfClinicsInHospital")]
        public async Task<IActionResult> ListOfClinicsInHospital(int hospitalId)
        {
            var result = await _clinicServices.ListOfClinicsInHospital(hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

       // [Authorize]
        [HttpGet("ListOfXrayLabsInHospital")]
        public async Task<IActionResult> ListOfXrayLabsInHospital(int hospitalId)
        {
            var result = await _clinicServices.ListOfXrayLabsInHospital(hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

      //  [Authorize]
        [HttpGet("ListOfLabsInHospital")]
        public async Task<IActionResult> ListOfLabsInHospital(int hospitalId)
        {
            var result = await _clinicServices.ListOfLabsInHospital(hospitalId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

       // [Authorize]
        [HttpGet("ListOfClinicsOfSpecificSpecialization")]
        public async Task<IActionResult> ListOfClinicsOfSpecificSpecialization(int specializationId)
        {
            var result = await _clinicServices.ListOfClinicsOfSpecificSpecialization(specializationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

      //  [Authorize]
        [HttpGet("ListOfXraysOfSpecificSpecialization")]
        public async Task<IActionResult> ListOfXraysOfSpecificSpecialization(int xraySpecializationId)
        {
            var result = await _clinicServices.ListOfXraysOfSpecificSpecialization(xraySpecializationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

       // [Authorize]
        [HttpGet("ListOfLabsOfSpecificSpecialization")]
        public async Task<IActionResult> ListOfLabsOfSpecificSpecialization(int labSpecializationId)
        {
            var result = await _clinicServices.ListOfLabsOfSpecificSpecialization(labSpecializationId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        //[Authorize]
        [HttpGet("FilterClinicsBySpecializeAndGovernorate")]
        public async Task<IActionResult> FilterClinicsBySpecializeAndGovernorate(int SpecializationId, int GovernorateId)
        {
            var result = await _clinicServices.FilterClinicsBySpecializeAndGovernorate(SpecializationId, GovernorateId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

      //  [Authorize]
        [HttpGet("FilterXraysBySpecializeAndGovernorate")]
        public async Task<IActionResult> FilterXraysBySpecializeAndGovernorate(int xraySpecializationId, int GovernorateId)
        {
            var result = await _clinicServices.FilterXraysBySpecializeAndGovernorate(xraySpecializationId, GovernorateId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

       // [Authorize]
        [HttpGet("FilterLabsBySpecializeAndGovernorate")]
        public async Task<IActionResult> FilterLabsBySpecializeAndGovernorate(int labSpecializationId, int GovernorateId)
        {
            var result = await _clinicServices.FilterLabsBySpecializeAndGovernorate(labSpecializationId, GovernorateId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }



    }
}
