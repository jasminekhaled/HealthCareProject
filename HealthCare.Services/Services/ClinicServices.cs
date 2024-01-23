using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.ClinicModule.RequestDto;
using HealthCare.Core.DTOS.ClinicModule.ResponseDto;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.IRepositories;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.ClinicModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.Services
{
    public class ClinicServices : IClinicServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClinicServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<GeneralResponse<string>> AddXraySpecialization([FromForm]SpecializationRequestDto dto)
        {
            try
            {
                var fakeFileName = Path.GetRandomFileName();
                var uploadedFile = new UploadedFile()
                {
                    FileName = dto.Image.FileName,
                    ContentType = dto.Image.ContentType,
                    StoredFileName = fakeFileName
                };
                var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "XraySpecializationImages");
                var path = Path.Combine(directoryPath, fakeFileName);
                using FileStream fileStream = new(path, FileMode.Create);
                dto.Image.CopyTo(fileStream);
                uploadedFile.FilePath = path;
                await _unitOfWork.UploadedFileRepository.AddAsync(uploadedFile);
                await _unitOfWork.CompleteAsync();

                var xraySpecialization = new XraySpecialization()
                {
                    Name = dto.Name,
                    UploadedFile = uploadedFile
                };
                await _unitOfWork.XraySpecializationRepository.AddAsync(xraySpecialization);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "New X-raySpecialization added successfully."
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }

        }
        public async Task<GeneralResponse<string>> DeleteXraySpecialization(int xraySpecializationId)
        {
            try
            {
                var xraySpecialization = await _unitOfWork.XraySpecializationRepository.GetByIdAsync(xraySpecializationId);
                if (xraySpecialization == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "X-raySpecialization Not Found!"
                    };
                }
                var uploadedFile = await _unitOfWork.UploadedFileRepository.GetByIdAsync(xraySpecialization.UploadedFileId);
                File.Delete(uploadedFile.FilePath);
                _unitOfWork.UploadedFileRepository.Remove(uploadedFile);
                _unitOfWork.XraySpecializationRepository.Remove(xraySpecialization);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The X-raySpecialization Deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public Task<GeneralResponse<AddDoctorResponseDto>> AddClinic(int hospitalAdminId, AddClinicDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<AddDoctorResponseDto>> AddXrayLab(int hospitalAdminId, AddClinicDto dto)
        {
            throw new NotImplementedException();
        }


        public Task<GeneralResponse<string>> DeleteClinic(int clinicId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeleteXrayLab(int xrayLabId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfClinicsADoctorWorksin(int doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfClinicsInHospital(int hospitalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfXrayLabsADoctorWorksin(int doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfXrayLabsInHospital(int hospitalId)
        {
            throw new NotImplementedException();
        }
    }
}
