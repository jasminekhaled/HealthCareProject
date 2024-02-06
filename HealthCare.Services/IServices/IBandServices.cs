using HealthCare.Core.DTOS;
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
        Task<GeneralResponse<string>> BandActivation(int bandId);
        Task<GeneralResponse<string>> BandDeactivation(int bandId);
        Task<GeneralResponse<BandResponseDto>> ChangePatientOfPrivateBand(int bandId, int patientNatinalId);
        Task<GeneralResponse<string>> BandSaved(int doctorId, int bandId);
        Task<GeneralResponse<string>> BandUnsaved(int doctorId, int bandId); 
        Task<GeneralResponse<BandResponseDto>> GetPatientBand(int PatientId);
        Task<GeneralResponse<List<BandResponseDto>>> GetHospitalPrivateBands(int hospitalId);
        Task<GeneralResponse<List<BandResponseDto>>> GetPublicBands();
        Task<GeneralResponse<List<BandResponseDto>>> GetSavedPublicBands(int doctorId);
        Task<GeneralResponse<List<BandResponseDto>>> GetSavedPrivateBands(int doctorId, int hospitalId);
        Task<GeneralResponse<CurrentStateDto>> BandCurrentState(int bandId);
       // Task<GeneralResponse<>> BandAlarm();
    }
}
