using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.DTOS.AuthModule.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.IServices
{
    public interface IAuthServices
    {
        Task<GeneralResponse<SignUpResponse>> SignUp(SignUpRequestDto dto);
        Task<GeneralResponse<VerifyResponse>> VerifyEmail(string email ,string verificationCode); 
        Task<GeneralResponse<RefreshTokenResponse>> RefreshToken(string? RefreshToken);
        Task AddNationalId(int nationalId, string name);
    }
}
