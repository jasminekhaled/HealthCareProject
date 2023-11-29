﻿using AutoMapper;
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
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using HealthCare.Core.Models.PatientModule;

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


        public async Task<GeneralResponse<List<HospitalDto>>> ListOfHospitals()
        {
            try
            {
                var hospitals = await _unitOfWork.HospitalRepository.GetAllIncludedAsync(i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                if(hospitals == null)
                {
                    return new GeneralResponse<List<HospitalDto>>
                    {
                        IsSuccess = false,
                        Message = "No Hospitals Found!!"
                    };

                }
                var data = _mapper.Map<List<HospitalDto>>(hospitals);

                return new GeneralResponse<List<HospitalDto>>
                {
                    IsSuccess = true,
                    Message = "Hospitals Listed Successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<HospitalDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
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
                uploadedFile.FilePath = path;
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
                    Message = "New Hospital Added Successfully.",
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


        public async Task<GeneralResponse<string>> DeleteHospital(int hospitalId)
        {
            try
            {
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalId);
                if(hospital == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No hospital Found ."
                    };
                }
                var upload = await _unitOfWork.UploadedFileRepository.GetByIdAsync(hospital.UploadedFileId);
                if(upload != null)
                {
                    _unitOfWork.UploadedFileRepository.Remove(upload);
                }
                _unitOfWork.HospitalRepository.Remove(hospital);
                await _unitOfWork.CompleteAsync();
                

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "Hospital is deleted successfully."
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

        

        public async Task<GeneralResponse<List<HospitalDto>>> GetHospitalByName(string Name)
        {
            try
            {
                var hospitals = await _unitOfWork.HospitalRepository.GetAllIncludedAsync(i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                var hospital = hospitals.Where(a => a.Name == Name).ToList();
                if (hospital.Count == 0)
                {
                    return new GeneralResponse<List<HospitalDto>>
                    {
                        IsSuccess = false,
                        Message = "No hospital Found."
                    };
                }
                var data = _mapper.Map<List<HospitalDto>>(hospital);

                return new GeneralResponse<List<HospitalDto>>
                {
                    IsSuccess = true,
                    Message = "Hospitals with inserted name listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<HospitalDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }


        public async Task<GeneralResponse<List<HospitalDto>>> GetHospitalByGovernorate(int governoratetId)
        {
            try
            {
                if(!await _unitOfWork.GovernorateRepository.AnyAsync(n => n.Id == governoratetId))
                {
                    return new GeneralResponse<List<HospitalDto>>
                    {
                        IsSuccess = false,
                        Message = "No Governorate Found."
                    };
                }
                var hospitals = await _unitOfWork.HospitalRepository.WhereIncludeAsync(filter: a => a.HospitalGovernorate.GovernorateId == governoratetId, i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                
                if (!hospitals.Any())
                {
                    return new GeneralResponse<List<HospitalDto>>
                    {
                        IsSuccess = false,
                        Message = "No hospital found ."
                    };
                }
                var data = _mapper.Map<List<HospitalDto>>(hospitals);

                return new GeneralResponse<List<HospitalDto>>
                {
                    IsSuccess = true,
                    Message = "Hospitals with inserted name listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<HospitalDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        public async Task<GeneralResponse<HospitalDto>> HospitalDetails(int hospitalId)
        {
            try
            {
                var hospital = await _unitOfWork.HospitalRepository.GetSingleWithIncludesAsync(single: h => h.Id == hospitalId, i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                if(hospital == null)
                {
                    return new GeneralResponse<HospitalDto>
                    {
                        IsSuccess = false,
                        Message = "No Hospital found."
                    };
                }

                var data = _mapper.Map<HospitalDto>(hospital);

                return new GeneralResponse<HospitalDto>
                {
                    IsSuccess = true,
                    Message = "Hospital details have been displayed successfully.",
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
        public async Task<GeneralResponse<HospitalDto>> EditHospital(int hospitalId, [FromForm]EditHospitalDto dto)
        {
            try
            {
                var hospital = await _unitOfWork.HospitalRepository.GetSingleWithIncludesAsync(single: h => h.Id == hospitalId, i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                if (hospital == null)
                {
                    return new GeneralResponse<HospitalDto>
                    {
                        IsSuccess = false,
                        Message = "No Hospital found."
                    };
                }
                var hospitalGovernorate = await _unitOfWork.HospitalGovernorateRepository.SingleOrDefaultAsync(s => s.HospitalId == hospitalId);
                if(dto.GovernorateId != null && !await _unitOfWork.GovernorateRepository.AnyAsync(a => a.Id == dto.GovernorateId))
                {
                    return new GeneralResponse<HospitalDto>
                    {
                        IsSuccess = false,
                        Message = "No Governorate found."
                    };
                }

                if(dto.Image != null)
                {
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
                    var uploadedFile = await _unitOfWork.UploadedFileRepository.GetByIdAsync(hospital.UploadedFileId);
                    if (uploadedFile.FilePath != null)
                    {
                        File.Delete(uploadedFile.FilePath);
                    }
                    uploadedFile.FileName = dto.Image.FileName;
                    uploadedFile.ContentType = dto.Image.ContentType;
                    uploadedFile.StoredFileName = fakeFileName;

                    var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "HospitalImages");
                    var path = Path.Combine(directoryPath, fakeFileName);
                    using FileStream fileStream = new(path, FileMode.Create);
                    dto.Image.CopyTo(fileStream);
                    
                    uploadedFile.FilePath = path;
                    _unitOfWork.UploadedFileRepository.Update(uploadedFile);
                    await _unitOfWork.CompleteAsync();
                }

                hospital.Description = dto.Description ?? hospital.Description;
                hospital.Name = dto.Name ?? hospital.Name;
                hospital.Address = dto.Address ?? hospital.Address;
                hospitalGovernorate.GovernorateId = dto.GovernorateId ?? hospitalGovernorate.GovernorateId;
                if(dto.GovernorateId != null)
                {
                    hospitalGovernorate.Governorate = await _unitOfWork.GovernorateRepository.GetByIdAsync(dto.GovernorateId.Value);
                }
                

                _unitOfWork.HospitalGovernorateRepository.Update(hospitalGovernorate);
                _unitOfWork.HospitalRepository.Update(hospital);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<HospitalDto>(hospital);
                

                return new GeneralResponse<HospitalDto>
                {
                    IsSuccess = true,
                    Message = "Hospital has been edited successfully.",
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

        

        public Task<GeneralResponse<string>> DeleteHospitalAdmin(int hospitalAdminId)
        {
            throw new NotImplementedException();
        }

        

        public Task<GeneralResponse<HospitalAdminDto>> EditHospitalAdmin(EditHospitalAdminDto dto)
        {
            throw new NotImplementedException();
        }

       

        public Task<GeneralResponse<HospitalAdminDto>> HospitalAdminDetails(int hospitalAdminId)
        {
            throw new NotImplementedException();
        }

        

        

        public Task<GeneralResponse<List<HospitalAdminDto>>> ListOfSpecificHospitalAdmins(int HospitalId)
        {
            throw new NotImplementedException();
        }
    }
}
