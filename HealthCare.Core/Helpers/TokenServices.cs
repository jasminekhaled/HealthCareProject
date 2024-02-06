﻿using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.Models.AuthModule;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Helpers
{
    public static class TokenServices
    {
        private static readonly string SecretKey = "LPzE9/EeJTNzftqmZ33CEK44BC9z+0XExPkuYo91dE0=";
        private static readonly string Issuer = "SecureApi";
        public static JwtSecurityToken CreateJwtToken(UserTokenDto dto)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>() {
                new Claim("userId", dto.UserId.ToString()),
                new Claim("UserName", dto.userName),
                new Claim("NationalId", dto.NationalId),
                new Claim(ClaimTypes.Role, dto.Role)
            };

            var token = new JwtSecurityToken(Issuer,
              Issuer,
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return token;
        }


        public static RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.Now.AddMinutes(120),
                CreatedOn = DateTime.Now
            };
        }

        public static GeneralResponse<TokenDto> ExtractUserIdFromToken(string jwtToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)), // Replace with your secret key
                    ValidIssuer = "SecureApi",
                    ValidateIssuer = true, // You may want to validate issuer if needed
                    ValidAudience = "SecureApi",
                    ValidateAudience = true, // You may want to validate audience if needed
                    ClockSkew = TimeSpan.Zero // Optional: Set clock skew to zero for better precision
                };
                SecurityToken validatedToken;
                var principal = handler.ValidateToken(jwtToken, validationParameters, out validatedToken);
                var userIdClaim = principal.FindFirst("userId");

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var tokenDto = new TokenDto() { userId = userId };
                    return new GeneralResponse<TokenDto>
                    {
                        Data = tokenDto
                    };
                }
                return new GeneralResponse<TokenDto>
                {
                    IsSuccess = false,
                    Message = "someThing went wrong with the Token"
                };

            }
            catch (SecurityTokenException ex)
            {
                return new GeneralResponse<TokenDto>
                {
                    IsSuccess = false,
                    Message = "Token validation failed",
                    Error = ex
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<TokenDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
    }
}
