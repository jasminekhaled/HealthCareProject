using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.DTOS.AuthModule.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.IServices
{
    public interface IAuthServices
    {
        Task<GeneralResponse<SignUpResponse>> SignUp([FromForm]SignUpRequestDto dto);
        Task<GeneralResponse<SignUpResponse>> VerifyEmail(string userName, string verificationCode); 
        Task<GeneralResponse<RefreshTokenResponse>> RefreshToken(string? RefreshToken);
        Task<GeneralResponse<LogInResponse>> Login([FromForm]LoginRequest dto);
        Task<GeneralResponse<string>> ResetPassword([FromForm]ResetPasswordRequestDto dto);
        Task<GeneralResponse<string>> ForgetPassword([FromForm]ForgetPasswordRequestDto dto);
        Task<GeneralResponse<string>> ChangeForgettedPassword(string userName, [FromForm]ChangeForgettedPasswordDto dto); // userName from frontend , frontend will save it from the forgetPassword Api
       // Task<GeneralResponse<SuperAdminResponseDto>> AddSuperAdmin([FromForm]SuperAdminRequestDto dto); 
       // Task<GeneralResponse<string>> DeleteSuperAdmin(int superAdminId); 

        Task AddNationalId(string nationalId, string name);
    }
}
