using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.HospitalModule.RequestDto;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using HealthCare.Core.IRepositories;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HealthCare.Core.Models;
using HealthCare.Core.Models.HospitalModule;
using Microsoft.Extensions.Configuration;

namespace HealthCare.Services.Services
{
    public class HospitalServices : IHospitalServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public HospitalServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }


        public async Task<GeneralResponse<HospitalDto>> AddHospital([FromForm] HospitalRequestDto dto)
        {
            try
            {
                var governorate = await _unitOfWork.GovernorateRepository.GetByIdAsync(dto.GovernorateId);
                if(governorate == null)
                {
                    return new GeneralResponse<HospitalDto>
                    {
                        IsSuccess = false,
                        Message = "Please choose a Governorate",
                    };
                }

                var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                if (!AllowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                {
                    return new GeneralResponse<HospitalDto>
                    {
                        IsSuccess = false,
                        Message = "Only .jpg and .png Images Are Allowed."
                    };
                }

                if (dto.Image.Length > MaxAllowedPosterSize)
                {
                    return new GeneralResponse<HospitalDto>
                    {
                        IsSuccess = false,
                        Message = "Max Allowed Size Is 1MB."
                    };
                }

                var fakeFileName = Path.GetRandomFileName();
                var uploadedFile = new UploadedFile()
                {
                    FileName = dto.Image.FileName,
                    ContentType = dto.Image.ContentType,
                    StoredFileName = fakeFileName
                };
                var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "HospitalImages");
                var path = Path.Combine(directoryPath, fakeFileName);
                using FileStream fileStream = new(path, FileMode.Create);
                dto.Image.CopyTo(fileStream);
                await _unitOfWork.UploadedFileRepository.AddAsync(uploadedFile);
                await _unitOfWork.CompleteAsync();

                var hospital = _mapper.Map<Hospital>(dto);
                hospital.UploadedFile = uploadedFile;
                await _unitOfWork.HospitalRepository.AddAsync(hospital);
                await _unitOfWork.CompleteAsync();

                var govern = new HospitalGovernorate()
                {
                    HospitalId = hospital.Id,
                    GovernorateId = dto.GovernorateId
                };
                await _unitOfWork.HospitalGovernorateRepository.AddAsync(govern);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<HospitalDto>(hospital);
                data.ImagePath = path;
                data.Governorate = governorate.Name;

                return new GeneralResponse<HospitalDto>
                {
                    IsSuccess = true,
                    Message = "Patients Listed Successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<HospitalDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
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
