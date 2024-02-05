using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.BandModule.ResponseDtos;
using HealthCare.Core.IRepositories;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.Services
{
    public class BandServices : IBandServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BandServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<GeneralResponse<AddPrivateBandDto>> AddPrivateBand(int hospitalAdminId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<BandResponseDto>> AddPublicBand(int hospitalAdminId, string patientNationalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> BandActivation(int bandId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<CurrentStateDto>> BandCurrentState(int bandId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> BandDeactivation(int bandId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> BandSaved(int doctorId, int bandId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> BandUnsaved(int doctorId, int bandId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<BandResponseDto>> ChangePatientOfPrivateBand(int bandId, int patientNatinalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeletePrivateBand(int hospitalAdminId, int BandId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeletePuplicBand(int patientId, int BandId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<BandResponseDto>>> GetHospitalPrivateBands(int hospitalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<BandResponseDto>> GetPatientBand(int PatientId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<BandResponseDto>>> GetPublicBands()
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<BandResponseDto>>> GetSavedPrivateBands(int doctorId, int hospitalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<BandResponseDto>>> GetSavedPublicBands(int doctorId)
        {
            throw new NotImplementedException();
        }
    }
}
