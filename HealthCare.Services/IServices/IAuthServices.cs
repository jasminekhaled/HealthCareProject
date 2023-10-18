using HealthCare.Core.DTOS;
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
        //Task<GeneralResponse<UserDto>> SignUp(SignUpDto dto);
        Task<GeneralResponse<RefreshTokenResponse>> RefreshToken(string? RefreshToken);
    }
}
