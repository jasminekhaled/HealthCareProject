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

namespace HealthCare.Services.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
         
        public async Task<GeneralResponse<SignUpResponse>> SignUp([FromForm]SignUpRequestDto dto)
        {
            try
            {
                var nationalId = await _unitOfWork.CivilRegestrationRepository.SingleOrDefaultAsync(c => c.NationalId == dto.NationalId);
                if (nationalId == null)
                {
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "Wrong National Id.",
                    };
                }
                if (await _unitOfWork.UserRepository.AnyAsync(check: c => c.NationalId == dto.NationalId && c.RoleId == 3))
                {
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "User with this national Id is already Exist.",
                    };
                }
                if (await _unitOfWork.UserRepository.AnyAsync(check: c => c.Email == dto.Email && c.RoleId == 3))
                {
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "User with this email is already Exist.",
                    };
                }
                
                if (await _unitOfWork.PatientRepository.AnyAsync(check: c => c.NationalId == dto.NationalId && c.IsEmailConfirmed == false))
                {
                    var UnConfirmedPatient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.NationalId == dto.NationalId);
                    _unitOfWork.PatientRepository.Remove(UnConfirmedPatient);
                    await _unitOfWork.CompleteAsync();   
                }
                if (await _unitOfWork.PatientRepository.AnyAsync(check: c => c.Email == dto.Email && c.IsEmailConfirmed == false))
                {
                    var UnConfirmedPatient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.Email == dto.Email);
                    _unitOfWork.PatientRepository.Remove(UnConfirmedPatient);
                    await _unitOfWork.CompleteAsync();
                }

                if(await _unitOfWork.UserRepository.AnyAsync(a => a.UserName == dto.UserName))
                {
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "UserName is already Exist! Please write another UserName.",
                    };
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
                var data = _mapper.Map<SignUpResponse>(patient);
                await _unitOfWork.PatientRepository.AddAsync(patient);
                await _unitOfWork.CompleteAsync();

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

        public async Task<GeneralResponse<VerifyResponse>> VerifyEmail(string email, string verificationCode)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.Email == email);
                if(patient == null)
                {
                    return new GeneralResponse<VerifyResponse>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found with this Email"
                    };
                }
                if(patient.VerificationCode != verificationCode)
                {
                    _unitOfWork.PatientRepository.Remove(patient);
                    await _unitOfWork.CompleteAsync();
                    return new GeneralResponse<VerifyResponse>
                    {
                        IsSuccess = false,
                        Message = "Wrong Verification code"
                    };
                }
                patient.IsEmailConfirmed = true;
                _unitOfWork.PatientRepository.Update(patient);
                await _unitOfWork.CompleteAsync();

                var user = _mapper.Map<User>(patient);
                user.RoleId = 3;
                var DefaultFile = new UploadedFile()
                {
                    FileName = "DefaultImage.png",
                    StoredFileName = "DefaultImage",
                    ContentType = "image/png",
                    FilePath = "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\DefaultImage"

                };
                await _unitOfWork.UploadedFileRepository.AddAsync(DefaultFile);
                user.UploadedFile = DefaultFile;
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = 3
                };
                await _unitOfWork.UserRoleRepository.AddAsync(userRole);
                await _unitOfWork.CompleteAsync();

                var userToken = _mapper.Map<UserTokenDto>(user);
                userToken.Role = "Patient";
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

                var data = _mapper.Map<VerifyResponse>(patient);
                data.RefreshToken = refreshToken.Token;
                data.RefreshTokenExpiration = refreshToken.ExpiresOn;
                data.Token = new JwtSecurityTokenHandler().WriteToken(Token);
                data.ExpiresOn = Token.ValidTo;
                var Role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
                data.Role = Role.Name;
                data.ImagePath = DefaultFile.FilePath;

                return new GeneralResponse<VerifyResponse>
                {
                    IsSuccess = true,
                    Message = "email verified sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<VerifyResponse>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<LogInResponse>> Login([FromForm] LoginRequest dto)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.UserName == dto.UserName);

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
                var userRole = await _unitOfWork.UserRoleRepository.SingleOrDefaultAsync(s => s.UserId == user.Id);
                var role  = await _unitOfWork.RoleRepository.SingleOrDefaultAsync(s => s.Id == userRole.RoleId);
                userToken.Role = role.Name;
                var Token = TokenServices.CreateJwtToken(userToken);
                data.Token = new JwtSecurityTokenHandler().WriteToken(Token);
                data.ExpiresOn = Token.ValidTo;
                var Role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
                data.Role = Role.Name;

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
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.UserName == dto.UserName);
                if(user == null || user.Email != dto.Email || user.PassWord != HashingService.GetHash(dto.PassWord))
                {
                    return new GeneralResponse<string>()
                    {
                        IsSuccess = false,
                        Message = "Wrong Email Or Password or UserName."
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

        public async Task<GeneralResponse<string>> ChangeForgettedPassword([FromForm]ChangeForgettedPasswordDto dto)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.UserName == dto.UserName);
                if (user == null || user.VerificationCode != dto.VerificationCode)
                {
                    return new GeneralResponse<string>()
                    {
                        IsSuccess = false,
                        Message = "Wrong UserName or verificationCode."
                    };
                }
                var HashedPassword = HashingService.GetHash(dto.PassWord);
                user.PassWord = HashedPassword;
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CompleteAsync();


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

        
    }
}
