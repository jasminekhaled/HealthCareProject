﻿using HealthCare.Core.DTOS;
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
        Task<GeneralResponse<BandResponseDto>> AddPublicBand(int hospitalAdminId, string patientNationalId);
        Task<GeneralResponse<AddPrivateBandDto>> AddPrivateBand(int hospitalAdminId);
        Task<GeneralResponse<string>> DeletePrivateBand(int hospitalAdminId, int BandId);
        Task<GeneralResponse<string>> DeletePuplicBand(int patientId, int BandId);
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