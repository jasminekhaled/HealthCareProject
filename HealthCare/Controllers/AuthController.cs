using HealthCare.Services.IServices;
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



        /*[HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authServices.Register(dto);
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


        [HttpPost("GetToken")]
        public async Task<IActionResult> GetTokenAsync(TokenRequest dto)
        {
            var result = await _authServices.GetTokenAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result);

            if (!string.IsNullOrEmpty(result.Data.RefreshToken))
                SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpiration);

            return Ok(result);

        }*/



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
