using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.ClinicModule.RequestDto;
using HealthCare.Core.DTOS.ClinicModule.ResponseDto;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.IServices
{
    public interface IClinicServices
    {
        Task<GeneralResponse<string>> AddXraySpecialization([FromForm]SpecializationRequestDto dto);
        Task<GeneralResponse<string>> DeleteXraySpecialization(int xraySpecializationId);
        Task<GeneralResponse<AddDoctorResponseDto>> AddClinic(int hospitalAdminId, AddClinicDto dto);
        Task<GeneralResponse<AddDoctorResponseDto>> AddXrayLab(int hospitalAdminId, AddClinicDto dto);
        Task<GeneralResponse<string>> DeleteClinic(int clinicId);
        Task<GeneralResponse<string>> DeleteXrayLab(int xrayLabId);
        Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfClinicsInHospital(int hospitalId);
        Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfXrayLabsInHospital(int hospitalId);
        Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfClinicsADoctorWorksin(int doctorId);
        Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfXrayLabsADoctorWorksin(int doctorId);
    }
}
