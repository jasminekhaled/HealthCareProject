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
using HealthCare.Core.Models.PatientModule;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public MedicalHistoryServices(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GeneralResponse<PatientResponseDto>> AddMedicalHistory(string email, [FromForm]AddMedicalHistoryDto dto)
        {
            try
            {
                
                var patient = await _unitOfWork.PatientRepository.GetSingleWithIncludesAsync(
                    s => s.Email == email && s.IsEmailConfirmed && s.MedicalHistory == null,
                    a => a.UploadedFile);
                if(patient == null)
                {
                    return new GeneralResponse<PatientResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found with this email!"
                    };
                }

                string format = "d/M/yyyy";
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
                            Message = "Please write a good description with length from 3 to 255 characters, this description may save your life later."
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
                            Message = "Please write a good description with length from 5 to 255 characters, this description may save your life later."
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
                            Message = "Please write a good description with length from 5 to 255 characters, this description may save your life later."
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
                            Message = "Please write a good description with length from 5 to 255 characters, this description may save your life later."
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
                            Message = "Please write a good description with length from 10 to 255 characters, this description may save your life later."
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
                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = true,
                    Message = "New User is fully signedUp sucessfully.",
                    // Data = data
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
                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = true,
                    Message = "New User is fully signedUp sucessfully.",
                    // Data = data
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
                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "New User is fully signedUp sucessfully.",
                    // Data = data
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

        public async Task<GeneralResponse<MedicalHistoryResponseDto>> EditMedicalHistoryByPatient([FromForm]EditMedicalHistoryDto dto)
        {
            try
            {
                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = true,
                    Message = "New User is fully signedUp sucessfully.",
                    // Data = data
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

        public async Task<GeneralResponse<MedicalHistoryResponseDto>> GetMedicalHistory(int medicalHistoryId)
        {
            try
            {
                return new GeneralResponse<MedicalHistoryResponseDto>
                {
                    IsSuccess = true,
                    Message = "New User is fully signedUp sucessfully.",
                    // Data = data
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
