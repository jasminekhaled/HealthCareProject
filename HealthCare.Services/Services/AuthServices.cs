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
                    Token = NewJwtToken,
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


    }
}
