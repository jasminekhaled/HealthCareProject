using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }


        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpRequestDto dto)
        {
            var result = await _authServices.SignUp(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string email, string verificationCode)
        {
            var result = await _authServices.VerifyEmail(email, verificationCode);
            if (result.Data != null)
            {
                var CookieOptions = new CookieOptions()
                {
                    HttpOnly = true,
                    Expires = result.Data.ExpiresOn
                };
                Response.Cookies.Append("refreshToken", result.Data.RefreshToken, CookieOptions);
            }
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest dto)
        {
            var result = await _authServices.Login(dto);
            if (!result.IsSuccess)
                return BadRequest(result);
            SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpiration);
            return Ok(result);
        }

        
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto dto)
        {
            var result = await _authServices.ResetPassword(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequestDto dto)
        {
            var result = await _authServices.ForgetPassword(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ChangeForgettedPassword")]
        public async Task<IActionResult> ChangeForgettedPassword(ChangeForgettedPasswordDto dto)
        {
            var result = await _authServices.ChangeForgettedPassword(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpPost("AddNationalId")]
        public async Task<IActionResult> AddNationalId(string nationalId, string name)
        {
            await _authServices.AddNationalId(nationalId, name);
                return Ok();

        }


        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var RefreshToken = Request.Cookies["refreshToken"];

            var result = await _authServices.RefreshToken(RefreshToken);

            if (!result.IsSuccess)
                return BadRequest(result);
            if (!string.IsNullOrEmpty(result.Data.RefreshToken))
                SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.ExpireOn);

            return Ok(result);

        }

        private void SetRefreshTokenInCookie(string RefreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", RefreshToken, cookieOptions);
        }
    }
}
