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
using HealthCare.Core.DTOS.ClinicModule.RequestDto;

namespace HealthCare.Services.IServices
{
    public interface IDoctorServices
    {
        Task<GeneralResponse<string>> AddSpecialization([FromForm]SpecializationRequestDto dto); 
        Task<GeneralResponse<string>> DeleteSpecialization(int specializationId);
        Task<GeneralResponse<List<SpecializationDto>>> ListOfSpecialization();
        Task<GeneralResponse<List<DoctorDto>>> ListOfDoctors();
        Task<GeneralResponse<List<DoctorDto>>> ListOfDoctorsinHospital(int hospitalId);
        Task<GeneralResponse<List<ListOfHospitalDto>>> ListOfHospitalsADoctorWorksin();
        Task<GeneralResponse<List<DoctorDto>>> GetDoctorByName(string FullName);
        Task<GeneralResponse<List<DoctorDto>>> GetDoctorInSpecificHospitalByName(string FullName, int hospitalId);
        Task<GeneralResponse<AddDoctorResponseDto>> AddDoctor([FromForm]DoctorRequestDto dto);
        Task<GeneralResponse<string>> DeleteDoctor(int doctorId);
        Task<GeneralResponse<DoctorDto>> EditDoctor([FromForm]EditDoctorDto dto);
        Task<GeneralResponse<DoctorDto>> DoctorDetails();
        Task<GeneralResponse<string>> AddDoctorToHospital(int doctorId);
        Task<GeneralResponse<string>> DeleteDoctorFromHospital(int doctorId);
        Task<GeneralResponse<string>> RateTheDoctor(int doctorId, [FromForm] RateRequestDto dto);


    }
}
