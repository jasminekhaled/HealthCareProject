using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using HealthCare.Core.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.DTOS.HospitalModule.RequestDto;
using HealthCare.Core.Models.HospitalModule;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using HealthCare.Core.DTOS.ClinicModule.ResponseDto;

namespace HealthCare.Services.IServices
{
    public interface IHospitalServices
    {
        Task<GeneralResponse<List<ListOfHospitalDto>>> ListOfHospitals();
        Task<GeneralResponse<List<ListOfHospitalDto>>> GetHospitalByGovernorate(int governoratetId);
        Task<GeneralResponse<List<ListOfHospitalDto>>> GetHospitalByName(string Name);
        Task<GeneralResponse<HospitalDto>> AddHospital(HospitalRequestDto dto);
        Task<GeneralResponse<string>> DeleteHospital(int hospitalId);
        Task<GeneralResponse<HospitalDto>> HospitalDetails(int hospitalId);
        Task<GeneralResponse<HospitalDto>> EditHospital(int hospitalId, [FromForm] EditHospitalDto dto);
        Task<GeneralResponse<HospitalAdminDto>> AddHospitalAdmin(int hospitalId, [FromForm] HospitalAdminRequestDto dto);
        Task<GeneralResponse<List<ListOfHospitalAdminDto>>> ListOfSpecificHospitalAdmins(int HospitalId);
        Task<GeneralResponse<string>> DeleteHospitalAdmin(int hospitalAdminId);
        Task<GeneralResponse<EditHospitalAdminResponse>> HospitalAdminDetails();
        Task<GeneralResponse<EditHospitalAdminResponse>> EditHospitalAdmin([FromForm]EditHospitalAdminDto dto);
        Task<GeneralResponse<List<GovernorateDto>>> ListOfGovernorates();
        Task<GeneralResponse<string>> AddGovernorate(string name);
        Task<GeneralResponse<string>> DeleteGovernorate(int governorateId);
        Task<GeneralResponse<List<ListOfSpecificClinics>>> GetHospitalClinicByName(string Name, int specId);
        Task<GeneralResponse<List<ListOfSpecificLabs>>> GetHospitalLabByName(string Name, int specId);
        Task<GeneralResponse<List<ListOfSpecificClinics>>> GetHospitalXrayByName(string Name, int specId);
    }
}
