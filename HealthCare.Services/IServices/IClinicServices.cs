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
        Task<GeneralResponse<AddClinicResponseDto>> AddClinic(int hospitalAdminId, [FromForm]AddClinicDto dto);
        Task<GeneralResponse<AddClinicResponseDto>> AddXrayLab(int hospitalAdminId, [FromForm]AddClinicDto dto);
        Task<GeneralResponse<string>> DeleteClinic(int clinicId); // donot forget to delete the appointments and reservations of this clinic
        Task<GeneralResponse<string>> DeleteXrayLab(int xrayLabId); // donot forget to delete the appointments and reservations of this clinic
        Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfClinicsInHospital(int hospitalId);
        Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfXrayLabsInHospital(int hospitalId);
        Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfClinicsADoctorWorksin(int doctorId, int hospitalId);
        Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfXrayLabsADoctorWorksin(int doctorId, int hospitalId);
    }
}
