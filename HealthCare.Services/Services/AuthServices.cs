  using AutoMapper;
using HealthCare.Core.DTOS.AuthModule.ResponseDtos;
using HealthCare.Core.DTOS;
using HealthCare.Core.IRepositories;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Cryptography;
using HealthCare.Core.Models.AuthModule;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using HealthCare.Core.Helpers;
using Microsoft.Extensions.Options;
using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.Models.PatientModule;
using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.Models.DoctorModule;
using Microsoft.AspNetCore.Hosting;
using System.Numerics;
using Microsoft.AspNetCore.Http;

namespace HealthCare.Services.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GeneralResponse<SignUpResponse>> SignUp([FromForm]SignUpRequestDto dto)
        {
            try
            {
                if (await _unitOfWork.PatientRepository.AnyAsync(a => a.UserName == dto.UserName) ||
                    await _unitOfWork.UserRepository.AnyAsync(a => a.UserName == dto.UserName))
                {
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "UserName is already used! Please use another UserName.",
                    };
                }

                var nationalId = await _unitOfWork.CivilRegestrationRepository.SingleOrDefaultAsync(c => c.NationalId == dto.NationalId);
                if (nationalId == null)
                {
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "Wrong National Id.",
                    };
                }
                var patientWithNId = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(c => c.NationalId == dto.NationalId);
                if (patientWithNId != null)
                {
                    if(patientWithNId.IsEmailConfirmed == true && patientWithNId.MedicalHistoryId != null)
                    {
                        return new GeneralResponse<SignUpResponse>
                        {
                            IsSuccess = false,
                            Message = "Patient with this National Id is already Exist.",
                        };
                    }
                    else
                    {
                        _unitOfWork.PatientRepository.Remove(patientWithNId);
                        await _unitOfWork.CompleteAsync();
                    }
                        
                }
                var patientWithEmail = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(c => c.Email == dto.Email);
                if (patientWithEmail != null)
                {
                    if (patientWithEmail.IsEmailConfirmed == true && patientWithEmail.MedicalHistoryId != null)
                    {
                        return new GeneralResponse<SignUpResponse>
                        {
                            IsSuccess = false,
                            Message = "Patient with this Email is already Exist.",
                        };
                    }
                    else
                    {
                        _unitOfWork.PatientRepository.Remove(patientWithEmail);
                        await _unitOfWork.CompleteAsync();
                    }

                }

                if(!dto.PhoneNumber.All(char.IsDigit) || dto.PhoneNumber.Length != 11)
                {
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "Wrong Phone Number"
                    };
                }

                var patient = _mapper.Map<Patient>(dto);
                patient.PassWord = HashingService.GetHash(dto.PassWord);
                patient.FullName = nationalId.Name;
                
                var uploadedFile = new UploadedFile();

                if(dto.Image != null)
                {
                    var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                    List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                    if (!AllowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    {
                        return new GeneralResponse<SignUpResponse>
                        {
                            IsSuccess = false,
                            Message = "Only .jpg and .png Images Are Allowed."
                        };
                    }

                    if (dto.Image.Length > MaxAllowedPosterSize)
                    {
                        return new GeneralResponse<SignUpResponse>
                        {
                            IsSuccess = false,
                            Message = "Max Allowed Size Is 1MB."
                        };
                    }
                    var fakeFileName = Path.GetRandomFileName();

                    uploadedFile.FileName = dto.Image.FileName;
                    uploadedFile.ContentType = dto.Image.ContentType;
                    uploadedFile.StoredFileName = fakeFileName;
                    
                    var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "PatientImages");
                    var path = Path.Combine(directoryPath, fakeFileName);
                    using FileStream fileStream = new(path, FileMode.Create);
                    dto.Image.CopyTo(fileStream);
                    uploadedFile.FilePath = path;
                    
                }
                else
                {
                    uploadedFile.FileName = "DefaultImage.png";
                    uploadedFile.StoredFileName = "DefaultImage";
                    uploadedFile.ContentType = "image/png";
                    uploadedFile.FilePath = "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\DefaultImage";
                }
                await _unitOfWork.UploadedFileRepository.AddAsync(uploadedFile);
                await _unitOfWork.CompleteAsync();
                patient.UploadedFile = uploadedFile;

                var verificationCode = MailServices.RandomString(6);
                if (!await MailServices.SendEmailAsync(dto.Email, "Verification Code", verificationCode))
                {
                    return new GeneralResponse<SignUpResponse>()
                    {
                        IsSuccess = false,
                        Message = "Sending the Verification Code is Failed"
                    };
                }
                    
                patient.VerificationCode = verificationCode;
                await _unitOfWork.PatientRepository.AddAsync(patient);
                await _unitOfWork.CompleteAsync();
                var data = _mapper.Map<SignUpResponse>(patient);
                data.ImagePath = uploadedFile.FilePath;

                return new GeneralResponse<SignUpResponse>
                {
                    IsSuccess = true,
                    Message = "Verification Code has been sent sucessfully",
                    Data = data
                };
            }
            catch(Exception ex)
            {
                return new GeneralResponse<SignUpResponse>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
      
        }
        
        public async Task<GeneralResponse<SignUpResponse>> VerifyEmail(string userName, string verificationCode)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetSingleWithIncludesAsync(s => s.UserName == userName, a=>a.UploadedFile);
                if(patient == null)
                {
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found with this Email"
                    };
                }
                if(patient.VerificationCode != verificationCode)
                {
                    _unitOfWork.PatientRepository.Remove(patient);
                    await _unitOfWork.CompleteAsync();
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "Wrong Verification code"
                    };
                }
                patient.IsEmailConfirmed = true;
                patient.VerificationCode = null;
                _unitOfWork.PatientRepository.Update(patient);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<SignUpResponse>(patient);
                data.ImagePath = patient.UploadedFile.FilePath;

                return new GeneralResponse<SignUpResponse>
                {
                    IsSuccess = true,
                    Message = "email verified sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<SignUpResponse>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<LogInResponse>> Login([FromForm]LoginRequest dto)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(s => s.UserName == dto.UserName, 
                     a => a.UploadedFile);

                var HashedPassword = HashingService.GetHash(dto.PassWord);
                if (user == null || user.Email != dto.Email || user.PassWord != HashedPassword)
                {
                    return new GeneralResponse<LogInResponse>()
                    {
                        IsSuccess = false,
                        Message = "User isnot found!.",
                    };
                }
                
                var data = _mapper.Map<LogInResponse>(user);
                var userToken = _mapper.Map<UserTokenDto>(user);
                userToken.Role = user.Role;
                var Token = TokenServices.CreateJwtToken(userToken);
                data.Token = new JwtSecurityTokenHandler().WriteToken(Token);
                data.ExpiresOn = Token.ValidTo;
                data.Role = user.Role;
                if(user.Role == User.Patient)
                {
                    var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == user.UserName);
                    data.FullName = patient.FullName;
                    data.PhoneNumber = patient.PhoneNumber;
                }
                if (user.Role == User.Doctor)
                {
                    var doctor = await _unitOfWork.DoctorRepository.SingleOrDefaultAsync(s => s.UserName == user.UserName);
                    data.FullName = doctor.FullName;
                }

                var userTokens = await _unitOfWork.RefreshTokenRepository.GetFirstItem(w => w.userId == user.Id && w.ExpiresOn>DateTime.Now);
                if(userTokens != null)
                {
                    data.RefreshToken = userTokens.Token;
                    data.RefreshTokenExpiration = userTokens.ExpiresOn;
                }
                else
                {
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
                    data.RefreshToken = newRefreshToken.Token;
                    data.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
                }


                return new GeneralResponse<LogInResponse>()
                {
                    IsSuccess = true,
                    Message = "LoggedIn Successfully.",
                    Data = data
                };


            }
            catch(Exception ex)
            {
                return new GeneralResponse<LogInResponse>()
                {
                    IsSuccess = false,
                    Message = "Something Went Wrong.",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<string>> ResetPassword([FromForm] ResetPasswordRequestDto dto)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId);

                if(user == null || user.PassWord != HashingService.GetHash(dto.PassWord))
                {
                    return new GeneralResponse<string>()
                    {
                        IsSuccess = false,
                        Message = "Wrong old Password."
                    };
                }
                if(dto.NewPassWord != dto.ConfirmNewPassWord)
                {
                    return new GeneralResponse<string>()
                    {
                        IsSuccess = false,
                        Message = "Your Confirmation Password is different than your NewPassword."
                    };
                }
                var HashNewPassword = HashingService.GetHash(dto.NewPassWord);
                user.PassWord = HashNewPassword;
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CompleteAsync();

                if (user.Role == User.Patient)
                {
                    var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == user.UserName);
                    patient.PassWord = HashNewPassword;
                    _unitOfWork.PatientRepository.Update(patient);
                    _unitOfWork.CompleteAsync();
                }
                if (user.Role == User.Doctor)
                {
                    var doctor = await _unitOfWork.DoctorRepository.SingleOrDefaultAsync(s => s.UserName == user.UserName);
                    doctor.PassWord = HashNewPassword;
                    _unitOfWork.DoctorRepository.Update(doctor);
                    _unitOfWork.CompleteAsync();
                }
                if (user.Role == User.HospitalAdmin)
                {
                    var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.SingleOrDefaultAsync(s => s.UserName == user.UserName);
                    hospitalAdmin.PassWord = HashNewPassword;
                    _unitOfWork.HospitalAdminRepository.Update(hospitalAdmin);
                    _unitOfWork.CompleteAsync();
                }

                return new GeneralResponse<string>()
                {
                    IsSuccess = true,
                    Message = "Password changed Successfully."
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>()
                {
                    IsSuccess = false,
                    Message = "Something Went Wrong.",
                    Error = ex
                };
            }

        }


        public async Task<GeneralResponse<string>> ForgetPassword([FromForm] ForgetPasswordRequestDto dto)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.UserName == dto.UserName);
                if (user == null || user.Email != dto.Email)
                {
                    return new GeneralResponse<string>()
                    {
                        IsSuccess = false,
                        Message = "Wrong Email or UserName."
                    };
                }

                var verificationCode = MailServices.RandomString(6);
                if (!await MailServices.SendEmailAsync(dto.Email, "Verification Code", verificationCode))
                {
                    return new GeneralResponse<string>()
                    {
                        IsSuccess = false,
                        Message = "Sending the Verification Code is Failed"
                    };
                }
                user.VerificationCode = verificationCode;
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>()
                {
                    IsSuccess = true,
                    Message = "Verification code send Successfully."
                };

            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>()
                {
                    IsSuccess = false,
                    Message = "Something Went Wrong.",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<string>> ChangeForgettedPassword(string userName, [FromForm]ChangeForgettedPasswordDto dto)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.UserName == userName);
                if (user == null)
                {
                    return new GeneralResponse<string>()
                    {
                        IsSuccess = false,
                        Message = "no user found, you canot change password"
                    };
                }

                if (user.VerificationCode != dto.VerificationCode)
                {
                    return new GeneralResponse<string>()
                    {
                        IsSuccess = false,
                        Message = "wrong verification code"
                    };
                }

                if (dto.NewPassWord != dto.ConfirmPassWord)
                {
                    return new GeneralResponse<string>()
                    {
                        IsSuccess = false,
                        Message = "Your Confirmation Password is different than your NewPassword."
                    };
                }
                var HashedPassword = HashingService.GetHash(dto.NewPassWord);
                user.PassWord = HashedPassword;
                user.VerificationCode = null;
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CompleteAsync();

                if(user.Role == User.Patient)
                {
                    var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == userName);
                    patient.PassWord = HashedPassword;
                    _unitOfWork.PatientRepository.Update(patient);
                    _unitOfWork.CompleteAsync();
                }
                if (user.Role == User.Doctor)
                {
                    var doctor = await _unitOfWork.DoctorRepository.SingleOrDefaultAsync(s => s.UserName == userName);
                    doctor.PassWord = HashedPassword;
                    _unitOfWork.DoctorRepository.Update(doctor);
                    _unitOfWork.CompleteAsync();
                }
                if (user.Role == User.HospitalAdmin)
                {
                    var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.SingleOrDefaultAsync(s => s.UserName == userName);
                    hospitalAdmin.PassWord = HashedPassword;
                    _unitOfWork.HospitalAdminRepository.Update(hospitalAdmin);
                    _unitOfWork.CompleteAsync();
                }

                return new GeneralResponse<string>()
                {
                    IsSuccess = true,
                    Message = "Password Changed Successfully."
                };

            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>()
                {
                    IsSuccess = false,
                    Message = "Something Went Wrong.",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<RefreshTokenResponse>> RefreshToken(string? RefreshToken)
        {
            try
            {

                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == RefreshToken));

                if (user == null)
                {
                    return new GeneralResponse<RefreshTokenResponse>()
                    {
                        IsSuccess = false,
                        Message = "Invalid Token."
                    };
                }

                var ExistingRefreshToken = await _unitOfWork.RefreshTokenRepository.GetSpecificItem(filter: s => s.userId == user.Id, single: r => r.Token == RefreshToken);

                if (!ExistingRefreshToken.IsActive)
                {
                    return new GeneralResponse<RefreshTokenResponse>()
                    {
                        IsSuccess = false,
                        Message = "Inactive Token."
                    };
                }

                
                var NewRefreshToken = TokenServices.CreateRefreshToken();
                var refreshToken = new RefreshToken
                {
                    Token = NewRefreshToken.Token,
                    ExpiresOn = NewRefreshToken.ExpiresOn,
                    CreatedOn = NewRefreshToken.CreatedOn,
                    userId = user.Id
                };
                _unitOfWork.RefreshTokenRepository.Remove(ExistingRefreshToken);
                await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
                await _unitOfWork.CompleteAsync();

                var userToken= _mapper.Map<UserTokenDto>(user);
                var userRole = await _unitOfWork.UserRoleRepository.SingleOrDefaultAsync(s => s.UserId == user.Id);
                var role = await _unitOfWork.RoleRepository.SingleOrDefaultAsync(s => s.Id == userRole.RoleId);
                userToken.Role = role.Name;
                var NewJwtToken = TokenServices.CreateJwtToken(userToken);

                var data = new RefreshTokenResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(NewJwtToken),
                    RefreshToken = NewRefreshToken.Token,
                    ExpireOn = NewRefreshToken.ExpiresOn
                };

                return new GeneralResponse<RefreshTokenResponse>()
                {
                    IsSuccess = true,
                    Message = "Token Refreshed Successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<RefreshTokenResponse>()
                {
                    IsSuccess = false,
                    Message = "Something Went Wrong.",
                    Error = ex
                };
            }
        }

        public async Task AddNationalId(string nationalId, string name)
        {
            var civilRegestration = new CivilRegestration
            {
                Name = name,
                NationalId = nationalId
            };
            await _unitOfWork.CivilRegestrationRepository.AddAsync(civilRegestration);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<GeneralResponse<string>> WhichRole()
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId);
                if (ThisUser == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }
                var data = ThisUser.Role;

                return new GeneralResponse<string>()
                {
                    IsSuccess = true,
                    Message = "Token Refreshed Successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>()
                {
                    IsSuccess = false,
                    Message = "Something Went Wrong.",
                    Error = ex
                };
            }
        }


    }
}
