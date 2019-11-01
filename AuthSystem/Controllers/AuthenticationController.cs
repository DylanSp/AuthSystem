using AuthSystem.Data;
using AuthSystem.DTOs;
using AuthSystem.Interfaces;
using AuthSystem.Interfaces.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("v{version:apiVersion}")]
    public class AuthenticationController : ControllerBase
    {
        private IUserManager UserManager { get; }
        private IJwtService JwtService { get; }

        public AuthenticationController(IUserManager userManager, IJwtService jwtService)
        {
            UserManager = userManager;
            JwtService = jwtService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserAuthenticationDTO userAuthentication)
        {
            var username = new Username(userAuthentication.Username);
            var isCorrectPassword =
                await UserManager.ValidatePasswordAsync(username, new PlaintextPassword(userAuthentication.Password));

            if (isCorrectPassword)
            {
                var userId = await UserManager.GetIdForUsernameAsync(username);
                if (!userId.HasValue)
                {
                    // TODO - what to do here? either some sort of error, or user got deleted between validating password and fetching ID
                    return Unauthorized();
                }

                // TODO - make this duration configurable?
                var expirationTime = DateTimeOffset.Now.AddMinutes(15);
                var accessJwt = JwtService.CreateToken(userId.Value, expirationTime);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                };
                Response.Cookies.Append(Constants.ACCESS_TOKEN_COOKIE_NAME, accessJwt.Value, cookieOptions);
                // TODO - construct refresh token, save refresh token, return tokens
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
