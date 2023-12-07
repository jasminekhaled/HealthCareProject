using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.DoctorModule.RequestDtos;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.IRepositories;
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
using System.Xml.Linq;

namespace HealthCare.Services.Services
{
    public class DoctorServices : IDoctorServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public DoctorServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }


        public async Task<GeneralResponse<string>> AddSpecialization(string Name)
        {
            try
            {
                var specialization = new Specialization()
                {
                    Name = Name
                };
                await _unitOfWork.SpecializationRepository.AddAsync(specialization);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "New Specialization added successfully."
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
        public async Task<GeneralResponse<string>> DeleteSpecialization(int specializationId)
        {
            try
            {
                var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(specializationId);
                if(specialization == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "Specialization Not Found!"
                    };
                }

                _unitOfWork.SpecializationRepository.Remove(specialization);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The Specialization Deleted successfully."
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

        public async Task<GeneralResponse<List<SpecializationDto>>> ListOfSpecialization()
        {
            try
            {
                var specializations = await _unitOfWork.SpecializationRepository.GetAllAsync();
                if (specializations == null)
                {
                    return new GeneralResponse<List<SpecializationDto>>
                    {
                        IsSuccess = false,
                        Message = "No Specializations Found!"
                    };
                }

                var data = _mapper.Map<List<SpecializationDto>>(specializations);

                return new GeneralResponse<List<SpecializationDto>>
                {
                    IsSuccess = true,
                    Message = "The Specializations Listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<SpecializationDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        public Task<GeneralResponse<AddDoctorResponseDto>> AddDoctor(DoctorRequestDto dto)
        {
            throw new NotImplementedException();
        }
        
        public Task<GeneralResponse<string>> DeleteDoctor(int doctorId)
        {
            throw new NotImplementedException();
        }
        
        public Task<GeneralResponse<List<DoctorDto>>> ListOfDoctors()
        {
            throw new NotImplementedException();
        }
        public Task<GeneralResponse<DoctorDto>> DoctorDetails(int doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<EditDoctorResponseDto>> EditDoctor(int doctorId, [FromForm] EditDoctorDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<DoctorDto>>> GetDoctorByName(string FullName)
        {
            throw new NotImplementedException();
        }
        public Task<GeneralResponse<List<DoctorDto>>> GetDoctorInSpecificHospitalByName(string FullName)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<DoctorDto>>> ListOfDoctorsinHospital(int hospitalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<ListOfHospitalDto>>> ListOfHospitalsADoctorWorksin(int doctorId)
        {
            throw new NotImplementedException();
        }
        
        public Task<GeneralResponse<string>> RateTheDoctor(int doctorId, int PatientId, [FromForm] RateRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> AddDoctorToClinic(int doctorId, int clinicId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> AddDoctorToHospital(int doctorId, int hospitalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeleteDoctorFromClinic(int doctorId, int clinicId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeleteDoctorFromHospital(int doctorId, int hospitalId)
        {
            throw new NotImplementedException();
        }

        
    }
}
