using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.PatientModule.RequestDto;
using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.IServices
{
    public interface IPatientServices
    {
        Task<GeneralResponse<List<PatientDto>>> ListOfPatients();
        Task<GeneralResponse<PatientDto>> GetPatientByUserName(string userName);
        Task<GeneralResponse<PatientDto>> GetPatientByNationalId(string nationalId);
        Task<GeneralResponse<string>> DeletePatient(int patientId); // it's  better not to use it
        Task<GeneralResponse<PatientDto>> EditPatient([FromForm]EditPatientDto dto);
        Task<GeneralResponse<PatientDto>> PatientDetails();
        Task<GeneralResponse<string>> NumOfAdmins();
        Task<GeneralResponse<string>> NumOfPatient();
        Task<GeneralResponse<string>> NumOfDoctors();
        Task<GeneralResponse<string>> NumOfHospitals();
        Task<GeneralResponse<string>> NumOfBands();



    }
}
