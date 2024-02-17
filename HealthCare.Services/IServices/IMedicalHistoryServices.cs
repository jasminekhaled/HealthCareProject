using HealthCare.Core.DTOS.BandModule.ResponseDtos;
using HealthCare.Core.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.DTOS.PatientModule.RequestDto;
using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Services.IServices
{
    public interface IMedicalHistoryServices
    {
        Task<GeneralResponse<PatientResponseDto>> AddMedicalHistory(string email, [FromForm]AddMedicalHistoryDto dto); // delete mediacl history when patient is deleted
        Task<GeneralResponse<MedicalHistoryResponseDto>> EditMedicalHistoryByPatient(int medicalHistoryId, [FromForm]EditMedicalHistoryDto dto); // token of patient
        Task<GeneralResponse<MedicalHistoryResponseDto>> AddXrayFilesToMedicalHistory(int medicalHistoryId, [FromForm]AddFilesDto dto); // token of doctor
        Task<GeneralResponse<MedicalHistoryResponseDto>> AddTestFilesToMedicalHistory(int medicalHistoryId, [FromForm]AddFilesDto dto); // token of doctor
        Task<GeneralResponse<string>> DeleteFileFromMedicalHistory(int fileId); //token of doctor
        Task<GeneralResponse<MedicalHistoryResponseDto>> GetMedicalHistory(int medicalHistoryId); // display medical history in patient apis at response

    }
}
