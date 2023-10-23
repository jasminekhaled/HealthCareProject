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



        public async Task<GeneralResponse<SignUpResponse>> SignUp(SignUpRequestDto dto)
        {
            try
            {
                if(await _unitOfWork.PatientRepository.AnyAsync(check: c => c.NationalId == dto.NationalId))
                {
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "User with this national Id is already Exist.",
                    };
                }
                if (!await _unitOfWork.CivilRegestrationRepository.AnyAsync(check: c => c.NationalId == dto.NationalId))
                {
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "Wrong National Id.",
                    };
                }
                var phoneNumber = Convert.ToInt32(dto.PhoneNumber); 
                if (dto.PhoneNumber.Length != 11 && phoneNumber < 0)
                {
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "Wrong Phone Number.",
                    };
                }
                if (await _unitOfWork.PatientRepository.AnyAsync(check: c => c.Email == dto.Email))
                {
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "User with this Email is already Exist.",
                    };
                }
                var patient = _mapper.Map<Patient>(dto);
                patient.PassWord = HashingService.GetHash(dto.PassWord);
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
                    Message = "Verification Code is send sucessfully",
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
                        Message = "Wrong Email"
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
                

                var user = _mapper.Map<User>(patient);
                user.RoleId = 3;
                var userToken = _mapper.Map<UserTokenDto>(user);
                var Token = TokenServices.CreateJwtToken(userToken);
                var refreshToken = CreateRefreshToken();
                user.RefreshTokens?.Add(refreshToken);

                var data = _mapper.Map<VerifyResponse>(patient);
                data.RefreshToken = refreshToken.Token;
                data.RefreshTokenExpiration = refreshToken.ExpiresOn;
                data.Token = new JwtSecurityTokenHandler().WriteToken(Token);
                data.ExpiresOn = Token.ValidTo;
                var Role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
                data.Role = Role.Name;

                _unitOfWork.PatientRepository.Update(patient);
                await _unitOfWork.UserRepository.AddAsync(user);
                
                await _unitOfWork.CompleteAsync();

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

                var ExistingRefreshToken = user.RefreshTokens.Single(t => t.Token == RefreshToken);

                if (!ExistingRefreshToken.IsActive)
                {
                    return new GeneralResponse<RefreshTokenResponse>()
                    {
                        IsSuccess = false,
                        Message = "Inactive Token."
                    };
                }


                ExistingRefreshToken.RevokedOn = DateTime.UtcNow;
                var NewRefreshToken = CreateRefreshToken();
                user.RefreshTokens.Add(NewRefreshToken);
                _unitOfWork.UserRepository.Update(user);
                var userToken= _mapper.Map<UserTokenDto>(user);

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

        

        private RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddMinutes(120),
                CreatedOn = DateTime.UtcNow
            };
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
