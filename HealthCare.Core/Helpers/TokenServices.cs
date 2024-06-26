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


    }
}
