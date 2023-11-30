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

namespace HealthCare.Services.IServices
{
    public interface IHospitalServices
    {
        Task<GeneralResponse<List<HospitalDto>>> ListOfHospitals();
        Task<GeneralResponse<List<HospitalDto>>> GetHospitalByGovernorate(int governoratetId);
        Task<GeneralResponse<List<HospitalDto>>> GetHospitalByName(string Name);
        Task<GeneralResponse<HospitalDto>> AddHospital(HospitalRequestDto dto);
        Task<GeneralResponse<string>> DeleteHospital(int hospitalId);
        Task<GeneralResponse<HospitalDto>> HospitalDetails(int hospitalId);
        Task<GeneralResponse<HospitalDto>> EditHospital(int hospitalId, [FromForm] EditHospitalDto dto);
        Task<GeneralResponse<HospitalAdminDto>> AddHospitalAdmin(int hospitalId, [FromForm] HospitalAdminRequestDto dto);
        Task<GeneralResponse<List<HospitalAdminDto>>> ListOfSpecificHospitalAdmins(int HospitalId);
        Task<GeneralResponse<string>> DeleteHospitalAdmin(int hospitalAdminId);
        Task<GeneralResponse<EditHospitalAdminResponse>> HospitalAdminDetails(int hospitalAdminId);
        Task<GeneralResponse<EditHospitalAdminResponse>> EditHospitalAdmin(EditHospitalAdminDto dto);

    }
}
