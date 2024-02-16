﻿using HealthCare.Core.DTOS;
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
        Task<GeneralResponse<AddPublicBandDto>> AddPublicBand(string patientNationalId); //hospitalAdmin
        Task<GeneralResponse<AddPrivateBandDto>> AddPrivateBand(); //hospitalAdmin
        Task<GeneralResponse<string>> DeletePrivateBand(int bandId); //hospitalAdmin
        Task<GeneralResponse<string>> DeletePublicBand(int bandId); //Patient
        Task<GeneralResponse<string>> PrivateBandActivation(int bandId); //hospitalAdmin
        Task<GeneralResponse<string>> PublicBandActivation(int bandId); //patient
        Task<GeneralResponse<BandResponseDto>> ChangePatientOfPrivateBand(int bandId, [FromForm]ChangeBandPatientDto dto); //hospitalAdmin
        Task<GeneralResponse<string>> BandSaved(int bandId); //doctor
        Task<GeneralResponse<BandResponseDto>> GetPatientBand(); //patient
        Task<GeneralResponse<List<BandResponseDto>>> GetHospitalPrivateBands(int hospitalId); //doctor
        Task<GeneralResponse<List<BandResponseDto>>> GetPublicBands();
        Task<GeneralResponse<List<BandResponseDto>>> GetSavedPublicBands(); //doctor
        Task<GeneralResponse<List<BandResponseDto>>> GetSavedPrivateBands(int hospitalId); //doctor
        Task<GeneralResponse<CurrentStateDto>> BandCurrentState(int bandId); //hospitalAdmin, doctor, patient
        Task<GeneralResponse<BandResponseDto>> GetPrivateBandByUniqueId(string uniqueId); //doctor, hospitalAdmin
        Task<GeneralResponse<BandResponseDto>> GetPublicBandByUniqueId(string uniqueId);
       // Task<GeneralResponse<>> BandAlarm();
    }
}
