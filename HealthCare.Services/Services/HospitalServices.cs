using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.HospitalModule.RequestDto;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.DTOS.PatientModule.ResponseDto;
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
    public class HospitalServices : IHospitalServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HospitalServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public Task<GeneralResponse<HospitalDto>> AddHospital(HospitalRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<HospitalAdminDto>> AddHospitalAdmin(int OldHospitalAdminId, HospitalAdminRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeleteHospital(int hospitalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeleteHospitalAdmin(int hospitalAdminId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<HospitalDto>> EditHospital(EditHospitalDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<HospitalAdminDto>> EditHospitalAdmin(EditHospitalAdminDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<HospitalDto>>> GetHospitalByGovernment(string GovernmentName)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<PatientDto>>> GetHospitalByName(string Name)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<HospitalAdminDto>> HospitalAdminDetails(int hospitalAdminId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<HospitalDto>> HospitalDetails(int hospitalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<HospitalDto>>> ListOfHospitals()
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<HospitalAdminDto>>> ListOfSpecificHospitalAdmins(int HospitalId)
        {
            throw new NotImplementedException();
        }
    }
}
