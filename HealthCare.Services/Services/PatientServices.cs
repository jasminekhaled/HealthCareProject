using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AuthModule.ResponseDtos;
using HealthCare.Core.DTOS.BandModule.ResponseDtos;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.DTOS.PatientModule.RequestDto;
using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using HealthCare.Core.Helpers;
using HealthCare.Core.IRepositories;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.Services
{
    public class PatientServices : IPatientServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        public PatientServices(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, 
            IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }
        
        
        public async Task<GeneralResponse<List<PatientDto>>> ListOfPatients()
        {
            try
            {
                var patientUserNames = await _unitOfWork.UserRepository.GetSpecificItems(w => w.Role == User.Patient, 
                    s => s.UserName);
                var patients = await _unitOfWork.PatientRepository.WhereIncludeAsync(
                    w => patientUserNames.Contains(w.UserName),
                    a => a.UploadedFile);

                var data = _mapper.Map<List<PatientDto>>(patients);

                return new GeneralResponse<List<PatientDto>>
                {
                    IsSuccess = true,
                    Message = "Patients Listed Successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<PatientDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        
        public async Task<GeneralResponse<PatientDto>> GetPatientByNationalId(string nationalId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.NationalId == nationalId && 
                s.Role == User.Patient);
                if(user == null)
                {
                    return new GeneralResponse<PatientDto>
                    {
                        IsSuccess = false,
                        Message = "No user was found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == user.UserName,
                    a => a.UploadedFile);

                var data = _mapper.Map<PatientDto>(patient);

                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = true,
                    Message = "The patient was found successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        
        public async Task<GeneralResponse<PatientDto>> GetPatientByUserName(string userName)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.UserName == userName &&
                s.Role == User.Patient);
                if (user == null)
                {
                    return new GeneralResponse<PatientDto>
                    {
                        IsSuccess = false,
                        Message = "No user was found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == userName,
                    a => a.UploadedFile);
                
                var data = _mapper.Map<PatientDto>(patient);

                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = true,
                    Message = "The patient was found successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<string>> DeletePatient(int patientId)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(
                    s => s.Id == patientId && s.IsEmailConfirmed == true && s.MedicalHistory != null);
                if (patient == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Patient had been found!"
                    };
                }
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.UserName == patient.UserName);
                var refreshTokens = await _unitOfWork.RefreshTokenRepository.Where(s => s.userId == user.Id);
                var medicalHistory = await _unitOfWork.MedicalHistoryRepository.SingleOrDefaultAsync(s => s.Id == patient.MedicalHistoryId);
                var uploadedfile = await _unitOfWork.UploadedFileRepository.GetByIdAsync(user.UploadedFileId);

                if (uploadedfile.StoredFileName != "DefaultImage")
                { File.Delete(uploadedfile.FilePath); }

                _unitOfWork.RefreshTokenRepository.RemoveRange(refreshTokens);
                _unitOfWork.UserRepository.Remove(user);
                _unitOfWork.PatientRepository.Remove(patient);
                _unitOfWork.MedicalHistoryRepository.Remove(medicalHistory);
                _unitOfWork.UploadedFileRepository.Remove(uploadedfile);
                await _unitOfWork.CompleteAsync();


                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "Patient deleted Successfully"
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

        public async Task<GeneralResponse<PatientDto>> PatientDetails()
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.Patient);
                if (user == null)
                {
                    return new GeneralResponse<PatientDto>
                    {
                        IsSuccess = false,
                        Message = "No patient Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == user.UserName,
                    a => a.UploadedFile);

                var data = _mapper.Map<PatientDto>(patient);

                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = true,
                    Message = "The patient details is showed successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
   
        public async Task<GeneralResponse<PatientDto>> EditPatient([FromForm]EditPatientDto dto)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.Patient);
                if (user == null)
                {
                    return new GeneralResponse<PatientDto>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == user.UserName,
                    a => a.UploadedFile);

                if (dto.PhoneNumber != null)
                {
                    if(!dto.PhoneNumber.All(char.IsDigit) || dto.PhoneNumber.Length != 11)
                    {
                        return new GeneralResponse<PatientDto>
                        {
                            IsSuccess = false,
                            Message = "Wrong Phone Number"
                        };
                    }     
                }
                if (dto.Image != null)
                {
                    var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                    List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                    if (!AllowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    {
                        return new GeneralResponse<PatientDto>
                        {
                            IsSuccess = false,
                            Message = "Only .jpg and .png Images Are Allowed."
                        };
                    }

                    if (dto.Image.Length > MaxAllowedPosterSize)
                    {
                        return new GeneralResponse<PatientDto>
                        {
                            IsSuccess = false,
                            Message = "Max Allowed Size Is 1MB."
                        };
                    }

                    var fakeFileName = Path.GetRandomFileName();
                    var uploadedFile = await _unitOfWork.UploadedFileRepository.GetByIdAsync(user.UploadedFileId);
                    File.Delete(uploadedFile.FilePath);
                    uploadedFile.FileName = dto.Image.FileName;
                    uploadedFile.ContentType = dto.Image.ContentType;
                    uploadedFile.StoredFileName = fakeFileName;

                    var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "PatientImages");
                    var path = Path.Combine(directoryPath, fakeFileName);
                    using FileStream fileStream = new(path, FileMode.Create);
                    dto.Image.CopyTo(fileStream);

                    uploadedFile.FilePath = path;
                    _unitOfWork.UploadedFileRepository.Update(uploadedFile);
                    await _unitOfWork.CompleteAsync();

                }
                
                patient.PhoneNumber = dto.PhoneNumber ?? patient.PhoneNumber;
                _unitOfWork.PatientRepository.Update(patient);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<PatientDto>(patient);

                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = true,
                    Message = "The patient details is showed successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<string>> NumOfPatient()
        {
            try
            {
                var num = await _unitOfWork.PatientRepository.GetAllAsync();
                var numOfPatient = num.Count();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Data = numOfPatient.ToString()
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

        public async Task<GeneralResponse<string>> NumOfDoctors()
        {
            try
            {
                var num = await _unitOfWork.DoctorRepository.GetAllAsync();
                var numOfDoctors = num.Count();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Data = numOfDoctors.ToString()
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

        public async Task<GeneralResponse<string>> NumOfHospitals()
        {
            try
            {
                var num = await _unitOfWork.HospitalRepository.GetAllAsync();
                var NumOfHospitals = num.Count();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Data = NumOfHospitals.ToString()
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

        public async Task<GeneralResponse<string>> NumOfBands()
        {
            try
            {
                var num = await _unitOfWork.BandRepository.GetAllAsync();
                var NumOfBands = num.Count();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Data = NumOfBands.ToString()
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

        public async Task<GeneralResponse<string>> NumOfAdmins()
        {
            try
            {
                var num = await _unitOfWork.BandRepository.GetAllAsync();
                var NumOfAdmins = num.Count();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Data = NumOfAdmins.ToString()
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
    }
}
