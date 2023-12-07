using HealthCare.Core.DTOS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.DTOS.DoctorModule.RequestDtos;

namespace HealthCare.Services.IServices
{
    public interface IDoctorServices
    {
        Task<GeneralResponse<string>> AddSpecialization(string Name);
        Task<GeneralResponse<string>> DeleteSpecialization(int specializationId);
        Task<GeneralResponse<List<SpecializationDto>>> ListOfSpecialization();
        Task<GeneralResponse<List<DoctorDto>>> ListOfDoctors();
        Task<GeneralResponse<List<DoctorDto>>> ListOfDoctorsinHospital(int hospitalId);
        Task<GeneralResponse<List<ListOfHospitalDto>>> ListOfHospitalsADoctorWorksin(int doctorId);
        //Task<GeneralResponse<List<DoctorDto>>> ListOfClinicsADoctorWorksin();
       // Task<GeneralResponse<List<DoctorDto>>> ListOfDoctorsinClinic();
        Task<GeneralResponse<List<DoctorDto>>> GetDoctorByName(string FullName);
        Task<GeneralResponse<List<DoctorDto>>> GetDoctorInSpecificHospitalByName(string FullName);
        Task<GeneralResponse<AddDoctorResponseDto>> AddDoctor(DoctorRequestDto dto);
        Task<GeneralResponse<string>> DeleteDoctor(int doctorId);
        Task<GeneralResponse<EditDoctorResponseDto>> EditDoctor(int doctorId, [FromForm]EditDoctorDto dto);
        Task<GeneralResponse<DoctorDto>> DoctorDetails(int doctorId);
        Task<GeneralResponse<string>> AddDoctorToHospital(int doctorId, int hospitalId);
        Task<GeneralResponse<string>> AddDoctorToClinic(int doctorId, int clinicId);
        Task<GeneralResponse<string>> DeleteDoctorFromHospital(int doctorId, int hospitalId);
        Task<GeneralResponse<string>> DeleteDoctorFromClinic(int doctorId, int clinicId);
        Task<GeneralResponse<string>> RateTheDoctor(int doctorId, int PatientId, [FromForm]RateRequestDto dto);

    }
}
