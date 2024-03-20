using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AppointmentModule.ResponseDto;
using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.DTOS.AuthModule.ResponseDtos;
using HealthCare.Core.DTOS.BandModule.ResponseDtos;
using HealthCare.Core.DTOS.PatientModule.RequestDto;
using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using HealthCare.Core.Helpers;
using HealthCare.Core.IRepositories;
using HealthCare.Core.IRepositories.PatientModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.PatientModule;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace HealthCare.Services.Services
{
    public class MedicalHistoryServices : IMedicalHistoryServices 
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public MedicalHistoryServices(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork; 
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<GeneralResponse<PatientResponseDto>> AddMedicalHistory(string userName, [FromForm]AddMedicalHistoryDto dto)
        {
            try
            {
                
                var patient = await _unitOfWork.PatientRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == userName && s.IsEmailConfirmed && s.MedicalHistory == null,
                    a => a.UploadedFile);
                if(patient == null)
                {
                    return new GeneralResponse<PatientResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found with this userName!"
                    };
                }

                string format = "mm/dd/yyyy";
                if (!DateTime.TryParseExact(dto.BirthDate, format, null, System.Globalization.DateTimeStyles.None, out DateTime result) && result.TimeOfDay == TimeSpan.Zero)
                {
                    return new GeneralResponse<PatientResponseDto>
                    {
                        IsSuccess = false,
                        Message = "Invalid BirthDate Formate!"
                    };
                }

                if (!dto.FriendPhoneNum.All(char.IsDigit) || dto.FriendPhoneNum.Length != 11)
                {
                    return new GeneralResponse<PatientResponseDto>
                    {
                        IsSuccess = false,
                        Message = "Wrong Phone Number"
                    };
                }
                if (dto.FriendPhoneNum == patient.PhoneNumber)
                {
                    return new GeneralResponse<PatientResponseDto>
                    {
                        IsSuccess = false,
                        Message = "You canot use your phone number again!!"
                    };
                }

                if(dto.AnyDiseases)
                {
                    if(dto.DiseasesDescribtion == null ||
                        dto.DiseasesDescribtion.Length < 3 || dto.DiseasesDescribtion.Length > 255)
                    {
                        return new GeneralResponse<PatientResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Please write a good description for Diseases with length from 3 to 255 characters, this description may save your life later."
                        };
                    }
                }

                if (dto.AnySurgery)
                {
                    if (dto.SurgeryDescribtion == null ||
                        dto.SurgeryDescribtion.Length < 5 || dto.SurgeryDescribtion.Length > 255)
                    {
                        return new GeneralResponse<PatientResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Please write a good description for Surgery with length from 5 to 255 characters, this description may save your life later."
                        };
                    }
                }

                if (dto.AnyAllergy)
                {
                    if (dto.AllergyDescribtion == null ||
                        dto.AllergyDescribtion.Length < 5 || dto.AllergyDescribtion.Length > 255)
                    {
                        return new GeneralResponse<PatientResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Please write a good description for Allergy with length from 5 to 255 characters, this description may save your life later."
                        };
                    }
                }

                if (dto.AnyMedicine)
                {
                    if (dto.MedicineDescribtion == null ||
                        dto.MedicineDescribtion.Length < 5 || dto.MedicineDescribtion.Length > 255)
                    {
                        return new GeneralResponse<PatientResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Please write a good description for Medicine with length from 5 to 255 characters, this description may save your life later."
                        };
                    }
                }

                if (dto.MedicalInsurance)
                {
                    if (dto.MedicalInsuranceDescribtion == null ||
                        dto.MedicalInsuranceDescribtion.Length < 10 || dto.MedicalInsuranceDescribtion.Length > 255)
                    {
                        return new GeneralResponse<PatientResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Please write a good description for MedicalInsurance with length from 10 to 255 characters, this description may save your life later."
                        };
                    }
                }

                if (!dto.Endorsement)
                {
                    return new GeneralResponse<PatientResponseDto>
                    {
                        IsSuccess = false,
                        Message = "you have to agree to continue in the process of creating your account."
                    };
                }
                var medicalHistory = _mapper.Map<MedicalHistory>(dto);
                await _unitOfWork.MedicalHistoryRepository.AddAsync(medicalHistory);
                await _unitOfWork.CompleteAsync();
                patient.MedicalHistory = medicalHistory;
                _unitOfWork.PatientRepository.Update(patient);

                var user = _mapper.Map<User>(patient);
                user.Role = User.Patient;
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.CompleteAsync();
                var role = await _unitOfWork.RoleRepository.SingleOrDefaultAsync(s => s.Name == user.Role);
                var userRole = new UserRole
                {
                    User = user,
                    Role = role
                };
                await _unitOfWork.UserRoleRepository.AddAsync(userRole);
                await _unitOfWork.CompleteAsync();

                var userToken = _mapper.Map<UserTokenDto>(user);
                userToken.Role = User.Patient;
                var Token = TokenServices.CreateJwtToken(userToken);
                var refreshToken = TokenServices.CreateRefreshToken();
                var newRefreshToken = new RefreshToken
                {
                    Token = refreshToken.Token,
                    ExpiresOn = refreshToken.ExpiresOn,
                    CreatedOn = refreshToken.CreatedOn,
                    userId = user.Id
                };
                await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<PatientResponseDto>(patient);
                data.MedicalHistoryResponse = _mapper.Map<MedicalHistoryResponseDto>(medicalHistory);
                data.RefreshToken = refreshToken.Token;
                data.RefreshTokenExpiration = refreshToken.ExpiresOn;
                data.Token = new JwtSecurityTokenHandler().WriteToken(Token);
                data.ExpiresOn = Token.ValidTo;
                data.Role = user.Role;
                if (dto.Gender) { data.MedicalHistoryResponse.GenderType = "Female"; } 
                else { data.MedicalHistoryResponse.GenderType = "Male"; }

                return new GeneralResponse<PatientResponseDto>
                {
                    IsSuccess = true,
                    Message = "New User is fully signedUp sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PatientResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<MedicalHistoryResponseDto>> AddTestFilesToMedicalHistory(int medicalHistoryId, [FromForm]AddFilesDto dto)
        {
            try
            {
                var medicalHistory = await _unitOfWork.MedicalHistoryRepository.GetSingleWithIncludesAsync(
                    s => s.Id == medicalHistoryId,
                    a => a.Patient.UploadedFile, a => a.Patient);
                if (medicalHistory == null)
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Medical History found!"
                    };
                }

                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId);
                if (user == null)
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }

                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.MedicalHistoryId == medicalHistoryId);

                if (user.Role == User.Doctor)
                {
                    var Doctor = await _unitOfWork.DoctorRepository.GetSingleWithIncludesAsync(s => s.UserName == user.UserName);
                    var doctors = await _unitOfWork.AllReservationsRepository.GetSpecificItems(w => w.PatientId == patient.Id, s => s.DoctorId);
                    bool NoneMatch = doctors.All(s => s != Doctor.Id);
                    if (NoneMatch)
                    {
                        return new GeneralResponse<MedicalHistoryResponseDto>
                        {
                            IsSuccess = false,
                            Message = "you donot have permission to modify this medicalHistory!!"
                        };
                    }
                }
                
                if(user.Role == User.Patient && user.UserName != patient.UserName)
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "you donot have permission to modify this medicalHistory!!"
                    };
                }
                
                var MaxAllowedFileSize = _configuration.GetValue<long>("MaxAllowedFileSize");
                List<string> AllowedFileExtenstions = _configuration.GetSection("AllowedFileExtenstions").Get<List<string>>();

                if (!AllowedFileExtenstions.Contains(Path.GetExtension(dto.File.FileName).ToLower()))
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "Only .pdf files Are Allowed."
                    };
                }

                if (dto.File.Length > MaxAllowedFileSize)
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "Max Allowed Size Is 5MB."
                    };
                }
                var fakeFileName = Path.GetRandomFileName();
                var medicalFile = new MedicalHistoryFile()
                {
                    FileName = dto.File.FileName,
                    ContentType = dto.File.ContentType,
                    StoredFileName = fakeFileName,
                    Description = dto.Description,
                    MedicalHistory = medicalHistory,
                    Type = MedicalHistoryFile.Test,
                    AddedBy = user.UserName
                };
                var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "FileUploads", "TestFiles");
                var path = Path.Combine(directoryPath, fakeFileName);
                using FileStream fileStream = new(path, FileMode.Create);
                dto.File.CopyTo(fileStream);
                medicalFile.FilePath = path;

                await _unitOfWork.MedicalHistoryFileRepository.AddAsync(medicalFile);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<MedicalHistoryResponseDto>(medicalHistory);
                var testFiles = await _unitOfWork.MedicalHistoryFileRepository.Where(w => w.MedicalHistoryId == medicalHistoryId && w.Type == MedicalHistoryFile.Test);
                var xrayFiles = await _unitOfWork.MedicalHistoryFileRepository.Where(w => w.MedicalHistoryId == medicalHistoryId && w.Type == MedicalHistoryFile.Xray);
                var testFileDtos = _mapper.Map<List<FileResponseDto>>(testFiles);
                var xrayFileDtos = _mapper.Map<List<FileResponseDto>>(xrayFiles);
                data.TestFiles = testFileDtos;
                data.XrayFiles = xrayFileDtos;
                if (data.Gender) { data.GenderType = "Female"; }
                else { data.GenderType = "Male"; }

                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = true,
                    Message = "New TestFile added sucessfully.",
                     Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<MedicalHistoryResponseDto>> AddXrayFilesToMedicalHistory(int medicalHistoryId, [FromForm]AddFilesDto dto)
        {
            try
            {
                var medicalHistory = await _unitOfWork.MedicalHistoryRepository.GetSingleWithIncludesAsync(
                    s => s.Id == medicalHistoryId,
                    a => a.Patient.UploadedFile, a => a.Patient);
                if (medicalHistory == null)
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Medical History found!"
                    };
                }

                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId);
                if (user == null)
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }

                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.MedicalHistoryId == medicalHistoryId);

                if (user.Role == User.Doctor)
                {
                    var Doctor = await _unitOfWork.DoctorRepository.GetSingleWithIncludesAsync(s => s.UserName == user.UserName);
                    var doctors = await _unitOfWork.AllReservationsRepository.GetSpecificItems(w => w.PatientId == patient.Id, s => s.DoctorId);
                    bool NoneMatch = doctors.All(s => s != Doctor.Id);
                    if (NoneMatch)
                    {
                        return new GeneralResponse<MedicalHistoryResponseDto>
                        {
                            IsSuccess = false,
                            Message = "you donot have permission to modify this medicalHistory!!"
                        };
                    }
                }

                if (user.Role == User.Patient && user.UserName != patient.UserName)
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "you donot have permission to modify this medicalHistory!!"
                    };
                }


                var MaxAllowedFileSize = _configuration.GetValue<long>("MaxAllowedFileSize");
                List<string> AllowedFileExtenstions = _configuration.GetSection("AllowedFileExtenstions").Get<List<string>>();

                if (!AllowedFileExtenstions.Contains(Path.GetExtension(dto.File.FileName).ToLower()))
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "Only .pdf files Are Allowed."
                    };
                }

                if (dto.File.Length > MaxAllowedFileSize)
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "Max Allowed Size Is 5MB."
                    };
                }
                var fakeFileName = Path.GetRandomFileName();
                var medicalFile = new MedicalHistoryFile()
                {
                    FileName = dto.File.FileName,
                    ContentType = dto.File.ContentType,
                    StoredFileName = fakeFileName,
                    Description = dto.Description,
                    MedicalHistory = medicalHistory,
                    Type = MedicalHistoryFile.Xray,
                    AddedBy = user.UserName
                };

                var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "FileUploads", "XrayFiles");
                var path = Path.Combine(directoryPath, fakeFileName);
                using FileStream fileStream = new(path, FileMode.Create);
                dto.File.CopyTo(fileStream);
                medicalFile.FilePath = path;

                await _unitOfWork.MedicalHistoryFileRepository.AddAsync(medicalFile);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<MedicalHistoryResponseDto>(medicalHistory);
                var testFiles = await _unitOfWork.MedicalHistoryFileRepository.Where(w => w.MedicalHistoryId == medicalHistoryId && w.Type == MedicalHistoryFile.Test);
                var xrayFiles = await _unitOfWork.MedicalHistoryFileRepository.Where(w => w.MedicalHistoryId == medicalHistoryId && w.Type == MedicalHistoryFile.Xray);
                var testFileDtos = _mapper.Map<List<FileResponseDto>>(testFiles);
                var xrayFileDtos = _mapper.Map<List<FileResponseDto>>(xrayFiles);
                data.TestFiles = testFileDtos;
                data.XrayFiles = xrayFileDtos;
                if (data.Gender) { data.GenderType = "Female"; }
                else { data.GenderType = "Male"; }

                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = true,
                    Message = "New XrayFile added sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<string>> DeleteFileFromMedicalHistory(int fileId)
        {
            try
            {
                var medicalFile = await _unitOfWork.MedicalHistoryFileRepository.GetByIdAsync(fileId);
                if(medicalFile == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No file found!"
                    };
                }

                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId);
                if (user == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }
                if(user.Role == User.Doctor && medicalFile.AddedBy != user.UserName)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "you donot have the permission to delete this file!"
                    };
                }

                _unitOfWork.MedicalHistoryFileRepository.Remove(medicalFile);
                await _unitOfWork.CompleteAsync();
                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The File is deleted sucessfully."
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
        public async Task<GeneralResponse<MedicalHistoryResponseDto>> GetMedicalHistoryByDoctor(int medicalHistoryId)
        {
            try
            {
                var medicalHistory = await _unitOfWork.MedicalHistoryRepository.GetSingleWithIncludesAsync(
                    s => s.Id == medicalHistoryId,
                    a => a.Patient.UploadedFile, a => a.Patient);
                if (medicalHistory == null)
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Medical History found!"
                    };
                }
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId
                && a.Role == User.Doctor);
                if (user == null)
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
                    };
                }
                var Doctor = await _unitOfWork.DoctorRepository.GetSingleWithIncludesAsync(s => s.UserName == user.UserName);
                var doctors = await _unitOfWork.AllReservationsRepository.GetSpecificItems(w => w.PatientId == medicalHistory.Patient.Id, s => s.DoctorId);
                bool NoneMatch = doctors.All(s => s != Doctor.Id);
                if (NoneMatch)
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "you donot have permission to modify this medicalHistory!!"
                    };
                }



                var data = _mapper.Map<MedicalHistoryResponseDto>(medicalHistory);
                var testFiles = await _unitOfWork.MedicalHistoryFileRepository.Where(w => w.MedicalHistoryId == medicalHistoryId && w.Type == MedicalHistoryFile.Test);
                var xrayFiles = await _unitOfWork.MedicalHistoryFileRepository.Where(w => w.MedicalHistoryId == medicalHistoryId && w.Type == MedicalHistoryFile.Xray);
                var testFileDtos = _mapper.Map<List<FileResponseDto>>(testFiles);
                var xrayFileDtos = _mapper.Map<List<FileResponseDto>>(xrayFiles);
                data.TestFiles = testFileDtos;
                data.XrayFiles = xrayFileDtos;
                if (data.Gender) { data.GenderType = "Female"; }
                else { data.GenderType = "Male"; }

                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = true,
                    Message = "Medical History is displayed sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }



        public async Task<GeneralResponse<MedicalHistoryResponseDto>> GetMedicalHistoryByPatient()
        {
            try
            {

                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role==User.Patient);
                if (user == null)
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No patient Found!"
                    };
                }

                var medicalHistoryId = await _unitOfWork.PatientRepository.WhereSelectTheFirstAsync(
                    s => s.UserName == user.UserName,
                    i => i.MedicalHistoryId);

                var medicalHistory = await _unitOfWork.MedicalHistoryRepository.GetSingleWithIncludesAsync(
                    s => s.Id == medicalHistoryId,
                    a => a.Patient.UploadedFile, a => a.Patient);


                var data = _mapper.Map<MedicalHistoryResponseDto>(medicalHistory);
                var testFiles = await _unitOfWork.MedicalHistoryFileRepository.Where(w => w.MedicalHistoryId == medicalHistoryId && w.Type == MedicalHistoryFile.Test);
                var xrayFiles = await _unitOfWork.MedicalHistoryFileRepository.Where(w => w.MedicalHistoryId == medicalHistoryId && w.Type == MedicalHistoryFile.Xray);
                var testFileDtos = _mapper.Map<List<FileResponseDto>>(testFiles);
                var xrayFileDtos = _mapper.Map<List<FileResponseDto>>(xrayFiles);
                data.TestFiles = testFileDtos;
                data.XrayFiles = xrayFileDtos;
                if (data.Gender) { data.GenderType = "Female"; }
                else { data.GenderType = "Male"; }

                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = true,
                    Message = "Medical History is displayed sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        public async Task<GeneralResponse<MedicalHistoryResponseDto>> EditMedicalHistoryByPatient([FromForm]EditMedicalHistoryDto dto)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.Patient);
                if (user == null)
                {
                    return new GeneralResponse<MedicalHistoryResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No patient Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == user.UserName);
                var medicalHistory = await _unitOfWork.MedicalHistoryRepository.GetSingleWithIncludesAsync(
                    s => s.Id == patient.MedicalHistoryId && s.Patient == patient,
                    a => a.Patient.UploadedFile, a => a.Patient);


                

                if(dto.FriendPhoneNum != null)
                {
                    if (!dto.FriendPhoneNum.All(char.IsDigit) || dto.FriendPhoneNum.Length != 11)
                    {
                        return new GeneralResponse<MedicalHistoryResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Wrong Phone Number"
                        };
                    }
                    if (dto.FriendPhoneNum == patient.PhoneNumber)
                    {
                        return new GeneralResponse<MedicalHistoryResponseDto>
                        {
                            IsSuccess = false,
                            Message = "You canot use your phone number again!!"
                        };
                    }
                }
                
                if (medicalHistory.AnyDiseases == false && dto.AnyDiseases == true)
                {
                    if (dto.DiseasesDescribtion == null ||
                        dto.DiseasesDescribtion.Length < 3 || dto.DiseasesDescribtion.Length > 255)
                    {
                        return new GeneralResponse<MedicalHistoryResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Please write a good description for Diseases with length from 3 to 255 characters, this description may save your life later."
                        };
                    }
                }

                if (medicalHistory.AnySurgery == false && dto.AnySurgery == true)
                {
                    if (dto.SurgeryDescribtion == null ||
                          dto.SurgeryDescribtion.Length < 5 || dto.SurgeryDescribtion.Length > 255)
                    {
                        return new GeneralResponse<MedicalHistoryResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Please write a good description for Surgery with length from 5 to 255 characters, this description may save your life later."
                        };
                    }
                }

                if (medicalHistory.AnyAllergy == false && dto.AnyAllergy == true)
                {
                    if (dto.AllergyDescribtion == null ||
                        dto.AllergyDescribtion.Length < 5 || dto.AllergyDescribtion.Length > 255)
                    {
                        return new GeneralResponse<MedicalHistoryResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Please write a good description for Allergy with length from 5 to 255 characters, this description may save your life later."
                        };
                    }
                }

                if (medicalHistory.AnyMedicine == false && dto.AnyMedicine == true)
                {
                    if (dto.MedicineDescribtion == null ||
                        dto.MedicineDescribtion.Length < 5 || dto.MedicineDescribtion.Length > 255)
                    {
                        return new GeneralResponse<MedicalHistoryResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Please write a good description for Medicines with length from 5 to 255 characters, this description may save your life later."
                        };
                    }
                }

                if (medicalHistory.MedicalInsurance == false && dto.MedicalInsurance == true)
                {
                    if (dto.MedicalInsuranceDescribtion == null ||
                        dto.MedicalInsuranceDescribtion.Length < 10 || dto.MedicalInsuranceDescribtion.Length > 255)
                    {
                        return new GeneralResponse<MedicalHistoryResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Please write a good description for MedicalInsurance with length from 10 to 255 characters, this description may save your life later."
                        };
                    }
                }
                medicalHistory.Address = dto.Address ?? medicalHistory.Address;
                medicalHistory.FriendAddress = dto.FriendAddress ?? medicalHistory.FriendAddress;
                medicalHistory.FriendPhoneNum = dto.FriendPhoneNum ?? medicalHistory.FriendPhoneNum;
                medicalHistory.FriendName = dto.FriendName ?? medicalHistory.FriendName;
                medicalHistory.AnyMedicine = dto.AnyMedicine ?? medicalHistory.AnyMedicine;
                medicalHistory.MedicineDescribtion = dto.MedicineDescribtion ?? medicalHistory.MedicineDescribtion;
                medicalHistory.AnyAllergy = dto.AnyAllergy ?? medicalHistory.AnyAllergy;
                medicalHistory.AllergyDescribtion = dto.AllergyDescribtion ?? medicalHistory.AllergyDescribtion;
                medicalHistory.AnySurgery = dto.AnySurgery ?? medicalHistory.AnySurgery;
                medicalHistory.SurgeryDescribtion = dto.SurgeryDescribtion ?? medicalHistory.SurgeryDescribtion;
                medicalHistory.AnyDiseases = dto.AnyDiseases ?? medicalHistory.AnyDiseases;
                medicalHistory.DiseasesDescribtion = dto.DiseasesDescribtion ?? medicalHistory.DiseasesDescribtion;
                medicalHistory.BirthDate = dto.BirthDate ?? medicalHistory.BirthDate;
                medicalHistory.Gender = dto.Gender ?? medicalHistory.Gender;
                medicalHistory.MedicalInsurance = dto.MedicalInsurance ?? medicalHistory.MedicalInsurance;
                medicalHistory.MedicalInsuranceDescribtion = dto.MedicalInsuranceDescribtion ?? medicalHistory.MedicalInsuranceDescribtion;
                _unitOfWork.MedicalHistoryRepository.Update(medicalHistory);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<MedicalHistoryResponseDto>(medicalHistory);
                var testFiles = await _unitOfWork.MedicalHistoryFileRepository.Where(w => w.MedicalHistoryId == patient.MedicalHistoryId && w.Type == MedicalHistoryFile.Test);
                var xrayFiles = await _unitOfWork.MedicalHistoryFileRepository.Where(w => w.MedicalHistoryId == patient.MedicalHistoryId && w.Type == MedicalHistoryFile.Xray);
                var testFileDtos = _mapper.Map<List<FileResponseDto>>(testFiles);
                var xrayFileDtos = _mapper.Map<List<FileResponseDto>>(xrayFiles);
                data.TestFiles = testFileDtos;
                data.XrayFiles = xrayFileDtos;
                if (data.Gender) { data.GenderType = "Female"; }
                else { data.GenderType = "Male"; }

                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = true,
                    Message = "Medical history Edited sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        
    }
}
