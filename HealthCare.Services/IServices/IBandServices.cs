using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.BandModule.RequestDtos;
using HealthCare.Core.DTOS.BandModule.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.IServices
{
    public interface IBandServices
    {
        Task<GeneralResponse<AddPublicBandDto>> AddPublicBand(string token, string patientNationalId); //hospitalAdmin
        Task<GeneralResponse<AddPrivateBandDto>> AddPrivateBand(string token); //hospitalAdmin
        Task<GeneralResponse<string>> DeletePrivateBand(string token , int bandId); //hospitalAdmin
        Task<GeneralResponse<string>> DeletePublicBand(string token, int bandId); //Patient
        Task<GeneralResponse<string>> PrivateBandActivation(string token, int bandId); //hospitalAdmin
        Task<GeneralResponse<string>> PublicBandActivation(string token, int bandId); //patient
        Task<GeneralResponse<BandResponseDto>> ChangePatientOfPrivateBand(string token, int bandId, [FromForm]ChangeBandPatientDto dto); //hospitalAdmin
        Task<GeneralResponse<string>> BandSaved(string token, int bandId); //doctor
        Task<GeneralResponse<BandResponseDto>> GetPatientBand(string token); //patient
        Task<GeneralResponse<List<BandResponseDto>>> GetHospitalPrivateBands(string token, int hospitalId); //doctor
        Task<GeneralResponse<List<BandResponseDto>>> GetPublicBands();
        Task<GeneralResponse<List<BandResponseDto>>> GetSavedPublicBands(string token); //doctor
        Task<GeneralResponse<List<BandResponseDto>>> GetSavedPrivateBands(string token, int hospitalId); //doctor
        Task<GeneralResponse<CurrentStateDto>> BandCurrentState(string token, int bandId); //hospitalAdmin, doctor, patient
        Task<GeneralResponse<BandResponseDto>> GetPrivateBandByUniqueId(string token, string uniqueId); //doctor, hospitalAdmin
        Task<GeneralResponse<BandResponseDto>> GetPublicBandByUniqueId(string uniqueId);
       // Task<GeneralResponse<>> BandAlarm();
    }
}
