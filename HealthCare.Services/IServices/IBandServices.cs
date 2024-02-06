using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.BandModule.RequestDtos;
using HealthCare.Core.DTOS.BandModule.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.IServices
{
    public interface IBandServices
    {
        Task<GeneralResponse<AddPublicBandDto>> AddPublicBand(string token, string patientNationalId);
        Task<GeneralResponse<AddPrivateBandDto>> AddPrivateBand(string token);
        Task<GeneralResponse<string>> DeletePrivateBand(string token , int bandId);
        Task<GeneralResponse<string>> DeletePublicBand(string token, int bandId);
        Task<GeneralResponse<string>> PrivateBandActivation(string token, int bandId);
        Task<GeneralResponse<string>> PublicBandActivation(string token, int bandId);
        Task<GeneralResponse<BandResponseDto>> ChangePatientOfPrivateBand(string token, int bandId, ChangeBandPatientDto dto);
        Task<GeneralResponse<string>> BandSaved(string token, int bandId);
        Task<GeneralResponse<BandResponseDto>> GetPatientBand(string token);
        Task<GeneralResponse<List<BandResponseDto>>> GetHospitalPrivateBands(string token, int hospitalId);
        Task<GeneralResponse<List<BandResponseDto>>> GetPublicBands();
        Task<GeneralResponse<List<BandResponseDto>>> GetSavedPublicBands(string token);
        Task<GeneralResponse<List<BandResponseDto>>> GetSavedPrivateBands(string token, int hospitalId);
        Task<GeneralResponse<CurrentStateDto>> BandCurrentState(string token, int bandId);
        Task<GeneralResponse<BandResponseDto>> GetPrivateBandByUniqueId(string token, string uniqueId);
        Task<GeneralResponse<BandResponseDto>> GetPublicBandByUniqueId(string uniqueId);
       // Task<GeneralResponse<>> BandAlarm();
    }
}
