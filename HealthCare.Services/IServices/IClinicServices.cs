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
        Task<GeneralResponse<string>> AddLabSpecialization([FromForm] LabSpecializationRequestDto dto);
        Task<GeneralResponse<string>> DeleteLabSpecialization(int LabSpecializationId);
        Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfXraySpecialization();
        Task<GeneralResponse<List<ListOfLabSpecializationDto>>> ListOfLabSpecialization();
        Task<GeneralResponse<AddClinicResponseDto>> AddClinic([FromForm]AddClinicDto dto);
        Task<GeneralResponse<AddClinicResponseDto>> AddXrayLab([FromForm]AddClinicDto dto);
        Task<GeneralResponse<AddLabResponseDto>> AddLab([FromForm] AddLabDto dto);
        Task<GeneralResponse<string>> DeleteClinic(int clinicId); // donot forget to delete the appointments and reservations of this clinic
        Task<GeneralResponse<string>> DeleteXrayLab(int xrayLabId); // donot forget to delete the appointments and reservations of this clinic
        Task<GeneralResponse<string>> DeleteLab(int labId); // donot forget to delete the appointments and reservations of this clinic
        Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfClinicsInHospital(int hospitalId);
        Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfXrayLabsInHospital(int hospitalId);
        Task<GeneralResponse<List<AddLabResponseDto>>> ListOfLabsInHospital(int hospitalId);
        Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfClinicsADoctorWorksin(int doctorId, int hospitalId);
        Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfXrayLabsADoctorWorksin(int doctorId, int hospitalId);
        Task<GeneralResponse<List<AddLabResponseDto>>> ListOfLabsADoctorWorksin(int doctorId, int hospitalId);//check that itis working
        Task<GeneralResponse<List<ListOfSpecificClinics>>> ListOfClinicsOfSpecificSpecialization(int specializationId);
        Task<GeneralResponse<List<ListOfSpecificClinics>>> ListOfXraysOfSpecificSpecialization(int xraySpecializationId);
        Task<GeneralResponse<List<ListOfSpecificLabs>>> ListOfLabsOfSpecificSpecialization(int labSpecializationId);
        Task<GeneralResponse<List<ListOfSpecificClinics>>> FilterClinicsBySpecializeAndGovernorate(int SpecializationId, int GovernorateId);
        Task<GeneralResponse<List<ListOfSpecificClinics>>> FilterXraysBySpecializeAndGovernorate(int xraySpecializationId, int GovernorateId);
        Task<GeneralResponse<List<ListOfSpecificLabs>>> FilterLabsBySpecializeAndGovernorate(int labSpecializationId, int GovernorateId);
    }
}
