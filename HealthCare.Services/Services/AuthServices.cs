﻿using AutoMapper;
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
               // var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.NationalId == dto.NationalId && s.IsEmailConfirmed);
                if (!await _unitOfWork.CivilRegestrationRepository.AnyAsync(check: c => c.NationalId == dto.NationalId))
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
                        Message = "User with this national Id is already Exist.",
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

                var phoneNumber = Convert.ToInt32(dto.PhoneNumber); 
                if (dto.PhoneNumber.Length != 11 && phoneNumber < 0)
                {
                    return new GeneralResponse<SignUpResponse>
                    {
                        IsSuccess = false,
                        Message = "Wrong Phone Number.",
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
                _unitOfWork.PatientRepository.Update(patient);
                await _unitOfWork.CompleteAsync();

                var user = _mapper.Map<User>(patient);
                user.RoleId = 3;
                user.TableId = patient.Id;
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
                var Token = TokenServices.CreateJwtToken(userToken);
                var refreshToken = CreateRefreshToken();
                var newRefreshToken = new RefreshToken
                {
                    Token = refreshToken.Token,
                    ExpiresOn = refreshToken.ExpiresOn,
                    CreatedOn = refreshToken.CreatedOn,
                    IsActive = true,
                    userId = user.Id
                };
                await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);
                await _unitOfWork.CompleteAsync();
                //user.RefreshTokens.Add(newRefreshToken);
                //_unitOfWork.UserRepository.Update(user);


                var data = _mapper.Map<VerifyResponse>(patient);
                data.RefreshToken = refreshToken.Token;
                data.RefreshTokenExpiration = refreshToken.ExpiresOn;
                data.Token = new JwtSecurityTokenHandler().WriteToken(Token);
                data.ExpiresOn = Token.ValidTo;
                var Role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId);
                data.Role = Role.Name;

                

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


                ExistingRefreshToken.IsActive = false;
                var NewRefreshToken = CreateRefreshToken();
                var refreshToken = new RefreshToken
                {
                    Token = NewRefreshToken.Token,
                    ExpiresOn = NewRefreshToken.ExpiresOn,
                    CreatedOn = NewRefreshToken.CreatedOn,
                    IsActive = true,
                    userId = user.Id
                };
                await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
               // user.RefreshTokens.Add(refreshToken);
                //_unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CompleteAsync();

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
                CreatedOn = DateTime.UtcNow,
                IsActive = true
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

        public async Task<GeneralResponse<VerifyResponse>> LoginAsPatient(LoginAsPatientRequest dto)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.Email == dto.Email);

                var HashedPassword = HashingService.GetHash(dto.PassWord);
                if (patient == null || patient.PassWord != HashedPassword )
                {
                    return new GeneralResponse<VerifyResponse>()
                    {
                        IsSuccess = false,
                        Message = "User isnot found!.",
                    };
                }
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.TableId == patient.Id && s.RoleId == 3);
                if(user == null)
                {
                    _unitOfWork.PatientRepository.Remove(patient);
                    await _unitOfWork.CompleteAsync();

                    return new GeneralResponse<VerifyResponse>()
                    {
                        IsSuccess = false,
                        Message = "User isnot found!.",
                    };
                }
                var data = _mapper.Map<VerifyResponse>(patient);
                var userToken = _mapper.Map<UserTokenDto>(user);
                var Token = TokenServices.CreateJwtToken(userToken);
                data.Token = new JwtSecurityTokenHandler().WriteToken(Token);
                data.ExpiresOn = Token.ValidTo;
                data.Role = "Patient";

                var userTokens = await _unitOfWork.RefreshTokenRepository.GetFirstItem(w => w.userId == user.Id && w.IsActive);
                if(userTokens != null)
                {
                    data.RefreshToken = userTokens.Token;
                    data.RefreshTokenExpiration = userTokens.ExpiresOn;
                }
                else
                {
                    var refreshToken = CreateRefreshToken();

                    var newRefreshToken = new RefreshToken
                    {
                        Token = refreshToken.Token,
                        ExpiresOn = refreshToken.ExpiresOn,
                        CreatedOn = refreshToken.CreatedOn,
                        IsActive = true,
                        userId = user.Id
                    };
                    await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);
                    await _unitOfWork.CompleteAsync();
                    data.RefreshToken = newRefreshToken.Token;
                    data.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
                }


                return new GeneralResponse<VerifyResponse>()
                {
                    IsSuccess = true,
                    Message = "LoggedIn Successfully.",
                    Data = data
                };


            }
            catch(Exception ex)
            {
                return new GeneralResponse<VerifyResponse>()
                {
                    IsSuccess = false,
                    Message = "Something Went Wrong.",
                    Error = ex
                };
            }
        }
    }
}
